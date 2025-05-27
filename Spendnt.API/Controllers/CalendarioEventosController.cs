// Spendnt.API/Controllers/CalendarioEventosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spendnt.Shared.DTOs;

namespace Spendnt.API.Controllers
{
   

    [ApiController]
    [Route("api/calendarioeventos")]
    public class CalendarioEventosController : ControllerBase
    {
        private readonly DataContext _context;

        public CalendarioEventosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarioEventoDto>>> GetEventos(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var eventos = new List<CalendarioEventoDto>();

            var ingresos = await _context.Ingresos
                .Where(i => i.Fecha >= fechaInicio && i.Fecha <= fechaFin)
                .Include(i => i.Categorias)
                .ToListAsync();
            eventos.AddRange(ingresos.Select(i => new CalendarioEventoDto
            {
                Id = $"ingreso-{i.Id}",
                Title = $"Ingreso: {i.Categorias?.FirstOrDefault()?.Nombre ?? "Sin categoría"} ({i.Ingreso:C})",
                Start = i.Fecha,
                Tipo = "ingreso",
                Monto = i.Ingreso,
                Color = "green"
            }));

            var egresos = await _context.Egresos
                .Where(e => e.Fecha >= fechaInicio && e.Fecha <= fechaFin)
                .Include(e => e.Categorias)
                .ToListAsync();
            eventos.AddRange(egresos.Select(e => new CalendarioEventoDto
            {
                Id = $"egreso-{e.Id}",
                Title = $"Egreso: {e.Categorias?.FirstOrDefault()?.Nombre ?? "Sin categoría"} ({e.Egreso:C})",
                Start = e.Fecha,
                Tipo = "egreso",
                Monto = e.Egreso,
                Color = "red"
            }));

            var recordatorios = await _context.RecordatoriosGasto
                .Where(r => r.FechaProgramada >= fechaInicio && r.FechaProgramada <= fechaFin)
                .ToListAsync();
            eventos.AddRange(recordatorios.Select(r => new CalendarioEventoDto
            {
                Id = $"recordatorio-{r.Id}",
                Title = $"Recordatorio: {r.Titulo} ({r.MontoEstimado:C})",
                Start = r.FechaProgramada,
                Tipo = "recordatorio",
                Monto = r.MontoEstimado,
                Color = "orange"
            }));

            var metas = await _context.MetasAhorro
                 .Where(m => m.FechaObjetivo.HasValue && m.FechaObjetivo.Value >= fechaInicio && m.FechaObjetivo.Value <= fechaFin)
                .ToListAsync();
            eventos.AddRange(metas.Select(m => new CalendarioEventoDto
            {
                Id = $"meta-{m.Id}",
                Title = $"Meta: {m.Nombre} (Objetivo: {m.MontoObjetivo:C})",
                Start = m.FechaObjetivo.Value,
                Tipo = "meta",
                Monto = m.MontoObjetivo,
                Color = "blue"
            }));

            return Ok(eventos.OrderBy(e => e.Start));
        }
    }
}