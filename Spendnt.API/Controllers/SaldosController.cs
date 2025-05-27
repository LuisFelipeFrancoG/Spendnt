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
            return Ok(saldo);
        }



    }
}
