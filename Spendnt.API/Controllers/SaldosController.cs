using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

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



        // Get para obtoner una lista de resultados
        // Select * from table

        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.Saldo.ToListAsync()); //200

        }



        //Get por parámetro
        //Select* from table Where Id=...

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {

            var owner = await _context.Saldo.FirstOrDefaultAsync(x => x.Id == id);

            if (owner == null)
            {



                return NotFound();//404
            }

            return Ok(owner); //200

        }


        //Insertar datos o crear registros
        [HttpPost]

        public async Task<ActionResult> Post(Saldo saldo)
        {

            _context.Saldo.Add(saldo);

            await _context.SaveChangesAsync();
            return Ok(saldo); //200




        }


        //Actualizar o modificar datos

        [HttpPut]

        public async Task<ActionResult> Put(Saldo saldo)
        {

            _context.Saldo.Update(saldo);

            await _context.SaveChangesAsync();
            return Ok(saldo);

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var filasafectadas = await _context.Saldo

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
