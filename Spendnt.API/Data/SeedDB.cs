using Spendnt.API.Helpers;
using Spendnt.Shared.Entities;
using Spendnt.Shared.Enums;

namespace Spendnt.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDB(DataContext context, IUserHelper userHelper) 
        { 
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync() 
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Luis", "Franco", "lff@gmail.com", "300445555", UserType.Admin);

        }

        private async Task<User> CheckUserAsync(string Nombre, string Apellido, string email, string Contraseña, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {

                    Nombre = Nombre,
                    Apellido = Apellido,
                    Email = email,
                    Contraseña = Contraseña,
                    


                    UserName = email,


                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

    }
}
