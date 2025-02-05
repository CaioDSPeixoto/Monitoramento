using MonitorDeServicos.Dominio.Entidade.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MonitorDeServicos.Dominio.Entidade
{
    public class LogMonitoramento : EntidadeBase
    {
        /// <summary>
        /// Data/hora do erro
        /// </summary>
        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Tempo de execucao da requisicao
        /// </summary>
        [Display(Name = "Tempo de Execução")]
        public TimeSpan TempoExecucao { get; set; }

        /// <summary>
        /// Mensagem de erro
        /// </summary>
        [Display(Name = "Mensagem de Erro")]
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o monitoramento está ativo ou não
        /// </summary>
        [Display(Name = "Status")]
        public bool? Ativo { get; set; } = true;

        #region Vinculo com monitoramento
        /// <summary>
        /// Id do monitoramento (FK)
        /// </summary>
        public int MonitoramentoId { get; set; }

        /// <summary>
        ///  Relação com monitoramento
        /// </summary>
        [ValidateNever]
        public Monitoramento Monitoramento { get; set; }
        #endregion
    }
}
