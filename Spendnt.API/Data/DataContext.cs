using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spendnt.Shared.Entities;


namespace Spendnt.API.Data
{
    public class DataContext : IdentityDbContext <User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Saldo> Saldos { get; set; }

        public DbSet<Ingresos> Ingresos { get; set; }

        public DbSet<Egresos> Egresos { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
