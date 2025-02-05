using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade;
using MonitorDeServicos.Dominio.Interface.Aplicacao;
using MonitorDeServicos.Infra.Contexto;

namespace MonitorDeServicos.Aplicacao
{
    public class LogMonitoramentoAplicacao(AppDbContext context) : ILogMonitoramentoAplicacao
    {
        private readonly AppDbContext _context = context;

        public async Task<List<LogMonitoramento>> ObterTodos(bool retornarMonitoramento)
        {
            var dbLogs = _context.LogsMonitoramento;

            if (retornarMonitoramento)
            {
                return await dbLogs.Include(i => i.Monitoramento).ToListAsync();
            }

            return await _context.LogsMonitoramento.ToListAsync();
        }

        public async Task<LogMonitoramento?> ObterPorId(int id, bool retornarMonitoramento)
        {
            var dbLogs = _context.LogsMonitoramento;

            if (retornarMonitoramento)
            {
                return await dbLogs.Include(i => i.Monitoramento).FirstOrDefaultAsync(f => f.Id == id);
            }

            return await _context.LogsMonitoramento.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<LogMonitoramento>?> ObterListaPorIdMonitoramento(int idMonitoramento, bool retornarMonitoramento)
        {
            IQueryable<LogMonitoramento> dbLogs = _context.LogsMonitoramento;

            if (retornarMonitoramento)
            {
                dbLogs = dbLogs.Include(i => i.Monitoramento);
            }

            return await dbLogs.Where(w => w.MonitoramentoId == idMonitoramento).ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.LogsMonitoramento.AnyAsync(a => a.Id == id);
        }

        public async Task Adicionar(LogMonitoramento logMonitoramento)
        {
            await _context.AddAsync(logMonitoramento);
        }

        public Task Atualizar(LogMonitoramento logMonitoramento)
        {
            _context.LogsMonitoramento.Update(logMonitoramento);
            return Task.CompletedTask;
        }

        public async Task Salvar()
        {
            await _context.SaveChangesAsync();
        }

        public Task Remover(LogMonitoramento logMonitoramento)
        {
            _context.LogsMonitoramento.Remove(logMonitoramento);
            return Task.CompletedTask;
        }
    }
}