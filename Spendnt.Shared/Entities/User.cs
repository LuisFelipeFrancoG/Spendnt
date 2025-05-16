using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Spendnt.Shared.Entities
{
    public class User:IdentityUser
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required]
        [MaxLength(50)]
        public string Apellido { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";

        [Display(Name = "Email")]
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required]
        [MaxLength(50)]
        public string Contraseña { get; set; }
    }
}
