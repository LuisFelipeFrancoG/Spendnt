// Spendnt.API/Controllers/SaldoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;
using System.Linq; // Asegúrate de tener este using para .Sum()
using System.Threading.Tasks;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("/api/Saldo")]
    public class SaldoController : ControllerBase
    {
        private readonly DataContext _context;

        public SaldoController(DataContext context)
        {
            _context = context;
        }

        private void CalcularYAsignarTotales(Saldo saldo)
        {
            if (saldo != null)
            {
                // Asegúrate de que las colecciones estén cargadas si no lo están ya
                // Esto es redundante si siempre usas .Include() antes de llamar a este método,
                // pero es una salvaguarda o útil si se llama desde otros contextos.
                // Sin embargo, para evitar múltiples viajes a la BD, es mejor asegurar que
                // el llamador ya hizo los Includes.

                saldo.TotalIngresos = saldo.Ingresos?.Sum(i => i.Ingreso) ?? 0;
                saldo.TotalEgresos = saldo.Egresos?.Sum(e => e.Egreso) ?? 0;
                saldo.TotalSaldoCalculado = saldo.TotalIngresos - saldo.TotalEgresos;
                // La propiedad TotalSaldo (con su lógica de _totalSaldoManual) se evaluará
                // correctamente en el momento de la serialización si _totalSaldoManual
                // tiene un valor, o usará el TotalSaldoCalculado que acabamos de asignar.
            }
        }

        [HttpGet("actual")]
        public async Task<ActionResult<Saldo>> GetCurrentSaldo()
        {
            var saldo = await _context.Saldo
                                .Include(s => s.Ingresos)
                                .Include(s => s.Egresos)
                                .FirstOrDefaultAsync();

            if (saldo == null)
            {
                return NotFound("No se encontró el registro de saldo principal.");
            }

            CalcularYAsignarTotales(saldo); // CALCULAR Y ASIGNAR ANTES DE DEVOLVER
            return Ok(saldo);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Saldo>> GetSaldoById(int id)
        {
            var saldo = await _context.Saldo
                                .Include(s => s.Ingresos)
                                .Include(s => s.Egresos)
                                .FirstOrDefaultAsync(x => x.Id == id);

            if (saldo == null)
            {
                return NotFound();
            }

            CalcularYAsignarTotales(saldo); // CALCULAR Y ASIGNAR ANTES DE DEVOLVER
            return Ok(saldo);
        }

        // ... (Tus métodos PUT y POST si los tienes) ...
        // Si tienes un método PUT, también deberías llamar a CalcularYAsignarTotales
        // en el objeto 'saldoExistente' antes de devolverlo, si es que lo devuelves.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutSaldo(int id, Saldo saldoActualizado)
        {
            if (id != saldoActualizado.Id)
            {
                return BadRequest("El ID del saldo en la ruta no coincide con el del cuerpo.");
            }

            var saldoExistente = await _context.Saldo
                                        .Include(s => s.Ingresos) // Incluir para recalcular
                                        .Include(s => s.Egresos)  // Incluir para recalcular
                                        .FirstOrDefaultAsync(s => s.Id == id);

            if (saldoExistente == null)
            {
                return NotFound("El saldo a actualizar no fue encontrado.");
            }

            saldoExistente.TotalSaldo = saldoActualizado.TotalSaldo; // Esto actualiza _totalSaldoManual

            // Recalcular después de cualquier cambio que pueda afectar los totales
            // (aunque aquí solo estamos cambiando el saldo manual, es buena práctica si
            // se pudieran modificar ingresos/egresos a través de este endpoint indirectamente)
            CalcularYAsignarTotales(saldoExistente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Saldo.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Devuelve el objeto actualizado con los cálculos correctos
            return Ok(saldoExistente);
            // o return NoContent(); si no necesitas devolver el objeto
        }
    }
}