// Spendnt.API/Controllers/MetasAhorroController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetasAhorroController : ControllerBase
    {
        private readonly DataContext _context;

        public MetasAhorroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetaAhorro>>> GetMetasAhorro()
        {
            return await _context.MetasAhorro.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MetaAhorro>> GetMetaAhorro(int id)
        {
            var metaAhorro = await _context.MetasAhorro.FindAsync(id);

            if (metaAhorro == null)
            {
                return NotFound();
            }
            return metaAhorro;
        }

        [HttpPost]
        public async Task<ActionResult<MetaAhorro>> PostMetaAhorro(MetaAhorro metaAhorro)
        {
            metaAhorro.FechaCreacion = DateTime.UtcNow;
            _context.MetasAhorro.Add(metaAhorro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMetaAhorro), new { id = metaAhorro.Id }, metaAhorro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetaAhorro(int id, MetaAhorro metaAhorro)
        {
            if (id != metaAhorro.Id)
            {
                return BadRequest();
            }

            _context.Entry(metaAhorro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetaAhorroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetaAhorro(int id)
        {
            var metaAhorro = await _context.MetasAhorro.FindAsync(id);
            if (metaAhorro == null)
            {
                return NotFound();
            }

            _context.MetasAhorro.Remove(metaAhorro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/contribuir")]
        public async Task<IActionResult> ContribuirMetaAhorro(int id, [FromBody] decimal monto)
        {
            var metaAhorro = await _context.MetasAhorro.FindAsync(id);

            if (metaAhorro == null)
            {
                return NotFound("Meta de ahorro no encontrada.");
            }

            metaAhorro.MontoActual += monto;
            if (metaAhorro.MontoActual < 0) metaAhorro.MontoActual = 0;
            if (metaAhorro.MontoActual >= metaAhorro.MontoObjetivo)
            {
                metaAhorro.EstaCompletada = true;
            }
            else
            {
                metaAhorro.EstaCompletada = false;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Hubo un problema de concurrencia al actualizar la meta.");
            }
            return Ok(metaAhorro);
        }

        private bool MetaAhorroExists(int id)
        {
            return _context.MetasAhorro.Any(e => e.Id == id);
        }
    }
}