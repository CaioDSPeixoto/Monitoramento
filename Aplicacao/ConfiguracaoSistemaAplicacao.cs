using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade;
using MonitorDeServicos.Dominio.Interface.Aplicacao;
using MonitorDeServicos.Infra.Contexto;

namespace MonitorDeServicos.Aplicacao
{
    public class ConfiguracaoSistemaAplicacao(AppDbContext context) : IConfiguracaoSistemaAplicacao
    {
        private readonly AppDbContext _context = context;

        public async Task<ConfiguracaoSistema?> ObterConfiguracaoSistema()
        {
            return await _context.ConfiguracaoSistema.FirstOrDefaultAsync();
        }

        public async Task<ConfiguracaoSistema?> ObterPorId(int id)
        {
            return await _context.ConfiguracaoSistema.FirstOrDefaultAsync(f => f.Id == id);
        }

        public Task Atualizar(ConfiguracaoSistema configSistema)
        {
            _context.ConfiguracaoSistema.Update(configSistema);
            return Task.CompletedTask;
        }
        public async Task Salvar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.ConfiguracaoSistema.AnyAsync(a => a.Id == id);
        }
    }
}
