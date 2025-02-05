using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Helper;
using MonitorDeServicos.Dominio.Interface.Aplicacao;

namespace Monitoramento.Controllers
{
    public class WebhookController(IWebhookAplicacao webhookAplicacao, IMonitoramentoWebhookAplicacao monitoramentoWebhookAplicacao) : Controller
    {
        private readonly IWebhookAplicacao _webhookAplicacao = webhookAplicacao;
        private readonly IMonitoramentoWebhookAplicacao _monitoramentoWebhookAplicacao = monitoramentoWebhookAplicacao;

        // GET: Webhook
        public async Task<IActionResult> Index()
        {
            var listaWebhook = await _webhookAplicacao.ObterTodos(buscarSomenteAtivos: false);
            listaWebhook.ForEach(web =>
            {
                web.Url = web.Url.Length > 40 ? web.Url[..40] + "..." : web.Url;
            });
            return View(listaWebhook.OrderByDescending(o => o.Ativo));
        }

        // GET: Webhook/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webhook = await _webhookAplicacao.ObterPorId(id.Value);

            if (webhook == null)
            {
                return NotFound();
            }

            return View(webhook);
        }

        // GET: Webhook/Create
        public IActionResult Create()
        {
            return View(new MonitorDeServicos.Dominio.Entidade.Webhook());
        }

        // POST: Webhook/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonitorDeServicos.Dominio.Entidade.Webhook webhook)
        {
            if (ModelState.IsValid)
            {
                await _webhookAplicacao.Adicionar(webhook);
                await _webhookAplicacao.Salvar();
                return RedirectToAction(nameof(Index));
            }
            return View(webhook);
        }

        // GET: Webhook/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webhook = await _webhookAplicacao.ObterPorId(id.Value);

            if (webhook == null)
            {
                return NotFound();
            }
            return View(webhook);
        }

        // POST: Webhook/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonitorDeServicos.Dominio.Entidade.Webhook webhook)
        {
            if (id != webhook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _webhookAplicacao.Atualizar(webhook);
                    await _webhookAplicacao.Salvar();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _webhookAplicacao.Existe(webhook.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(webhook);
        }

        // GET: Webhook/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webhook = await _webhookAplicacao.ObterPorId(id.Value);

            if (webhook == null)
            {
                return NotFound();
            }

            var relacionamentoComMonitoramento = await _monitoramentoWebhookAplicacao.ObterPorIdWebhook(webhook.Id, consultarComWebhook: false);

            if (relacionamentoComMonitoramento != null && relacionamentoComMonitoramento.Count != 0)
            {
                MessageHelper.Error(TempData, "Existe aplicações que estão utilizando esse webhook. Remova os vínculos antes de tentar excluir.");
                return RedirectToAction("Index", id);
            }

            return View(webhook);
        }

        // POST: Webhook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var webhook = await _webhookAplicacao.ObterPorId(id);

            if (webhook != null)
            {
                await _webhookAplicacao.Remover(webhook);
            }

            await _webhookAplicacao.Salvar();
            return RedirectToAction(nameof(Index));
        }
    }
}
