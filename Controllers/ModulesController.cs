using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FelterAPI.Data;
using FelterAPI.Models;

namespace FelterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly FelterContext _ctx;

        public ModulesController(FelterContext ctx)
        {
            _ctx = ctx;
        }

        // ==========================================
        // GET: Listar todos os módulos
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _ctx.GlobalModules
                .AsNoTracking()
                .OrderBy(m => m.DisplayName)
                .ToListAsync();

            return Ok(list);
        }

        // ==========================================
        // GET: Buscar módulo por KEY
        // ==========================================
        [HttpGet("{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var module = await _ctx.GlobalModules.FindAsync(key);

            if (module == null)
                return NotFound(new { message = "Módulo não encontrado." });

            return Ok(module);
        }

        // ==========================================
        // POST: Criar módulo
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Create(GlobalModule model)
        {
            if (await _ctx.GlobalModules.AnyAsync(x => x.Key == model.Key))
                return BadRequest(new { message = "Já existe um módulo com essa key." });

            model.CreatedAt = DateTime.UtcNow;

            _ctx.GlobalModules.Add(model);
            await _ctx.SaveChangesAsync();

            return Ok(model);
        }

        // ==========================================
        // PUT: Atualizar módulo
        // ==========================================
        [HttpPut("{key}")]
        public async Task<IActionResult> Update(string key, GlobalModule req)
        {
            var module = await _ctx.GlobalModules.FindAsync(key);

            if (module == null)
                return NotFound(new { message = "Módulo não encontrado." });

            module.DisplayName = req.DisplayName;
            module.Description = req.Description;
            module.Active = req.Active;
            module.UpdatedAt = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();

            return Ok(module);
        }

        // ==========================================
        // DELETE: Remover módulo
        // ==========================================
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            var module = await _ctx.GlobalModules.FindAsync(key);

            if (module == null)
                return NotFound(new { message = "Módulo não encontrado." });

            _ctx.GlobalModules.Remove(module);
            await _ctx.SaveChangesAsync();

            return Ok(new { message = "Módulo removido com sucesso." });
        }
    }
}
