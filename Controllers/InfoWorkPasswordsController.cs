using FelterAPI.Data;
using FelterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Controllers;

[ApiController]
[Route("api/infowork/passwords")]
[Authorize]
public class InfoWorkPasswordsController : ControllerBase
{
    private readonly FelterContext _ctx;

    public InfoWorkPasswordsController(FelterContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _ctx.InfoWorkPasswords.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entity = await _ctx.InfoWorkPasswords.FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InfoWorkPassword model)
    {
        model.Id = Guid.NewGuid();
        _ctx.InfoWorkPasswords.Add(model);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] InfoWorkPassword model)
    {
        var existing = await _ctx.InfoWorkPasswords.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.Entry(existing).CurrentValues.SetValues(model);
        existing.Id = id;
        await _ctx.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _ctx.InfoWorkPasswords.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.InfoWorkPasswords.Remove(existing);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}
