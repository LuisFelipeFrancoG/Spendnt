using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendet.Shared.Entities;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordatoriosGastoController : ControllerBase
    {
        private readonly DataContext _context;

        public RecordatoriosGastoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.RecordatoriosGasto.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var recordatorio = await _context.RecordatoriosGasto.FirstOrDefaultAsync(r => r.Id == id);
            if (recordatorio == null) return NotFound();
            return Ok(recordatorio);
        }

        [HttpPost]
        public async Task<ActionResult> Post(RecordatorioGasto recordatorio)
        {
            _context.Add(recordatorio);
            await _context.SaveChangesAsync();
            return Ok(recordatorio);
        }

        [HttpPut]
        public async Task<ActionResult> Put(RecordatorioGasto recordatorio)
        {
            _context.Update(recordatorio);
            await _context.SaveChangesAsync();
            return Ok(recordatorio);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var recordatorio = await _context.RecordatoriosGasto.FirstOrDefaultAsync(r => r.Id == id);
            if (recordatorio == null) return NotFound();

            _context.Remove(recordatorio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
