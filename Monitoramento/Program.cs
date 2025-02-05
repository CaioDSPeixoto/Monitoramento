using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Aplicacao;
using MonitorDeServicos.Aplicacao.Factory;
using MonitorDeServicos.Dominio.Interface.Aplicacao;
using MonitorDeServicos.Infra.Contexto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext da aplicação (banco SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Monitoramento"));
});

builder.Services.AddScoped<IMonitoramentoAplicacao, MonitoramentoAplicacao>();
builder.Services.AddScoped<ILogMonitoramentoAplicacao, LogMonitoramentoAplicacao>();
builder.Services.AddScoped<IWebhookAplicacao, WebhookAplicacao>();
builder.Services.AddScoped<IConfiguracaoSistemaAplicacao, ConfiguracaoSistemaAplicacao>();
builder.Services.AddScoped<IMonitoramentoWebhookAplicacao, MonitoramentoWebhookAplicacao>();

builder.Services.AddSingleton<DatabasePingerFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Monitoramento}/{action=Dashboard}/{id?}")
    .WithStaticAssets();


app.Run();
