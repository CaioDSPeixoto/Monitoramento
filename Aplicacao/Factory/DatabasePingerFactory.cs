using MonitorDeServicos.Dominio.Interface.Factory;
using MonitorDeServicos.Infra;
using MonitorDeServicos.Infra.MongoDbNew;
using MonitorDeServicos.Infra.MongoDbOld;

namespace MonitorDeServicos.Aplicacao.Factory
{
    public class DatabasePingerFactory
    {
        public IDatabasePinger CreatePostgresPinger(string connectionString)
        {
            return new PostgresPinger(connectionString);
        }

        public IDatabasePinger CreateMongoPinger(string connectionString, bool usarVersaoAntiga)
        {
            return usarVersaoAntiga
                ? new MongoPingerAntigo(connectionString)
                : new MongoPingerRecente(connectionString);
        }
    }
}
