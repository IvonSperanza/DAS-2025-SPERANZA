using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCuentasCorrientes.Modelo;
using Microsoft.EntityFrameworkCore;

namespace SistemaCuentasCorrientes.Controlador
{
    public class CuentaCorrienteController
    {
        // Crear una nueva cuenta corriente
        public bool CrearCuentaCorriente(int clienteId)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var cuenta = new CuentaCorriente
                    {
                        ClienteId = clienteId,
                        FechaApertura = DateTime.Now,
                        Movimientos = new List<Movimiento>()
                    };

                    context.CuentasCorrientes.Add(cuenta);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Obtener todas las cuentas con sus saldos
        public List<CuentaCorriente> ObtenerTodasLasCuentas()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                return context.CuentasCorrientes
                    .Include(cc => cc.Cliente)
                    .Include(cc => cc.Movimientos)
                    .ToList();
            }
        }

        // Obtener cuenta por ID
        public CuentaCorriente ObtenerCuentaPorId(int id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                return context.CuentasCorrientes
                    .Include(cc => cc.Cliente)
                    .Include(cc => cc.Movimientos)
                    .FirstOrDefault(cc => cc.Id == id);
            }
        }

        // Obtener cuentas de un cliente específico
        public List<CuentaCorriente> ObtenerCuentasPorCliente(int clienteId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                return context.CuentasCorrientes
                    .Include(cc => cc.Movimientos)
                    .Where(cc => cc.ClienteId == clienteId)
                    .ToList();
            }
        }

        // Consultar saldo de una cuenta
        public decimal ObtenerSaldo(int cuentaId)
        {
            var cuenta = ObtenerCuentaPorId(cuentaId);
            return cuenta?.SaldoActual ?? 0;
        }

        // Eliminar cuenta corriente
        public bool EliminarCuenta(int id)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var cuenta = context.CuentasCorrientes.Find(id);
                    if (cuenta == null) return false;

                    context.CuentasCorrientes.Remove(cuenta);
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