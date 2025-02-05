using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade;
using MonitorDeServicos.Dominio.Interface.Aplicacao;
using MonitorDeServicos.Infra.Contexto;
using System.Text;

namespace MonitorDeServicos.Aplicacao
{
    public class WebhookAplicacao(AppDbContext context) : IWebhookAplicacao
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Webhook>> ObterTodos(bool buscarSomenteAtivos)
        {
            IQueryable<Webhook> query = _context.Webhooks;

            if (buscarSomenteAtivos)
            {
                query = query.Where(w => w.Ativo);
            }

            return await query.ToListAsync();
        }

        public string MontarDescricaoPorServico(Monitoramento monitoramento)
        {
            return $"[Servico] - {monitoramento.Nome} acessivel por [{monitoramento.Endereco}]";
        }

        public async Task EnviarMensagem(string mensagem, Webhook webhook)
        {
            try
            {
                await EnviarMensagemParaWebhook(mensagem, webhook);
            }
            catch (Exception)
            { }
        }

        public async Task EnviarMensagem(string mensagem, ICollection<Webhook> webhooks)
        {
            foreach (var webhook in webhooks)
            {
                try
                {
                    await EnviarMensagemParaWebhook(mensagem, webhook);
                }
                catch (Exception)
                { }
            }
        }

        public async Task<Webhook?> ObterPorId(int id)
        {
            return await _context.Webhooks.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Webhooks.AnyAsync(a => a.Id == id);
        }

        public async Task Adicionar(Webhook webhook)
        {
            await _context.AddAsync(webhook);
        }

        public Task Atualizar(Webhook webhook)
        {
            _context.Webhooks.Update(webhook);
            return Task.CompletedTask;
        }

        public async Task Salvar()
        {
            await _context.SaveChangesAsync();
        }

        public Task Remover(Webhook webhook)
        {
            _context.Webhooks.Remove(webhook);
            return Task.CompletedTask;
        }

        private static async Task EnviarMensagemParaWebhook(string mensagem, Webhook webhook)
        {
            try
            {
                mensagem = VerificandoTextoDaMensagem(mensagem);

                var payload = new
                {
                    text = string.Concat($"Alerta para {webhook.NomeResponsavel}, ", mensagem)
                };

                var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(webhook.Url, content);
                }
            }
            catch (Exception)
            {
                // não faz nada em caso de erro
            }
        }

        private static string VerificandoTextoDaMensagem(string mensagem)
        {
            if (string.IsNullOrEmpty(mensagem))
                mensagem = "ocorreu um erro, verifique os serviços que estão com falha no monitoramento.";

            if (mensagem.Contains("[Servico]"))
            {
                mensagem = mensagem.Replace("[Servico] -", string.Empty);
                mensagem = string.Concat(mensagem, $" encontra-se indisponível, verificação ocorreu as {DateTime.Now}.");
            }

            return mensagem;
        }
    }
}
