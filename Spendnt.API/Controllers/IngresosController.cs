using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{


    [ApiController]
    [Route("/api/ingresos")]
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

        public async Task<ActionResult> Post(Ingresos ingresos)
        {

            _context.Ingresos.Add(ingresos);

            await _context.SaveChangesAsync();
            return Ok(ingresos); //200




        }


        //Actualizar o modificar datos

        [HttpPut]

        public async Task<ActionResult> Put(Ingresos ingresos)
        {

            _context.Ingresos.Update(ingresos);

            await _context.SaveChangesAsync();
            return Ok(ingresos);

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var filasafectadas = await _context.Ingresos

               .Where(x => x.Id == id)
               .ExecuteDeleteAsync();






            if (filasafectadas == 0)
            {



                return NotFound();//404
            }

            return NoContent();//204

        }


    }
}