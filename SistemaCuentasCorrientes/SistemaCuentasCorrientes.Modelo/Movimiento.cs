using System;

namespace SistemaCuentasCorrientes.Modelo
{
    public enum TipoMovimiento
    {
        Debito = 0,
        Credito = 1
    }

    public class Movimiento
    {
        public int Id { get; set; }
        public int CuentaCorrienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public TipoMovimiento Tipo { get; set; }

        // Navegación
        public virtual CuentaCorriente CuentaCorriente { get; set; }
    }
}