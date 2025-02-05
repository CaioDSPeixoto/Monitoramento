using Monitoramento.Models.Entidades;
using Monitoramento.ORM.Context;

namespace Hangfire
{
    public class MonitoramentoService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MonitoramentoService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var monitoramentos = dbContext.Monitoramentos.Where(m => m.Ativo).ToList();

                    foreach (var item in monitoramentos)
                    {
                        try
                        {
                            // Realizar consulta (ping ou conexão)
                            bool isOnline = await VerificarStatus(item);
                            item.StatusOnline = isOnline;
                            item.UltimaVerificacao = DateTime.Now;

                            if (!isOnline)
                            {
                                await AcionarWebhooks(dbContext);
                            }

                            dbContext.Monitoramentos.Update(item);
                        }
                        catch (Exception ex)
                        {
                            dbContext.LogsMonitoramento.Add(new LogMonitoramento
                            {
                                MonitoramentoId = item.Id,
                                DataHora = DateTime.Now,
                                MensagemErro = ex.Message
                            });
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<bool> VerificarStatus(Monitoramento monitoramento)
        {
            // Implementação do "ping" ou teste de conexão
            return true;
        }

        private async Task AcionarWebhooks(MonitoramentoDbContext dbContext)
        {
            var webhooks = dbContext.Webhooks.Where(w => w.Ativo).ToList();
            foreach (var webhook in webhooks)
            {
                // Enviar notificação ao webhook
            }
        }
    }
}
