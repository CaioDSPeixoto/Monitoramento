using MonitorDeServicos.Dominio.Interface.Factory;
using Npgsql;

namespace MonitorDeServicos.Infra
{
    public class PostgresPinger(string connectionString) : IDatabasePinger
    {
        private readonly string _connectionString = connectionString;

        public async Task<bool> PingAsync()
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new NpgsqlCommand("SELECT 1", connection);
                await command.ExecuteScalarAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
