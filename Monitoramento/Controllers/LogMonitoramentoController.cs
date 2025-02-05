using Microsoft.AspNetCore.Mvc;
using MonitorDeServicos.Dominio.Interface.Aplicacao;

namespace Monitoramento.Controllers
{
    public class LogMonitoramentoController(ILogMonitoramentoAplicacao logMonitoramentoAplicacao) : Controller
    {
        private readonly ILogMonitoramentoAplicacao _logMonitoramentoAplicacao = logMonitoramentoAplicacao;

        // GET: LogMonitoramentoes
        public async Task<IActionResult> Index(int idMonitoramento)
        {
            var listaLogMonitor = await _logMonitoramentoAplicacao.ObterListaPorIdMonitoramento(idMonitoramento, retornarMonitoramento: false);
            return View(listaLogMonitor.OrderByDescending(o => o.DataHora).Take(10));
        }
    }
}
