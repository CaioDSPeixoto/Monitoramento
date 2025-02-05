using MongoDB.Driver;
using MonitorDeServicos.Dominio.Interface.Factory;

namespace MonitorDeServicos.Infra.MongoDbOld
{
    public class MongoPingerAntigo(string connectionString) : IDatabasePinger
    {
        private readonly MongoClient _client = new MongoClient(connectionString);

        public async Task<bool> PingAsync()
        {
            try
            {
                var databaseNames = await _client.ListDatabaseNamesAsync();
                return databaseNames.Any();
            }
            catch
            {
                throw;
            }
        }
    }
}
