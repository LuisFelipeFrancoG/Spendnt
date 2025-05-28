
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historial>>> GetHistoriales()
        {
            return await _context.Historiales.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Historial>> GetHistorial(int id)
        {
            var historial = await _context.Historiales.FindAsync(id);

            if (historial == null)
            {
                return NotFound();
            }

            return historial;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorial(int id)
        {
            var historial = await _context.Historiales.FindAsync(id);
            if (historial == null)
            {
                return NotFound();
            }

            _context.Historiales.Remove(historial);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}