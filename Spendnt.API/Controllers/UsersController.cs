using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // PUT: api/users
        [HttpPut]
        public async Task<ActionResult> Put(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var affected = await _context.Users
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (affected == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

