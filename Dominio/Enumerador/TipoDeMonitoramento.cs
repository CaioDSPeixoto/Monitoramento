using System.ComponentModel.DataAnnotations;

namespace MonitorDeServicos.Dominio.Enumerador
{
    public enum TipoDeMonitoramento
    {
        [Display(Name = "Aplicação")]
        Aplicacao,

        [Display(Name = "Banco de Dados [Postgres]")]
        BancoDeDadosPostgres,

        [Display(Name = "Banco de Dados [MongoDb]")]
        BancoDeDadosMongoDb,

        [Display(Name = "Banco de Dados [SqlServer]")]
        BancoDeDadosSqlServer
    }
}
