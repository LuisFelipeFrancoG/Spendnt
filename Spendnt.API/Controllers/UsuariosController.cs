using Microsoft.AspNetCore.Mvc;
using Spendnt.API.Data;
namespace Spendnt.API.Controllers
{
    [ApiController]
    public class UsuariosController:ControllerBase
    {
        private readonly DataContext _context;
    }
}
