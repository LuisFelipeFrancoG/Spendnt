
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

            await SeedCategoriasAsync();

            Saldo saldoPrincipal = await SeedSaldoPrincipalAsync();

            await SeedIngresosAsync(saldoPrincipal);

            await SeedEgresosAsync(saldoPrincipal);

        }

        private async Task SeedCategoriasAsync()
        {
            if (!await _context.Categorias.AnyAsync())
            {
                var categorias = new List<Categoria>
                {
                    new Categoria { Nombre = "Salario", Descripcion = "Ingresos por nómina o sueldo." },
                    new Categoria { Nombre = "Ventas", Descripcion = "Ingresos por ventas de productos o servicios." },
                    new Categoria { Nombre = "Alimentación", Descripcion = "Gastos en supermercado, restaurantes, etc." },
                    new Categoria { Nombre = "Transporte", Descripcion = "Gastos de combustible, transporte público, mantenimiento vehicular." },
                    new Categoria { Nombre = "Vivienda", Descripcion = "Alquiler, hipoteca, servicios básicos (agua, luz, gas)." },
                    new Categoria { Nombre = "Ocio", Descripcion = "Cine, conciertos, salidas, hobbies." },
                    new Categoria { Nombre = "Salud", Descripcion = "Medicamentos, consultas médicas, seguros." },
                    new Categoria { Nombre = "Educación", Descripcion = "Cursos, libros, matrículas." },
                    new Categoria { Nombre = "Otros Ingresos", Descripcion = "Ingresos no recurrentes o de otras fuentes." },
                    new Categoria { Nombre = "Otros Gastos", Descripcion = "Gastos varios no clasificados." }
                };
                _context.Categorias.AddRange(categorias);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<Saldo> SeedSaldoPrincipalAsync()
        {
            Saldo saldo = await _context.Saldo.FirstOrDefaultAsync();

            if (saldo == null)
            {
                saldo = new Saldo();
                _context.Saldo.Add(saldo);
                await _context.SaveChangesAsync();
            }
            return saldo;
        }

        private async Task SeedIngresosAsync(Saldo saldoPrincipal)
        {
            if (saldoPrincipal == null || await _context.Ingresos.AnyAsync())
            {
                return;
            }

            var categoriaSalario = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Salario");
            var categoriaVentas = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Ventas");

            if (categoriaSalario == null || categoriaVentas == null)
            {
                Console.WriteLine("WARN: No se pudieron encontrar categorías 'Salario' o 'Ventas' para sembrar ingresos.");
                return;
            }

            var ingresos = new List<Ingresos>
            {
                new Ingresos
                {
                    Ingreso = 2500.00M,
                    Fecha = DateTime.UtcNow.AddDays(-15),
                    Categoria = categoriaSalario,
                    Saldo = saldoPrincipal
                },
                new Ingresos
                {
                    Ingreso = 350.50M,
                    Fecha = DateTime.UtcNow.AddDays(-10),
                    Categoria = categoriaVentas,
                    Saldo = saldoPrincipal
                },
                new Ingresos
                {
                    Ingreso = 120.00M,
                    Fecha = DateTime.UtcNow.AddDays(-5),
                    Categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Otros Ingresos") ?? categoriaVentas, // Usa "Otros Ingresos" o "Ventas" como fallback
                    Saldo = saldoPrincipal
                }
            };

            _context.Ingresos.AddRange(ingresos);
            await _context.SaveChangesAsync();
        }

        private async Task SeedEgresosAsync(Saldo saldoPrincipal)
        {
            if (saldoPrincipal == null || await _context.Egresos.AnyAsync())
            {
                return;
            }

            var categoriaAlimentacion = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Alimentación");
            var categoriaTransporte = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Transporte");
            var categoriaVivienda = await _context.Categorias.FirstOrDefaultAsync(c => c.Nombre == "Vivienda");

            if (categoriaAlimentacion == null || categoriaTransporte == null || categoriaVivienda == null)
            {
                Console.WriteLine("WARN: No se pudieron encontrar categorías necesarias para sembrar egresos.");
                return;
            }

            var egresos = new List<Egresos>
            {
                new Egresos
                {
                    Egreso = 450.75M,
                    Fecha = DateTime.UtcNow.AddDays(-12),
                    Categoria = categoriaAlimentacion,
                    Saldo = saldoPrincipal
                },
                new Egresos
                {
                    Egreso = 80.00M,
                    Fecha = DateTime.UtcNow.AddDays(-8),
                    Categoria = categoriaTransporte,
                    Saldo = saldoPrincipal
                },
                new Egresos
                {
                    Egreso = 700.00M,
                    Fecha = DateTime.UtcNow.AddDays(-3),
                    Categoria = categoriaVivienda,
                    Saldo = saldoPrincipal
                }
            };

            _context.Egresos.AddRange(egresos);
            await _context.SaveChangesAsync();
        }

    }
}
