using MonitorDeServicos.Dominio.Entidade;

namespace MonitorDeServicos.Dominio.Interface.Aplicacao
{
    public interface IWebhookAplicacao
    {
        Task EnviarMensagem(string mensagem, Webhook webhook);
        Task EnviarMensagem(string mensagem, ICollection<Webhook> webhooks);
        Task<List<Webhook>> ObterTodos(bool buscarSomenteAtivos);
        string MontarDescricaoPorServico(Monitoramento monitoramento);
        Task<Webhook?> ObterPorId(int id);

        #region Operacoes de banco
        Task<bool> Existe(int id);
        Task Adicionar(Webhook webhook);
        Task Atualizar(Webhook webhook);
        Task Salvar();
        Task Remover(Webhook webhook);
        #endregion
    }
}
