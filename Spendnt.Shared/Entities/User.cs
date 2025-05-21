using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Spendnt.Shared.Enums;

namespace Spendnt.Shared.Entities
{
    public class User:IdentityUser
    {
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
        public UserType UserType { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }

        public ICollection<Ingresos> Ingresos { get; set; }
        public ICollection<Egresos> Egresos { get; set; }
    }
}
