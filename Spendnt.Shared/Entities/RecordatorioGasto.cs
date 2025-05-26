using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spendnt.Shared.Entities
{
    public class RecordatorioGasto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal MontoEstimado { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string? Notas { get; set; }
    }
}