using MonitorDeServicos.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade.Relacionamento;

namespace MonitorDeServicos.Infra.Contexto
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Monitoramento> Monitoramento { get; set; }
        public DbSet<Webhook> Webhooks { get; set; }
        public DbSet<LogMonitoramento> LogsMonitoramento { get; set; }
        public DbSet<ConfiguracaoSistema> ConfiguracaoSistema { get; set; }
        public DbSet<MonitoramentoWebhook> MonitoramentoWebhook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CriacaoDeSeed(modelBuilder);

            ConfiguracaoDeRelacionamentos(modelBuilder);
        }

        private static void CriacaoDeSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfiguracaoSistema>().HasData(
                            new ConfiguracaoSistema
                            {
                                Id = 1,
                                IntervaloMinutos = 5,
                                FalhasParaNotificacao = 3,
                            });
        }

        private static void ConfiguracaoDeRelacionamentos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonitoramentoWebhook>()
                            .HasKey(mw => new { mw.MonitoramentoId, mw.WebhookId }); // Chave composta

            modelBuilder.Entity<MonitoramentoWebhook>()
                .HasOne(mw => mw.Monitoramento)
                .WithMany(m => m.MonitoramentoWebhooks)
                .HasForeignKey(mw => mw.MonitoramentoId);

            modelBuilder.Entity<MonitoramentoWebhook>()
                .HasOne(mw => mw.Webhook)
                .WithMany(w => w.MonitoramentoWebhooks)
                .HasForeignKey(mw => mw.WebhookId);
        }
    }
}
