using System.Data;

namespace Controladora
{

    public class ProductoController
    {
        private ProductoData data;

        public ProductoController()
        {
            data = new ProductoData();
        }

        public DataTable ListarProductos()
        {
            return data.ObtenerDataTable();
        }

        public bool CrearProducto(string nombre, decimal precio, int stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio");

            if (precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");

            if (stock < 0)
                throw new Exception("El stock no puede ser negativo");

            return data.Insertar(nombre, precio, stock);
        }

        public bool ActualizarProducto(int id, string nombre, decimal precio, int stock)
        {
            if (id <= 0)
                throw new Exception("ID inválido");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio");

            if (precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");

            if (stock < 0)
                throw new Exception("El stock no puede ser negativo");

            return data.Actualizar(id, nombre, precio, stock);
        }

        public bool EliminarProducto(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido");

            return data.Eliminar(id);
        }

        public DataTable BuscarProductos(string nombre)
        {
            return data.BuscarPorNombre(nombre);
        }
    }
}
