using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MonitorDeServicos.Dominio.Entidade.Relacionamento
{
    public class MonitoramentoWebhook
    {
        public int MonitoramentoId { get; set; }

        [ValidateNever]
        public Monitoramento Monitoramento { get; set; } = null!;

        public int WebhookId { get; set; }

        [ValidateNever]
        public Webhook Webhook { get; set; } = null!;
    }
}
