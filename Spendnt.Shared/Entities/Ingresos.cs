using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Spendnt.Shared.Entities
{
    public class Ingresos
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ingreso")]
        [Range(0.01, double.MaxValue)]
        public decimal Ingreso { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria Categoria { get; set; }

        [Required]
        public int SaldoId { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }



    }
}