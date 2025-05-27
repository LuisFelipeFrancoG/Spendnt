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

        public async Task<ActionResult> Post(Egresos egresos)
        {

            _context.Egresos.Add(egresos);

            await _context.SaveChangesAsync();
            return Ok(egresos); //200




        }


        //Actualizar o modificar datos

        [HttpPut]

        public async Task<ActionResult> Put(Egresos egresos)
        {

            _context.Egresos.Update(egresos);

            await _context.SaveChangesAsync();
            return Ok(egresos);

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var filasafectadas = await _context.Egresos

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
