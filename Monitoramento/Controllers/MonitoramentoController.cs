using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade.Relacionamento;
using MonitorDeServicos.Dominio.Interface.Aplicacao;

namespace Monitoramento.Controllers
{
    public class MonitoramentoController(IMonitoramentoAplicacao monitoramentoAplicacao,
        IWebhookAplicacao webhookAplicacao,
        IConfiguracaoSistemaAplicacao configSistemaAplicacao,
        IMonitoramentoWebhookAplicacao monitoramentoWebhookAplicacao) : Controller
    {

        private readonly IMonitoramentoAplicacao _monitoramentoAplicacao = monitoramentoAplicacao;
        private readonly IWebhookAplicacao _webhookAplicacao = webhookAplicacao;
        private readonly IConfiguracaoSistemaAplicacao _configSistemaAplicacao = configSistemaAplicacao;
        private readonly IMonitoramentoWebhookAplicacao _monitoramentoWebhookAplicacao = monitoramentoWebhookAplicacao;

        /// <summary>
        /// GET: Monitoramento
        /// Tela de listagem de monitoramentos (/Monitoramento/Index)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var listaAplicacoes = await _monitoramentoAplicacao.ObterTodos(consultarComWebhook: false, buscarSomenteAtivos: false);
            listaAplicacoes.ForEach(web =>
            {
                web.Endereco = web.Endereco.Length > 30 ? web.Endereco[..30] + "..." : web.Endereco;
            });

            return View(listaAplicacoes.OrderByDescending(o => o.Ativo));
        }

        /// <summary>
        /// Tela que realiza os testes de ping (/Dashboard)
        /// Tela de monitoramento
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Dashboard()
        {
            await AdicionarViewModelConfigSistema();

            var listaAplicacoes = await _monitoramentoAplicacao.ObterTodos(consultarComWebhook: false, buscarSomenteAtivos: true);
            listaAplicacoes.ForEach(web =>
            {
                web.Endereco = web.Endereco.Length > 30 ? web.Endereco[..30] + "..." : web.Endereco;
            });

            return View(listaAplicacoes.OrderBy(o => o.StatusOnline));
        }

        // GET: Monitoramento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitoramento = await _monitoramentoAplicacao.ObterPorId(id.Value, consultarComWebhook: false);

            if (monitoramento == null)
            {
                return NotFound();
            }

            return View(monitoramento);
        }

        // Consulta e atualiza o status
        public async Task<IActionResult> Consultar(int id)
        {
            var item = await _monitoramentoAplicacao.ObterPorId(id, consultarComWebhook: true);

            if (item == null)
            {
                return NotFound();
            }

            try
            {
                // Lógica para verificar o status (simula um ping/conexão)
                item = await _monitoramentoAplicacao.VerificarStatus(item);

                await _monitoramentoAplicacao.Atualizar(item);
                await _monitoramentoAplicacao.Salvar();

                return Json(new { success = true, statusOnline = item.StatusOnline });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // Verifica todos os itens
        public async Task<IActionResult> ConsultarTodos()
        {
            var monitoramentos = await _monitoramentoAplicacao.ObterTodos(consultarComWebhook: true, buscarSomenteAtivos: true);

            for (int i = 0; i < monitoramentos.Count; i++)
            {
                monitoramentos[i] = await _monitoramentoAplicacao.VerificarStatus(monitoramentos[i]);

                await _monitoramentoAplicacao.Atualizar(monitoramentos[i]);
            }

            await _monitoramentoAplicacao.Salvar();

            return RedirectToAction(nameof(Index));
        }


        // GET: Monitoramento/Create
        public async Task<IActionResult> Create()
        {
            await AdicionarViewModelDosWebhook();
            return View(new MonitorDeServicos.Dominio.Entidade.Monitoramento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonitorDeServicos.Dominio.Entidade.Monitoramento monitoramento)
        {
            if (ModelState.IsValid)
            {
                // Adicionar os webhooks selecionados
                if (monitoramento.WebhookIds != null && monitoramento.WebhookIds.Count != 0)
                {
                    foreach (var webhookId in monitoramento.WebhookIds)
                    {
                        if (webhookId.HasValue)
                        {
                            monitoramento.MonitoramentoWebhooks.Add(new MonitoramentoWebhook
                            {
                                WebhookId = webhookId.Value
                            });
                        }
                    }
                }

                await _monitoramentoAplicacao.Adicionar(monitoramento);
                await _monitoramentoAplicacao.Salvar();

                return RedirectToAction(nameof(Index));
            }

            await AdicionarViewModelDosWebhook();
            return View(monitoramento);
        }

        // GET: Monitoramento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //todo: testar a edicao dos webhooks
            var monitoramento = await _monitoramentoAplicacao.ObterPorId(id.Value, consultarComWebhook: true);
            if (monitoramento == null)
            {
                return NotFound();
            }

            monitoramento.WebhookIds = monitoramento.MonitoramentoWebhooks
                                        .Select(mw => (int?)mw.WebhookId)
                                        .ToList();

            await AdicionarViewModelDosWebhook();
            return View(monitoramento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonitorDeServicos.Dominio.Entidade.Monitoramento monitoramento)
        {
            if (id != monitoramento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualizar os webhooks
                    var monitoramentoDb = await _monitoramentoAplicacao.ObterPorId(monitoramento.Id, consultarComWebhook: true);

                    monitoramentoDb.MonitoramentoWebhooks.Clear();

                    if (monitoramento.WebhookIds != null && monitoramento.WebhookIds.Any())
                    {
                        foreach (var webhookId in monitoramento.WebhookIds)
                        {
                            if (webhookId.HasValue)
                            {
                                monitoramentoDb.MonitoramentoWebhooks.Add(new MonitoramentoWebhook
                                {
                                    WebhookId = webhookId.Value
                                });
                            }
                        }
                    }

                    monitoramentoDb.Nome = monitoramento.Nome;
                    monitoramentoDb.Tipo = monitoramento.Tipo;
                    monitoramentoDb.Endereco = monitoramento.Endereco;
                    monitoramentoDb.Ativo = monitoramento.Ativo;

                    await _monitoramentoAplicacao.Atualizar(monitoramentoDb);
                    await _monitoramentoAplicacao.Salvar();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _monitoramentoAplicacao.Existe(monitoramento.Id))
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

            await AdicionarViewModelDosWebhook();
            return View(monitoramento);
        }

        // GET: Monitoramento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitoramento = await _monitoramentoAplicacao.ObterPorId(id.Value, consultarComWebhook: false);
            if (monitoramento == null)
            {
                return NotFound();
            }

            return View(monitoramento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monitoramento = await _monitoramentoAplicacao.ObterPorId(id, consultarComWebhook: false);
            if (monitoramento != null)
            {
                var relacionamentos = await _monitoramentoWebhookAplicacao.ObterPorIdMonitoramento(monitoramento.Id, consultarComWebhook: false);

                if (relacionamentos != null && relacionamentos.Count != 0)
                {
                    await _monitoramentoWebhookAplicacao.RemoverEmLote(relacionamentos);
                }

                await _monitoramentoAplicacao.Remover(monitoramento);
            }

            await _monitoramentoAplicacao.Salvar();

            return RedirectToAction(nameof(Index));
        }

        private async Task AdicionarViewModelDosWebhook()
        {
            ViewBag.Webhooks = await _webhookAplicacao.ObterTodos(buscarSomenteAtivos: true);
        }

        private async Task AdicionarViewModelConfigSistema()
        {
            ViewBag.ConfiguracaoSistema = await _configSistemaAplicacao.ObterConfiguracaoSistema();
        }
    }
}
