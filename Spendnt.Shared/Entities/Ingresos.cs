using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Required(ErrorMessage = "Por favor, seleccione una categoría.")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [JsonIgnore]
        [Display(Name = "Nombre de Categoría")]
        public List<Categoria> Categorias { get; set; }

        public int SaldoId { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }
    }
}