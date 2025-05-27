// En Spendnt.Shared.Entities.Ingresos.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // NECESARIO

namespace Spendnt.Shared.Entities
{
    public class Ingresos
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ingreso")]
        [Range(0.01, double.MaxValue)]
        [JsonPropertyName("monto")] // Coincide con IngresoResponseDto.Monto
        public decimal Ingreso { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [JsonPropertyName("fecha")] // Coincide con IngresoResponseDto.Fecha
        public DateTime Fecha { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        [JsonPropertyName("categoriaId")] // Coincide con IngresoResponseDto.CategoriaId
        public int CategoriaId { get; set; }

        [JsonIgnore] // Para evitar ciclos y porque no lo envías/recibes como objeto completo
        public Categoria Categoria { get; set; }


        // Esta propiedad es la que se mostrará en la tabla y se debe mapear desde la API
        [Display(Name = "Nombre de Categoría")]
        [JsonPropertyName("categoriaNombre")] // Coincide con IngresoResponseDto.CategoriaNombre
        public string NombreCategoria { get; set; } = string.Empty;


        [Required]
        [JsonPropertyName("saldoId")] // Coincide con IngresoResponseDto.SaldoId
        public int SaldoId { get; set; }

        [JsonIgnore]
        public Saldo Saldo { get; set; }
    }
}