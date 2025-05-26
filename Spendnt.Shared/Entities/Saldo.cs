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
    public class Saldo
    {
        public int Id { get; set; }

        public ICollection<Ingresos> Ingresos { get; set; } = new List<Ingresos>();
        public ICollection<Egresos> Egresos { get; set; } = new List<Egresos>();

        [NotMapped]
        public decimal TotalIngresos => Ingresos?.Sum(i => i.Ingreso) ?? 0;

        [NotMapped]
        public decimal TotalEgresos => Egresos?.Sum(e => e.Egreso) ?? 0;

        [NotMapped]
        public decimal TotalSaldo => TotalIngresos - TotalEgresos;
    }
}