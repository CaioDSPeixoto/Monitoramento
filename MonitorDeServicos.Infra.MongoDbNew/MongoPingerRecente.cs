using MongoDB.Bson;
using MongoDB.Driver;
using MonitorDeServicos.Dominio.Interface.Factory;

namespace MonitorDeServicos.Infra.MongoDbNew
{
    public class MongoPingerRecente(string connectionString) : IDatabasePinger
    {
        private readonly IMongoClient _client = new MongoClient(connectionString);

        public async Task<bool> PingAsync()
        {
            try
            {
                var database = _client.GetDatabase("admin"); // Conecta ao DB "admin"
                await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
