using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EgresosController : ControllerBase
    {
        private readonly DataContext _context;

        public EgresosController(DataContext context)
        {
            _context = context;
        }
        public class EgresoCreateDto
        {
            [Required(ErrorMessage = "El monto del egreso es requerido.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El monto del egreso debe ser mayor que cero.")]
            public decimal Monto { get; set; }

            [Required(ErrorMessage = "La categoría es requerida.")]
            [MaxLength(100)]
            public string Categoria { get; set; }

            [Required(ErrorMessage = "La fecha es requerida.")]
            public DateTime Fecha { get; set; }
        }
        public class EgresoUpdateDto
        {
            [Required(ErrorMessage = "El monto del egreso es requerido.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El monto del egreso debe ser mayor que cero.")]
            public decimal Monto { get; set; }

            [Required(ErrorMessage = "La categoría es requerida.")]
            [MaxLength(100)]
            public string Categoria { get; set; }

            [Required(ErrorMessage = "La fecha es requerida.")]
            public DateTime Fecha { get; set; }
        }
        public class EgresoResponseDto
        {
            public int Id { get; set; }
            public decimal Monto { get; set; }
            public string CategoriaNombre { get; set; }
            public int CategoriaId { get; set; }
            public DateTime Fecha { get; set; }
            public int SaldoId { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EgresoResponseDto>>> GetEgresos()
        {
            var egresos = await _context.Egresos
                .Include(e => e.Categoria)
                .Select(e => new EgresoResponseDto
                {
                    Id = e.Id,
                    Monto = e.Egreso,
                    CategoriaNombre = e.Categoria.Nombre,
                    CategoriaId = e.CategoriaId,
                    Fecha = e.Fecha,
                    SaldoId = e.SaldoId
                })
                .ToListAsync();

            return Ok(egresos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EgresoResponseDto>> GetEgreso(int id)
        {
            var egreso = await _context.Egresos
                .Include(e => e.Categoria)
                .Where(e => e.Id == id)
                .Select(e => new EgresoResponseDto
                {
                    Id = e.Id,
                    Monto = e.Egreso,
                    CategoriaNombre = e.Categoria.Nombre,
                    CategoriaId = e.CategoriaId,
                    Fecha = e.Fecha,
                    SaldoId = e.SaldoId
                })
                .FirstOrDefaultAsync();

            if (egreso == null)
            {
                return NotFound($"Egreso con Id {id} no encontrado.");
            }

            return Ok(egreso);
        }

        [HttpPost]
        public async Task<ActionResult<EgresoResponseDto>> PostEgreso([FromBody] EgresoCreateDto egresoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Saldo saldoGlobal = await _context.Saldos.FirstOrDefaultAsync();
            if (saldoGlobal == null)
            {
                saldoGlobal = new Saldo();
                _context.Saldos.Add(saldoGlobal);
            }

            Categoria categoriaEntidad = await _context.Categorias
                                            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == egresoDto.Categoria.ToLower());
            if (categoriaEntidad == null)
            {
                categoriaEntidad = new Categoria { Nombre = egresoDto.Categoria };
                _context.Categorias.Add(categoriaEntidad);
            }

            var nuevoEgreso = new Egresos
            {
                Egreso = egresoDto.Monto,
                Categoria = categoriaEntidad,
                Fecha = egresoDto.Fecha,
                Saldo = saldoGlobal
            };

            _context.Egresos.Add(nuevoEgreso);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocurrió un error interno al guardar: {ex.InnerException?.Message ?? ex.Message}");
            }

            var responseDto = new EgresoResponseDto
            {
                Id = nuevoEgreso.Id,
                Monto = nuevoEgreso.Egreso,
                CategoriaNombre = nuevoEgreso.Categoria.Nombre,
                CategoriaId = nuevoEgreso.CategoriaId,
                Fecha = nuevoEgreso.Fecha,
                SaldoId = nuevoEgreso.SaldoId
            };

            return CreatedAtAction(nameof(GetEgreso), new { id = nuevoEgreso.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEgreso(int id, [FromBody] EgresoUpdateDto egresoUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var egresoExistente = await _context.Egresos
                                          .Include(e => e.Saldo)
                                          .FirstOrDefaultAsync(e => e.Id == id);

            if (egresoExistente == null)
            {
                return NotFound($"Egreso con Id {id} no encontrado para actualizar.");
            }

            Categoria categoriaEntidad = await _context.Categorias
                                            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == egresoUpdateDto.Categoria.ToLower());
            if (categoriaEntidad == null)
            {
                categoriaEntidad = new Categoria { Nombre = egresoUpdateDto.Categoria };
                _context.Categorias.Add(categoriaEntidad);
            }

            egresoExistente.Egreso = egresoUpdateDto.Monto;
            egresoExistente.Fecha = egresoUpdateDto.Fecha;
            egresoExistente.Categoria = categoriaEntidad;

            _context.Entry(egresoExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Egresos.Any(e => e.Id == id))
                {
                    return NotFound($"Egreso con Id {id} ya no existe (conflicto de concurrencia).");
                }
                else { throw; }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocurrió un error interno al actualizar: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEgreso(int id)
        {

            var egreso = await _context.Egresos.FindAsync(id);
            if (egreso == null)
            {
                return NotFound($"Egreso con Id {id} no encontrado para eliminar.");
            }

            _context.Egresos.Remove(egreso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocurrió un error interno al eliminar: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}