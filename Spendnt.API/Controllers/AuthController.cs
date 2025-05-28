// Spendnt.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spendnt.Shared.DTOs;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Spendnt.API.Data;

namespace Spendnt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userExistsByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userExistsByEmail != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Message = "El correo electrónico ya está registrado." });

           
            var userExistsByUsername = await _userManager.FindByNameAsync(model.UserName);
            if (userExistsByUsername != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Message = "El nombre de usuario ya existe." });


            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName, 
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error al crear el usuario.", Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, "User");

            var saldoPrincipal = new Saldo { UserId = user.Id, TotalSaldo = 0 };
            _context.Saldo.Add(saldoPrincipal);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Usuario registrado exitosamente." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

           
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName), 
                    new Claim(JwtRegisteredClaimNames.Email, user.Email), 
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userId = user.Id,
                    userName = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    roles = userRoles
                });
            }
            return Unauthorized(new { Message = "Correo electrónico o contraseña incorrectos." });
        }
    }
}