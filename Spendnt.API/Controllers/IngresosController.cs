
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
    public class IngresosController : ControllerBase
    {
        private readonly DataContext _context;

        public IngresosController(DataContext context)
        {
            _context = context;
        }

        public class IngresoCreateDto
        {
            [Required(ErrorMessage = "El monto del ingreso es requerido.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El monto del ingreso debe ser mayor que cero.")]
            public decimal Monto { get; set; }

            [Required(ErrorMessage = "La categoría es requerida.")]
            [MaxLength(100)]
            public string Categoria { get; set; }

            [Required(ErrorMessage = "La fecha es requerida.")]
            public DateTime Fecha { get; set; }
        }

        public class IngresoUpdateDto
        {
            [Required(ErrorMessage = "El monto del ingreso es requerido.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El monto del ingreso debe ser mayor que cero.")]
            public decimal Monto { get; set; }

            [Required(ErrorMessage = "La categoría es requerida.")]
            [MaxLength(100)]
            public string Categoria { get; set; }

            [Required(ErrorMessage = "La fecha es requerida.")]
            public DateTime Fecha { get; set; }

        }

        public class IngresoResponseDto
        {
            public int Id { get; set; }
            public decimal Monto { get; set; }
            public string CategoriaNombre { get; set; }
            public int CategoriaId { get; set; }
            public DateTime Fecha { get; set; }
            public int SaldoId { get; set; }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngresoResponseDto>>> GetIngresos()
        {
            var ingresos = await _context.Ingresos
                .Include(i => i.Categoria)
                .Select(i => new IngresoResponseDto
                {
                    Id = i.Id,
                    Monto = i.Ingreso,
                    CategoriaNombre = i.Categoria.Nombre,
                    CategoriaId = i.CategoriaId,
                    Fecha = i.Fecha,
                    SaldoId = i.SaldoId
                })
                .ToListAsync();

            return Ok(ingresos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngresoResponseDto>> GetIngreso(int id)
        {
            var ingreso = await _context.Ingresos
                .Include(i => i.Categoria)
                .Where(i => i.Id == id)
                .Select(i => new IngresoResponseDto
                {
                    Id = i.Id,
                    Monto = i.Ingreso,
                    CategoriaNombre = i.Categoria.Nombre,
                    CategoriaId = i.CategoriaId,
                    Fecha = i.Fecha,
                    SaldoId = i.SaldoId
                })
                .FirstOrDefaultAsync();

            if (ingreso == null)
            {
                return NotFound($"Ingreso con Id {id} no encontrado.");
            }

            return Ok(ingreso);
        }

        [HttpPost]
        public async Task<ActionResult<IngresoResponseDto>> PostIngreso([FromBody] IngresoCreateDto ingresoDto)
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
                                            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == ingresoDto.Categoria.ToLower());
            if (categoriaEntidad == null)
            {
                categoriaEntidad = new Categoria { Nombre = ingresoDto.Categoria };
                _context.Categorias.Add(categoriaEntidad);
            }

            var nuevoIngreso = new Ingresos
            {
                Ingreso = ingresoDto.Monto,
                Categoria = categoriaEntidad,
                Fecha = ingresoDto.Fecha,
                Saldo = saldoGlobal
            };

            _context.Ingresos.Add(nuevoIngreso);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocurrió un error interno al guardar: {ex.InnerException?.Message ?? ex.Message}");
            }

            var responseDto = new IngresoResponseDto
            {
                Id = nuevoIngreso.Id,
                Monto = nuevoIngreso.Ingreso,
                CategoriaNombre = nuevoIngreso.Categoria.Nombre,
                CategoriaId = nuevoIngreso.CategoriaId,
                Fecha = nuevoIngreso.Fecha,
                SaldoId = nuevoIngreso.SaldoId
            };

            return CreatedAtAction(nameof(GetIngreso), new { id = nuevoIngreso.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngreso(int id, [FromBody] IngresoUpdateDto ingresoUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ingresoExistente = await _context.Ingresos
                                          .Include(i => i.Saldo)
                                          .FirstOrDefaultAsync(i => i.Id == id);

            if (ingresoExistente == null)
            {
                return NotFound($"Ingreso con Id {id} no encontrado para actualizar.");
            }

            Categoria categoriaEntidad = await _context.Categorias
                                            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == ingresoUpdateDto.Categoria.ToLower());
            if (categoriaEntidad == null)
            {
                categoriaEntidad = new Categoria { Nombre = ingresoUpdateDto.Categoria };
                _context.Categorias.Add(categoriaEntidad);
            }

            ingresoExistente.Ingreso = ingresoUpdateDto.Monto;
            ingresoExistente.Fecha = ingresoUpdateDto.Fecha;
            ingresoExistente.Categoria = categoriaEntidad;

            _context.Entry(ingresoExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Ingresos.Any(e => e.Id == id))
                {
                    return NotFound($"Ingreso con Id {id} ya no existe (conflicto de concurrencia).");
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocurrió un error interno al actualizar: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngreso(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound($"Ingreso con Id {id} no encontrado para eliminar.");
            }

            _context.Ingresos.Remove(ingreso);
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
