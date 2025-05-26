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

        [Required(ErrorMessage = "El monto del egreso es requerido.")]
        [Display(Name = "Egreso")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto del egreso debe ser mayor que cero.")]
        public decimal Egreso { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "La fecha es requerida.")]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El saldo es obligatorio.")]
        public int SaldoId { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }
    }
}