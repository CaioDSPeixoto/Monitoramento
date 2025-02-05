using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade;

namespace MonitorDeServicos.Dominio.Interface.Aplicacao
{
    public interface IMonitoramentoAplicacao
    {
        Task<List<Monitoramento>> ObterTodos(bool consultarComWebhook, bool buscarSomenteAtivos);
        Task<Monitoramento?> ObterPorId(int id, bool consultarComWebhook);
        Task<Monitoramento> VerificarStatus(Monitoramento monitoramento);

        #region Operacoes de banco
        Task<bool> Existe(int id);
        Task Adicionar(Monitoramento monitoramento);
        Task Atualizar(Monitoramento monitoramento);
        Task Salvar();
        Task Remover(Monitoramento monitoramento);
        DbSet<Monitoramento> ObterContexto();
        #endregion
    }
}
