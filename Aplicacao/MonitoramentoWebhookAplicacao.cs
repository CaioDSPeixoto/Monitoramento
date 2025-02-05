using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade.Relacionamento;
using MonitorDeServicos.Dominio.Interface.Aplicacao;
using MonitorDeServicos.Infra.Contexto;

namespace MonitorDeServicos.Aplicacao
{
    public class MonitoramentoWebhookAplicacao(AppDbContext context) : IMonitoramentoWebhookAplicacao
    {
        private readonly AppDbContext _context = context;

        public async Task<List<MonitoramentoWebhook>> ObterPorIdMonitoramento(int id, bool consultarComWebhook)
        {
            IQueryable<MonitoramentoWebhook> query = _context.MonitoramentoWebhook;

            if (consultarComWebhook)
            {
                query = query
                    .Include(i => i.Webhook);
            }

            return await query.Where(f => f.MonitoramentoId == id).ToListAsync();
        }

        public async Task<List<MonitoramentoWebhook>> ObterPorIdWebhook(int id, bool consultarComWebhook)
        {
            IQueryable<MonitoramentoWebhook> query = _context.MonitoramentoWebhook;

            if (consultarComWebhook)
            {
                query = query
                    .Include(i => i.Webhook);
            }

            return await query.Where(f => f.WebhookId == id).ToListAsync();
        }

        public async Task Salvar()
        {
            await _context.SaveChangesAsync();
        }

        public Task Remover(MonitoramentoWebhook monitoramentoWebhook)
        {
            _context.MonitoramentoWebhook.Remove(monitoramentoWebhook);
            return Task.CompletedTask;
        }

        public Task RemoverEmLote(List<MonitoramentoWebhook> monitoramentosWebhook)
        {
            _context.MonitoramentoWebhook.RemoveRange(monitoramentosWebhook);
            return Task.CompletedTask;
        }
    }
}
