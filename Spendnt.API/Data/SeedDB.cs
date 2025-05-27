using Microsoft.EntityFrameworkCore;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckCategoriasAsync();
            await CheckSaldosConIngresosEgresosAsync();
            await CheckHistorialAsync();
            await CheckRecordatoriosGastoAsync();
            await CheckMetasAhorroAsync();
        }

        private async Task CheckCategoriasAsync()
        {
            if (!_context.Categorias.Any())
            {
                var categorias = new List<Categoria>
                {
                    new Categoria { Nombre = "Alimentación", Descripcion = "Gastos en supermercado, restaurantes, etc." },
                    new Categoria { Nombre = "Transporte", Descripcion = "Gastos de combustible, transporte público, mantenimiento vehículo." },
                    new Categoria { Nombre = "Vivienda", Descripcion = "Alquiler, hipoteca, servicios (luz, agua, gas)." },
                    new Categoria { Nombre = "Ocio", Descripcion = "Cine, conciertos, salidas, hobbies." },
                    new Categoria { Nombre = "Salud", Descripcion = "Médico, farmacia, seguros de salud." },
                    new Categoria { Nombre = "Educación", Descripcion = "Cursos, libros, matrículas." },
                    new Categoria { Nombre = "Ropa y Accesorios", Descripcion = "Compra de vestimenta y complementos." },
                    new Categoria { Nombre = "Regalos y Donaciones", Descripcion = "Obsequios y contribuciones benéficas." },
                    new Categoria { Nombre = "Otros Gastos", Descripcion = "Gastos varios no clasificados." },
                    new Categoria { Nombre = "Sueldo", Descripcion = "Ingreso principal por trabajo." },
                    new Categoria { Nombre = "Ingresos Freelance", Descripcion = "Ingresos por trabajos independientes." },
                    new Categoria { Nombre = "Inversiones", Descripcion = "Rendimientos de inversiones." },
                    new Categoria { Nombre = "Regalos Recibidos", Descripcion = "Dinero recibido como obsequio." },
                    new Categoria { Nombre = "Otros Ingresos", Descripcion = "Ingresos varios no clasificados." }
                };
                _context.Categorias.AddRange(categorias);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckSaldosConIngresosEgresosAsync()
        {
            if (!_context.Saldo.Any())
            {
                var catSueldo = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Sueldo");
                var catFreelance = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Ingresos Freelance");
                var catOtrosIngresos = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Otros Ingresos");
                var catAlimentacion = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Alimentación");
                var catTransporte = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Transporte");
                var catOcio = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Ocio");

                if (catSueldo == null || catFreelance == null || catOtrosIngresos == null ||
                    catAlimentacion == null || catTransporte == null || catOcio == null)
                {
                    return;
                }

                var saldoPrincipal = new Saldo();

                saldoPrincipal.Ingresos.Add(new Ingresos
                {
                    Ingreso = 2200.75M,
                    Fecha = DateTime.UtcNow.AddDays(-25),
                    CategoriaId = catSueldo.Id,
                });
                saldoPrincipal.Ingresos.Add(new Ingresos
                {
                    Ingreso = 350.00M,
                    Fecha = DateTime.UtcNow.AddDays(-15),
                    CategoriaId = catFreelance.Id,
                });
                saldoPrincipal.Ingresos.Add(new Ingresos
                {
                    Ingreso = 80.50M,
                    Fecha = DateTime.UtcNow.AddDays(-5),
                    CategoriaId = catOtrosIngresos.Id,
                });

                saldoPrincipal.Egresos.Add(new Egresos
                {
                    Egreso = 450.20M,
                    Fecha = DateTime.UtcNow.AddDays(-20),
                    CategoriaId = catAlimentacion.Id,
                });
                saldoPrincipal.Egresos.Add(new Egresos
                {
                    Egreso = 75.00M,
                    Fecha = DateTime.UtcNow.AddDays(-10),
                    CategoriaId = catTransporte.Id,
                });
                saldoPrincipal.Egresos.Add(new Egresos
                {
                    Egreso = 120.99M,
                    Fecha = DateTime.UtcNow.AddDays(-3),
                    CategoriaId = catOcio.Id,
                });

                _context.Saldo.Add(saldoPrincipal);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckHistorialAsync()
        {
            if (!_context.Historiales.Any())
            {
                var catSueldo = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Sueldo");
                var catAlimentacion = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Alimentación");
                var catOcio = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Ocio");

                if (catSueldo != null && catAlimentacion != null && catOcio != null)
                {
                    var historiales = new List<Historial>
                    {
                        new Historial
                        {
                            Fecha = DateTime.UtcNow.AddMonths(-1).AddDays(5),
                            Monto = 2150.00M,
                            Tipo = "Ingreso",
                            Descripcion = "Nómina mes anterior",
                            CategoriaId = catSueldo.Id
                        },
                        new Historial
                        {
                            Fecha = DateTime.UtcNow.AddMonths(-1).AddDays(7),
                            Monto = 88.40M,
                            Tipo = "Egreso",
                            Descripcion = "Compra semanal supermercado",
                            CategoriaId = catAlimentacion.Id
                        },
                         new Historial
                        {
                            Fecha = DateTime.UtcNow.AddMonths(-1).AddDays(15),
                            Monto = 50.00M,
                            Tipo = "Egreso",
                            Descripcion = "Cena con amigos",
                            CategoriaId = catOcio.Id
                        }
                    };
                    _context.Historiales.AddRange(historiales);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task CheckRecordatoriosGastoAsync()
        {
            if (!_context.RecordatoriosGasto.Any())
            {
                var recordatorios = new List<RecordatorioGasto>
                {
                    new RecordatorioGasto
                    {
                        Titulo = "Pagar Alquiler Próximo Mes",
                        MontoEstimado = 750.00M,
                        FechaProgramada = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(1),
                        Notas = "Realizar transferencia antes del día 5."
                    },
                    new RecordatorioGasto
                    {
                        Titulo = "Renovación Seguro Coche",
                        MontoEstimado = 320.00M,
                        FechaProgramada = DateTime.UtcNow.AddMonths(2).AddDays(10),
                        Notas = "Comparar precios con otras aseguradoras."
                    },
                    new RecordatorioGasto
                    {
                        Titulo = "Suscripción Plataforma Streaming",
                        MontoEstimado = 14.99M,
                        FechaProgramada = DateTime.UtcNow.AddDays(20),
                        Notas = "Verificar si hay alguna promoción disponible."
                    }
                };
                _context.RecordatoriosGasto.AddRange(recordatorios);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMetasAhorroAsync()
        {
            if (!_context.MetasAhorro.Any())
            {
                var metas = new List<MetaAhorro>
                {
                    new MetaAhorro
                    {
                        Nombre = "Vacaciones Verano 2025",
                        Descripcion = "Viaje a la playa con la familia.",
                        MontoObjetivo = 1500.00M,
                        MontoActual = 350.00M,
                        FechaObjetivo = new DateTime(2025, 7, 1),
                        FechaCreacion = DateTime.UtcNow.AddMonths(-2),
                        EstaCompletada = false
                    },
                    new MetaAhorro
                    {
                        Nombre = "Nuevo Portátil",
                        Descripcion = "Para trabajo y estudio.",
                        MontoObjetivo = 800.00M,
                        MontoActual = 120.50M,
                        FechaObjetivo = new DateTime(DateTime.UtcNow.Year + 1, 3, 15),
                        FechaCreacion = DateTime.UtcNow.AddDays(-40),
                        EstaCompletada = false
                    },
                    new MetaAhorro
                    {
                        Nombre = "Fondo de Emergencia",
                        Descripcion = "Para imprevistos.",
                        MontoObjetivo = 2000.00M,
                        MontoActual = 1000.00M,
                        FechaCreacion = DateTime.UtcNow.AddYears(-1),
                        EstaCompletada = false
                    }
                };
                _context.MetasAhorro.AddRange(metas);
                await _context.SaveChangesAsync();
            }
        }
    }
}