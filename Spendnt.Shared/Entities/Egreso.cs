using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Spendnt.Shared.Entities
{
    public class Egresos
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [Required]
        [Display(Name = "Egreso")]
        [Range(0.01, double.MaxValue)]
        public decimal Egreso { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
