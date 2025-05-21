using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("/api/saldo")]
    public class SaldosController : ControllerBase
    {
        private readonly DataContext _context;
        public SaldosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {


            return Ok(await _context.Saldos.ToListAsync()); //200

        }



        //Get por parámetro
        //Select* from table Where Id=...

        [HttpGet("{userId}")]
        public async Task<ActionResult<Saldo>> Get(string userId)
        {

            var saldos = await _context.Saldos.FirstOrDefaultAsync(x => x.UserId == userId);

            if (saldos == null)
            {



                return NotFound();//404
            }

            return Ok(saldos); //200

        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> Delete(string userId)
        {

            var filasafectadas = await _context.Saldos

               .Where(x => x.UserId == userId)
               .ExecuteDeleteAsync();






            if (filasafectadas == 0)
            {



                return NotFound();//404
            }

            return NoContent();//204

        }

    }
}
