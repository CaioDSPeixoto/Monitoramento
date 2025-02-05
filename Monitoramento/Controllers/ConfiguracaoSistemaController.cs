using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitorDeServicos.Dominio.Entidade;
using MonitorDeServicos.Dominio.Interface.Aplicacao;

namespace Monitoramento.Controllers
{
    public class ConfiguracaoSistemaController : Controller
    {
        private readonly IConfiguracaoSistemaAplicacao _configSistemaAplicacao;

        public ConfiguracaoSistemaController(IConfiguracaoSistemaAplicacao configSistemaAplicacao)
        {
            _configSistemaAplicacao = configSistemaAplicacao;
        }

        // GET: ConfiguracaoSistema
        public async Task<IActionResult> Index()
        {
            return View(await _configSistemaAplicacao.ObterConfiguracaoSistema());
        }

        // GET: ConfiguracaoSistema/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracaoSistema = await _configSistemaAplicacao.ObterPorId(id.Value);

            if (configuracaoSistema == null)
            {
                return NotFound();
            }

            return View(configuracaoSistema);
        }

        // POST: ConfiguracaoSistema/Edit/1
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConfiguracaoSistema configuracaoSistema)
        {
            if (id != configuracaoSistema.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _configSistemaAplicacao.Atualizar(configuracaoSistema);
                    await _configSistemaAplicacao.Salvar();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _configSistemaAplicacao.Existe(configuracaoSistema.Id))
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
            return View(configuracaoSistema);
        }
    }
}
