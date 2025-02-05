namespace MonitorDeServicos.Dominio.Interface.Factory
{
    public interface IDatabasePinger
    {
        Task<bool> PingAsync();
    }
}
