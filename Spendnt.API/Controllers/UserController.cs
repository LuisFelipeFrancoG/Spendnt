using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("/api/usuarios")]
    public class UserController:ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {

            _context = context;

        }
    }
}
