using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Spendnt.Shared.Entities
{
    public class Usuario
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
        public string ContraseñaHash { get; set; }
    }
}
