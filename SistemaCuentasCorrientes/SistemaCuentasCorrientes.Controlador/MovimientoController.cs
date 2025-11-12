using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCuentasCorrientes.Modelo;
using Microsoft.EntityFrameworkCore;

namespace SistemaCuentasCorrientes.Controlador
{
    public class MovimientoController
    {
        // Registrar un débito (cargo)
        public bool RegistrarDebito(int cuentaId, string descripcion, decimal monto)
        {
            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser un valor positivo");
            }

            return RegistrarMovimiento(cuentaId, descripcion, monto, TipoMovimiento.Debito);
        }

        // Registrar un crédito (pago/abono)
        public bool RegistrarCredito(int cuentaId, string descripcion, decimal monto)
        {
            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser un valor positivo");
            }

            return RegistrarMovimiento(cuentaId, descripcion, monto, TipoMovimiento.Credito);
        }

        // Método privado para registrar movimientos
        private bool RegistrarMovimiento(int cuentaId, string descripcion, decimal monto, TipoMovimiento tipo)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var movimiento = new Movimiento
                    {
                        CuentaCorrienteId = cuentaId,
                        Fecha = DateTime.Now,
                        Descripcion = descripcion,
                        Monto = monto,
                        Tipo = tipo
                    };

                    context.Movimientos.Add(movimiento);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Obtener historial completo de movimientos de una cuenta
        public List<Movimiento> ObtenerHistorialMovimientos(int cuentaId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                return context.Movimientos
                    .Where(m => m.CuentaCorrienteId == cuentaId)
                    .OrderByDescending(m => m.Fecha)
                    .ToList();
            }
        }

        // Obtener resumen de una cuenta
        public (decimal TotalDebitos, decimal TotalCreditos, decimal Saldo) ObtenerResumenCuenta(int cuentaId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                var movimientos = context.Movimientos
                    .Where(m => m.CuentaCorrienteId == cuentaId)
                    .ToList();

                decimal totalDebitos = movimientos.Where(m => m.Tipo == TipoMovimiento.Debito).Sum(m => m.Monto);
                decimal totalCreditos = movimientos.Where(m => m.Tipo == TipoMovimiento.Credito).Sum(m => m.Monto);
                decimal saldo = totalCreditos - totalDebitos;

                return (totalDebitos, totalCreditos, saldo);
            }
        }

        // Eliminar movimiento
        public bool EliminarMovimiento(int id)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var movimiento = context.Movimientos.Find(id);
                    if (movimiento == null) return false;

                    context.Movimientos.Remove(movimiento);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}