public static class DbHelper
{
    public static DbContextOptions<CuentasCorrientesContext> GetOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");
        return optionsBuilder.Options;
    }
}