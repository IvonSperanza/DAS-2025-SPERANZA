using System.Collections.Generic;

namespace SistemaCuentasCorrientes.Modelo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }

        // Relación: Un cliente tiene muchas cuentas
        public virtual ICollection<CuentaCorriente> CuentasCorrientes { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}";

    }
}