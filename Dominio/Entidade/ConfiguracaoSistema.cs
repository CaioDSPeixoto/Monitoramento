using MonitorDeServicos.Dominio.Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace MonitorDeServicos.Dominio.Entidade
{
    public class ConfiguracaoSistema : EntidadeBase
    {
        [Display(Name = "Intervalo em minutos para consulta")]
        [Range(1, 60, ErrorMessage = "O campo {0} precisa ser maior que {1} e menor que {2}.")]
        public int IntervaloMinutos { get; set; }

        [Display(Name = "Quantidade de falhas antes de notificar")]
        [Range(1, 10, ErrorMessage = "O campo {0} precisa ser maior que {1} e menor que {2}.")]
        public int FalhasParaNotificacao { get; set; }

        [Display(Name = "Executar consultas em segundo plano?")]
        public bool ExecutarEmSegundoPlano { get; set; } = false;
    }
}
