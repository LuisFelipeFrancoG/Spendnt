using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        [Required(ErrorMessage = "El nombre de la categoría es requerido para el formulario.")]
        [StringLength(100, ErrorMessage = "El nombre de la categoría no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre de Categoría (para formulario)")]
        public string NombreCategoria { get; set; } = string.Empty;

        [Required]
        public int SaldoId { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }



    }
}