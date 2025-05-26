using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Categorias.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Categoria categoria)
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria == null) return NotFound();

            _context.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}