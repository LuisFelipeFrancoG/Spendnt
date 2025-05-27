using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{

    [ApiController]
    [Route("/api/Egresos")]
    public class EgresosController : ControllerBase
    {

        private readonly DataContext _context;

        public EgresosController(DataContext context)
        {

            _context = context;

        }



        // Get para obtoner una lista de resultados
        // Select * from table

        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.Egresos.ToListAsync()); //200

        }



        //Get por parámetro
        //Select* from table Where Id=...

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {

            var egresos = await _context.Egresos.FirstOrDefaultAsync(x => x.Id == id);

            if (egresos == null)
            {



                return NotFound();//404
            }

            return Ok(egresos); //200

        }


        //Insertar datos o crear registros
        [HttpPost]
        public async Task<ActionResult<Egresos>> Post(Egresos egresos)
        {
            var saldoPrincipal = await _context.Saldo.FirstOrDefaultAsync();
            if (saldoPrincipal == null)
            {
                return BadRequest("Error crítico: No se encontró un saldo principal en la base de datos para asociar el egreso.");
            }
            egresos.SaldoId = saldoPrincipal.Id;

            _context.Egresos.Add(egresos);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al guardar el egreso.");
            }
            return Ok(egresos);
        }


        //Actualizar o modificar datos

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Egresos egresos)
        {
            if (id != egresos.Id)
            {
                return BadRequest();
            }

            var saldoPrincipal = await _context.Saldo.FirstOrDefaultAsync();
            if (saldoPrincipal == null)
            {
                return BadRequest("Error crítico: No se encontró un saldo principal en la base de datos.");
            }
            if (egresos.SaldoId == 0)
            {
                egresos.SaldoId = saldoPrincipal.Id;
            }

            _context.Entry(egresos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Egresos.AnyAsync(e => e.Id == id))
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al actualizar el egreso.");
            }
            return NoContent();
        }



        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var filasafectadas = await _context.Egresos
               .Where(x => x.Id == id)
               .ExecuteDeleteAsync();

            if (filasafectadas == 0)
            {
                return NotFound();
            }
            return NoContent();
        }




    }
}
