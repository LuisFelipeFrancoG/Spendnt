using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{

    [ApiController]
    [Route("/api/Ingresos")]
    public class IngresosController : ControllerBase
    {

        private readonly DataContext _context;

        public IngresosController(DataContext context)
        {

            _context = context;

        }



        // Get para obtoner una lista de resultados
        // Select * from table

        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.Ingresos.ToListAsync()); //200

        }



        //Get por parámetro
        //Select* from table Where Id=...

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {

            var ingresos = await _context.Ingresos.FirstOrDefaultAsync(x => x.Id == id);

            if (ingresos == null)
            {



                return NotFound();//404
            }

            return Ok(ingresos); //200

        }


        //Insertar datos o crear registros
        [HttpPost]
        public async Task<ActionResult<Ingresos>> Post(Ingresos ingresos) 
        {
            var saldoPrincipal = await _context.Saldo.FirstOrDefaultAsync();

            if (saldoPrincipal == null)
            {
                return BadRequest("Error crítico: No se encontró un saldo principal en la base de datos para asociar el ingreso.");
            }

            ingresos.SaldoId = saldoPrincipal.Id;

            _context.Ingresos.Add(ingresos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar el ingreso. Revise los datos e intente de nuevo.");
            }
            return Ok(ingresos); 
        }


        //Actualizar o modificar datos

        [HttpPut("{id:int}")] 
        public async Task<IActionResult> Put(int id, Ingresos ingresos)
        {
            if (id != ingresos.Id)
            {
                return BadRequest("El ID del ingreso en la ruta no coincide con el ID en el cuerpo de la solicitud.");
            }

            var saldoPrincipal = await _context.Saldo.FirstOrDefaultAsync();
            if (saldoPrincipal == null)
            {
                return BadRequest("Error crítico: No se encontró un saldo principal en la base de datos.");
            }
            if (ingresos.SaldoId == 0)
            {
                ingresos.SaldoId = saldoPrincipal.Id;
            }
            else if (ingresos.SaldoId != saldoPrincipal.Id)
            {
            }

            _context.Entry(ingresos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Ingresos.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al actualizar el ingreso.");
            }

            return NoContent(); 
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound();
            }

            _context.Ingresos.Remove(ingreso);
            await _context.SaveChangesAsync();

            return NoContent();
        }




    }
}
