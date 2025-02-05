using MonitorDeServicos.Dominio.Entidade.Base;
using MonitorDeServicos.Dominio.Entidade.Relacionamento;
using MonitorDeServicos.Dominio.Enumerador;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorDeServicos.Dominio.Entidade
{
    public class Monitoramento : EntidadeBase
    {
        /// <summary>
        /// Nome do monitoramento
        /// </summary>
        [Display(Name = "Nome de identificação do serviço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de monitoramento (aplicacao, banco, etc)
        /// </summary>
        [Display(Name = "Tipo do serviço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoDeMonitoramento? Tipo { get; set; } = null;

        /// <summary>
        /// Endereço do monitoramento (URL, IP, etc)
        /// </summary>
        [Display(Name = "Endereço (URL, URI)")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o monitoramento está ativo
        /// </summary>
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
        /// <summary>
        /// Data/hora da última verificação
        /// </summary>
        [Display(Name = "Data da ultima verificação")]
        public DateTime? UltimaVerificacao { get; set; } = null;

        /// <summary>
        /// Status atual (true: online, false: offline)
        /// </summary>
        [Display(Name = "Status da verificação")]
        public bool StatusOnline { get; set; }

        /// <summary>
        /// Data/hora da última verificação
        /// </summary>
        [Display(Name = "Data da ultima notificacao")]
        public DateTime? UltimaNotificacao { get; set; } = null;

        /// <summary>
        /// A cada requisicao falha, é incrementado um
        /// </summary>
        [Display(Name = "Contador de falha")]
        public int ContadorDeFalha { get; set; } = 0;

        #region Relação com Webhook
        public ICollection<MonitoramentoWebhook> MonitoramentoWebhooks { get; set; } = [];

        [NotMapped]
        public List<int?> WebhookIds { get; set; }
        #endregion
    }
}
