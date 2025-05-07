using Spendnt.Shared.Entities;

namespace Spendnt.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context) 
        { 
            _context = context;
        }

        public async Task SeedAsync() 
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckUsuario();
        }

        public async Task CheckUsuario()
        {
            if (!_context.Usuarios.Any())
            {
                //_context.Usuarios.Add(new Usuario { Nombre = "Pancho" });
                //_context.Usuarios.Add(new Usuario { Nombre = "Juancho" });
            }
            await _context.SaveChangesAsync();
        }
    }
}
