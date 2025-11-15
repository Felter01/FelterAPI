using FelterAPI.Data;
using FelterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FelterAPI.Controllers;

[ApiController]
[Route("api/ominiflow/facilities")]
[Authorize]
public class OmniFlowFacilitiesController : ControllerBase
{
    private readonly FelterContext _ctx;

    public OmniFlowFacilitiesController(FelterContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _ctx.OmniFlowFacilities.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var entity = await _ctx.OmniFlowFacilities.FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OmniFlowFacility model)
    {
        model.Id = Guid.NewGuid();
        _ctx.OmniFlowFacilities.Add(model);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] OmniFlowFacility model)
    {
        var existing = await _ctx.OmniFlowFacilities.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.Entry(existing).CurrentValues.SetValues(model);
        existing.Id = id;
        await _ctx.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _ctx.OmniFlowFacilities.FindAsync(id);
        if (existing == null) return NotFound();

        _ctx.OmniFlowFacilities.Remove(existing);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}
