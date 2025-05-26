
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldosController : ControllerBase
    {
        private readonly DataContext _context;

        public SaldosController(DataContext context)
        {
            _context = context;
        }

        public class SaldoResponseDto
        {
            public int Id { get; set; }
            public decimal TotalSaldoCalculado { get; set; }
            public decimal TotalIngresosCalculado { get; set; }
            public decimal TotalEgresosCalculado { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<SaldoResponseDto>> GetSaldoGlobal()
        {
            var saldo = await _context.Saldos
                                     .Include(s => s.Ingresos)
                                     .Include(s => s.Egresos)
                                     .FirstOrDefaultAsync();

            if (saldo == null)
            {
                return NotFound("No se encontró ningún saldo. Registre un ingreso o egreso para crear uno.");
            }

            var saldoDto = new SaldoResponseDto
            {
                Id = saldo.Id,
                TotalIngresosCalculado = saldo.TotalIngresos,
                TotalEgresosCalculado = saldo.TotalEgresos,
                TotalSaldoCalculado = saldo.TotalSaldo
            };

            return Ok(saldoDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SaldoResponseDto>> GetSaldoPorId(int id)
        {
            var saldo = await _context.Saldos
                                     .Include(s => s.Ingresos)
                                     .Include(s => s.Egresos)
                                     .FirstOrDefaultAsync(x => x.Id == id);

            if (saldo == null)
            {
                return NotFound($"Saldo con Id {id} no encontrado.");
            }

            var saldoDto = new SaldoResponseDto
            {
                Id = saldo.Id,
                TotalIngresosCalculado = saldo.TotalIngresos,
                TotalEgresosCalculado = saldo.TotalEgresos,
                TotalSaldoCalculado = saldo.TotalSaldo
            };

            return Ok(saldoDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaldo(int id)
        {
            bool tieneIngresos = await _context.Ingresos.AnyAsync(i => i.SaldoId == id);
            bool tieneEgresos = await _context.Egresos.AnyAsync(e => e.SaldoId == id);

            if (tieneIngresos || tieneEgresos)
            {
                return BadRequest($"No se puede eliminar el saldo con Id {id} porque tiene ingresos o egresos asociados. Elimine primero las transacciones.");
            }


            var saldo = await _context.Saldos.FindAsync(id);

            if (saldo == null)
            {
                return NotFound($"Saldo con Id {id} no encontrado para eliminar.");
            }

            _context.Saldos.Remove(saldo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar el saldo: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}
