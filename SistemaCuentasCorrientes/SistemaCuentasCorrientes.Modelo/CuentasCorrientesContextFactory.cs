using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaCuentasCorrientes.Modelo
{
    public class CuentasCorrientesContextFactory : IDesignTimeDbContextFactory<CuentasCorrientesContext>
    {
        public CuentasCorrientesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            return new CuentasCorrientesContext(optionsBuilder.Options);
        }
    }
}