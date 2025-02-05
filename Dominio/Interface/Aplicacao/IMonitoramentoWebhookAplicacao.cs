using MonitorDeServicos.Dominio.Entidade.Relacionamento;

namespace MonitorDeServicos.Dominio.Interface.Aplicacao
{
    public interface IMonitoramentoWebhookAplicacao
    {
        Task<List<MonitoramentoWebhook>> ObterPorIdMonitoramento(int id, bool consultarComWebhook);
        Task<List<MonitoramentoWebhook>> ObterPorIdWebhook(int id, bool consultarComWebhook);
        Task Salvar();
        Task Remover(MonitoramentoWebhook monitoramentoWebhook);
        Task RemoverEmLote(List<MonitoramentoWebhook> monitoramentosWebhook);
    }
}
