using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCuentasCorrientes.Modelo;
using Microsoft.EntityFrameworkCore;

namespace SistemaCuentasCorrientes.Controlador
{
    public class ClienteController
    {
        // Crear un nuevo cliente
        public bool CrearCliente(string nombre, string apellido, string dni, string telefono)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var cliente = new Cliente
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        DNI = dni,
                        Telefono = telefono
                    };

                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Obtener todos los clientes
        public List<Cliente> ObtenerTodosLosClientes()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                return context.Clientes.Include(c => c.CuentasCorrientes).ToList();
            }
        }

        // Buscar cliente por ID
        public Cliente ObtenerClientePorId(int id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

            using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
            {
                return context.Clientes.Include(c => c.CuentasCorrientes).FirstOrDefault(c => c.Id == id);
            }
        }

        // Actualizar cliente
        public bool ActualizarCliente(int id, string nombre, string apellido, string dni, string telefono)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var cliente = context.Clientes.Find(id);
                    if (cliente == null) return false;

                    cliente.Nombre = nombre;
                    cliente.Apellido = apellido;
                    cliente.DNI = dni;
                    cliente.Telefono = telefono;

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Eliminar cliente
        public bool EliminarCliente(int id)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<CuentasCorrientesContext>();
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CuentasCorrientesDB;Trusted_Connection=True;");

                using (var context = new CuentasCorrientesContext(optionsBuilder.Options))
                {
                    var cliente = context.Clientes.Find(id);
                    if (cliente == null) return false;

                    context.Clientes.Remove(cliente);
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

