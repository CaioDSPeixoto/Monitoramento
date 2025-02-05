using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Aplicacao.Factory;
using MonitorDeServicos.Dominio.Entidade;
using MonitorDeServicos.Dominio.Enumerador;
using MonitorDeServicos.Dominio.Helper;
using MonitorDeServicos.Dominio.Interface.Aplicacao;
using MonitorDeServicos.Infra.Contexto;
using System.Diagnostics;

namespace MonitorDeServicos.Aplicacao
{
    public class MonitoramentoAplicacao(AppDbContext context,
        IWebhookAplicacao webhookAplicacao,
        IConfiguracaoSistemaAplicacao configSistemaAplicacao,
        ILogMonitoramentoAplicacao logMonitoramentoAplicacao,
        DatabasePingerFactory databaseFactory) : IMonitoramentoAplicacao
    {
        private readonly AppDbContext _context = context;
        private readonly IWebhookAplicacao _webhookAplicacao = webhookAplicacao;
        private readonly IConfiguracaoSistemaAplicacao _configSistemaAplicacao = configSistemaAplicacao;
        private readonly ILogMonitoramentoAplicacao _logMonitoramentoAplicacao = logMonitoramentoAplicacao;
        private readonly DatabasePingerFactory _databaseFactory = databaseFactory;

        public async Task<List<Monitoramento>> ObterTodos(bool consultarComWebhook, bool buscarSomenteAtivos)
        {
            IQueryable<Monitoramento> query = _context.Monitoramento;

            if (consultarComWebhook)
            {
                query = query.Include(i => i.MonitoramentoWebhooks).ThenInclude(t => t.Webhook);
            }

            if (buscarSomenteAtivos)
            {
                query = query.Where(w => w.Ativo);
            }

            return await query.ToListAsync();
        }

        public async Task<Monitoramento?> ObterPorId(int id, bool consultarComWebhook)
        {
            IQueryable<Monitoramento> query = _context.Monitoramento;

            if (consultarComWebhook)
            {
                query = query
                    .Include(i => i.MonitoramentoWebhooks)
                    .ThenInclude(t => t.Webhook);
            }

            return await query.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Monitoramento> VerificarStatus(Monitoramento monitoramento)
        {
            Stopwatch tempoExecucao = new();
            tempoExecucao.Start();

            bool statusDaAplicacao = false;
            string mensagemErro = string.Empty;

            try
            {
                if (monitoramento.Tipo == TipoDeMonitoramento.Aplicacao)
                {
                    using (var client = new HttpClient())
                    {
                        var urlTratada = UrlHelper.NormalizeUrl(monitoramento.Endereco);
                        var response = await client.GetAsync(urlTratada);
                        statusDaAplicacao = response.IsSuccessStatusCode;

                        return await ValidarResultado(monitoramento, _webhookAplicacao.MontarDescricaoPorServico(monitoramento), statusDaAplicacao);
                    }
                }
                else if (monitoramento.Tipo == TipoDeMonitoramento.BancoDeDadosPostgres)
                {
                    var postgresPinger = _databaseFactory.CreatePostgresPinger(monitoramento.Endereco);
                    statusDaAplicacao = await postgresPinger.PingAsync();

                    return await ValidarResultado(monitoramento, _webhookAplicacao.MontarDescricaoPorServico(monitoramento), statusDaAplicacao);
                }
                else if (monitoramento.Tipo == TipoDeMonitoramento.BancoDeDadosMongoDb)
                {
                    bool tentarVersaoAntiga = false;

                    try
                    {
                        var mongoDbPinger = _databaseFactory.CreateMongoPinger(monitoramento.Endereco, usarVersaoAntiga: false);
                        statusDaAplicacao = await mongoDbPinger.PingAsync();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("version"))
                        {
                            tentarVersaoAntiga = true;
                        }
                        else
                        {
                            mensagemErro = ex.Message;
                        }
                    }

                    if (tentarVersaoAntiga)
                    {
                        try
                        {
                            var mongoDbPinger = _databaseFactory.CreateMongoPinger(monitoramento.Endereco, usarVersaoAntiga: true);
                            statusDaAplicacao = await mongoDbPinger.PingAsync();
                        }
                        catch (Exception ex)
                        {
                            mensagemErro = ex.Message;
                        }
                    }

                    return await ValidarResultado(monitoramento, _webhookAplicacao.MontarDescricaoPorServico(monitoramento), statusDaAplicacao);
                }

                return await ValidarResultado(monitoramento, _webhookAplicacao.MontarDescricaoPorServico(monitoramento), false);
            }
            catch (Exception ex)
            {
                mensagemErro = ex.Message;
                return await ValidarResultado(monitoramento, _webhookAplicacao.MontarDescricaoPorServico(monitoramento), false);
            }
            finally
            {
                tempoExecucao.Stop();
                await AdicionarLogDaAplicacao(monitoramento, tempoExecucao, statusDaAplicacao, mensagemErro);
            }
        }

        private async Task AdicionarLogDaAplicacao(Monitoramento monitoramento, Stopwatch tempoExecucao, bool statusDaAplicacao, string mensagemErro)
        {
            var log = new LogMonitoramento()
            {
                DataHora = DateTime.Now,
                MonitoramentoId = monitoramento.Id,
                TempoExecucao = tempoExecucao.Elapsed,
                Ativo = statusDaAplicacao,
                Mensagem = mensagemErro
            };

            await _logMonitoramentoAplicacao.Adicionar(log);
            await _logMonitoramentoAplicacao.Salvar();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Monitoramento.AnyAsync(a => a.Id == id);
        }

        public async Task Adicionar(Monitoramento monitoramento)
        {
            await _context.AddAsync(monitoramento);
        }

        public Task Atualizar(Monitoramento monitoramento)
        {
            _context.Monitoramento.Update(monitoramento);
            return Task.CompletedTask;
        }

        public async Task Salvar()
        {
            await _context.SaveChangesAsync();
        }

        public Task Remover(Monitoramento monitoramento)
        {
            _context.Monitoramento.Remove(monitoramento);
            return Task.CompletedTask;
        }

        public DbSet<Monitoramento> ObterContexto()
        {
            return _context.Monitoramento;
        }

        private async Task<Monitoramento> ValidarResultado(Monitoramento monitoramento, string mensagem, bool resultado)
        {
            monitoramento.StatusOnline = resultado;
            monitoramento.UltimaVerificacao = DateTime.Now;
            monitoramento.ContadorDeFalha = monitoramento.StatusOnline ? 0 : monitoramento.ContadorDeFalha + 1;

            if (!resultado && await VerificarSeDeveAlertar(monitoramento))
            {
                var listaWebHooks = monitoramento.MonitoramentoWebhooks
                    .Select(s => s.Webhook)
                    .Where(w => w.Ativo)
                    .ToList();

                await _webhookAplicacao.EnviarMensagem(mensagem, listaWebHooks);

                monitoramento.UltimaNotificacao = DateTime.Now;
                monitoramento.ContadorDeFalha = 0;
            }

            return monitoramento;
        }

        private async Task<bool> VerificarSeDeveAlertar(Monitoramento monitoramento)
        {
            if (monitoramento.MonitoramentoWebhooks?.Count == 0)
                return false;

            var configSistema = await _configSistemaAplicacao.ObterConfiguracaoSistema();

            if (configSistema == null)
                return false;

            return monitoramento.ContadorDeFalha >= configSistema.FalhasParaNotificacao;
        }
    }
}
