using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class BDdata : DbContext
    {
        public BDdata(DbContextOptions<BDdata> options) : base(options) { }

        public DbSet<Empleados> Empleados { get; set; }
        public DbSet<Sucursal> sucursales { get; set; }
        public DbSet<Puesto> puestos { get; set; }
        public DbSet<EmpleadosTDO> GetEmpleadosbyID { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmpleadosTDO>().HasNoKey();
        }
    }
}
