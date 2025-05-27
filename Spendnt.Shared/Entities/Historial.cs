// Spendnt.Shared/Entities/Historial.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spendnt.Shared.Entities
{
    public class Historial
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }

        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria Categoria { get; set; }
    }
}