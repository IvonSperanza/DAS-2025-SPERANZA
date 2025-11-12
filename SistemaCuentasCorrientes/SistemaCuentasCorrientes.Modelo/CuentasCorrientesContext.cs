using Microsoft.EntityFrameworkCore;

namespace SistemaCuentasCorrientes.Modelo
{
    public class CuentasCorrientesContext : DbContext
    {
        public CuentasCorrientesContext(DbContextOptions<CuentasCorrientesContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CuentaCorriente> CuentasCorrientes { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.CuentasCorrientes)
                .WithOne(cc => cc.Cliente)
                .HasForeignKey(cc => cc.ClienteId);

            modelBuilder.Entity<CuentaCorriente>()
                .HasMany(cc => cc.Movimientos)
                .WithOne(m => m.CuentaCorriente)
                .HasForeignKey(m => m.CuentaCorrienteId);

            // Configuración de decimales
            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Monto)
                .HasPrecision(18, 2);
        }
    }
}