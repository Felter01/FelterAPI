using FelterAPI.Data;
using FelterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Controllers;

[ApiController]
[Route("api/stockflow/employees")]
[Authorize]
public class StockFlowEmployeesController : ControllerBase
{
    private readonly FelterContext _ctx;

    public StockFlowEmployeesController(FelterContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _ctx.StockFlowEmployees.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entity = await _ctx.StockFlowEmployees.FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockFlowEmployee model)
    {
        model.Id = Guid.NewGuid();
        _ctx.StockFlowEmployees.Add(model);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StockFlowEmployee model)
    {
        var existing = await _ctx.StockFlowEmployees.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.Entry(existing).CurrentValues.SetValues(model);
        existing.Id = id;
        await _ctx.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _ctx.StockFlowEmployees.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.StockFlowEmployees.Remove(existing);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}
