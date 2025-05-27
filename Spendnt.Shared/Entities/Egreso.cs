using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class Egresos
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Egreso")]
        [Range(0.01, double.MaxValue)]
        public decimal Egreso { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [JsonIgnore]
        [Display(Name = "Nombre de Categoría")]
        public Categoria Categoria { get; set; }

        public int SaldoId { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }
    }
}