using GestorPresupuestosAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorPresupuestosAPI.Infraestructure.DataBases
{
    public class GestorPresupuestosAHM : DbContext
    {
        public GestorPresupuestosAHM(DbContextOptions<GestorPresupuestosAHM> options)
        : base(options)
        {
        }

        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Departamentos> departamentos { get; set; }
        public DbSet<Categorias> categorias {  get; set; } 
        public DbSet<Subcategorias> subcategorias { get; set;}
        public DbSet<Presupuesto> presupuesto { get; set; }
        public DbSet<Cuenta> cuenta {  get; set; } 
        public DbSet<PresupuestoCuenta> PresupuestoCuentas { get; set; }
        public DbSet<Tipo_presupuesto> Tipo_presupuesto { get; set; }
        public DbSet<ItemsEjecutados> ItemsEjecutados { get; set; }
        public DbSet<ItemsEjecucionParcial> ItemsEjecucionParcial { get; set; }
        public DbSet<Proveedores> proveedores { get; set; }

    }
}
