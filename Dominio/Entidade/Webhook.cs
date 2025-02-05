using MonitorDeServicos.Dominio.Entidade.Base;
using MonitorDeServicos.Dominio.Entidade.Relacionamento;
using System.ComponentModel.DataAnnotations;

namespace MonitorDeServicos.Dominio.Entidade
{
    public class Webhook : EntidadeBase
    {
        /// <summary>
        /// Nome do webhook
        /// </summary>
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Nome do responsável pelo webhook
        /// </summary>
        [Display(Name = "Nome do Responsável")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string NomeResponsavel { get; set; } = string.Empty;

        /// <summary>
        /// Url do webhook
        /// </summary>
        [Display(Name = "Url")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Se o webhook está ativo
        /// </summary>
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        public ICollection<MonitoramentoWebhook> MonitoramentoWebhooks { get; set; } = [];
    }
}
