
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spendnt.Shared.Entities;


namespace Spendnt.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Saldo> Saldo { get; set; }

        public DbSet<Ingresos> Ingresos { get; set; }

        public DbSet<Egresos> Egresos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Historial> Historiales { get; set; }
        public DbSet<RecordatorioGasto> RecordatoriosGasto { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ingresos>()
                .Property(i => i.Ingreso)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Egresos>()
                .Property(e => e.Egreso)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Historial>()
                .Property(h => h.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RecordatorioGasto>()
                .Property(r => r.MontoEstimado)
                .HasPrecision(18, 2);

        }
    }
}
