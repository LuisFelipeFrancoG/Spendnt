using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendet.Shared.Entities;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialesController : ControllerBase
    {
        private readonly DataContext _context;

        public HistorialesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Historiales
                .Include(h => h.Categoria)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var historial = await _context.Historiales
                .Include(h => h.Categoria)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (historial == null) return NotFound();
            return Ok(historial);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Historial historial)
        {
            _context.Add(historial);
            await _context.SaveChangesAsync();
            return Ok(historial);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Historial historial)
        {
            _context.Update(historial);
            await _context.SaveChangesAsync();
            return Ok(historial);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var historial = await _context.Historiales.FirstOrDefaultAsync(h => h.Id == id);
            if (historial == null) return NotFound();

            _context.Remove(historial);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}