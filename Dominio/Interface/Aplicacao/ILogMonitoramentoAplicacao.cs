using MonitorDeServicos.Dominio.Entidade;

namespace MonitorDeServicos.Dominio.Interface.Aplicacao
{
    public interface ILogMonitoramentoAplicacao
    {
        Task<List<LogMonitoramento>> ObterTodos(bool retornarMonitoramento = false);
        Task<LogMonitoramento?> ObterPorId(int id, bool retornarMonitoramento = false);
        Task<List<LogMonitoramento>?> ObterListaPorIdMonitoramento(int idMonitoramento, bool retornarMonitoramento);

        #region Operacoes de banco
        Task<bool> Existe(int id);
        Task Adicionar(LogMonitoramento logMonitoramento);
        Task Atualizar(LogMonitoramento logMonitoramento);
        Task Salvar();
        Task Remover(LogMonitoramento logMonitoramento);
        #endregion
    }
}
