using MonitorDeServicos.Dominio.Entidade;

namespace MonitorDeServicos.Dominio.Interface.Aplicacao
{
    public interface IConfiguracaoSistemaAplicacao
    {
        Task<ConfiguracaoSistema?> ObterConfiguracaoSistema();
        Task<ConfiguracaoSistema?> ObterPorId(int id);
        Task Atualizar(ConfiguracaoSistema configSistema);
        Task Salvar();
        Task<bool> Existe(int id);
    }
}
