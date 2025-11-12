using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCuentasCorrientes.Modelo
{
    public class CuentaCorriente
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaApertura { get; set; }

        // Navegación
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Movimiento> Movimientos { get; set; }

        // Propiedad calculada - NO se guarda en BD
        public decimal SaldoActual
        {
            get
            {
                if (Movimientos == null || !Movimientos.Any())
                    return 0;

                decimal creditos = Movimientos.Where(m => m.Tipo == TipoMovimiento.Credito).Sum(m => m.Monto);
                decimal debitos = Movimientos.Where(m => m.Tipo == TipoMovimiento.Debito).Sum(m => m.Monto);

                return creditos - debitos;
            }
        }
    }
}