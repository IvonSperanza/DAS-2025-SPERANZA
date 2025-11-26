using System.Data;

namespace Data
{

    public class ProductoData
    {
        private string connectionString = "Data Source=SERVIDOR;Initial Catalog=NombreDB;Integrated Security=True";
        private SqlConnection conexion;
        private SqlDataAdapter adapter;
        private DataSet dataSet;

        public ProductoData()
        {
            conexion = new SqlConnection(connectionString);
            dataSet = new DataSet();
        }

        public DataSet ObtenerTodos()
        {
            try
            {
                dataSet.Clear();
                string query = "SELECT Id, Nombre, Precio, Stock FROM Productos";
                adapter = new SqlDataAdapter(query, conexion);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                adapter.Fill(dataSet, "Productos");
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos: " + ex.Message);
            }
        }

        public bool Insertar(string nombre, decimal precio, int stock)
        {
            try
            {
                string query = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE 1=0";
                adapter = new SqlDataAdapter(query, conexion);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = builder.GetInsertCommand();

                DataTable tabla = new DataTable();
                adapter.Fill(tabla);

                DataRow nuevaFila = tabla.NewRow();
                nuevaFila["Nombre"] = nombre;
                nuevaFila["Precio"] = precio;
                nuevaFila["Stock"] = stock;

                tabla.Rows.Add(nuevaFila);

                int result = adapter.Update(tabla);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar: " + ex.Message);
            }
        }

        public bool Actualizar(int id, string nombre, decimal precio, int stock)
        {
            try
            {
                string query = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";
                adapter = new SqlDataAdapter(query, conexion);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.UpdateCommand = builder.GetUpdateCommand();

                DataTable tabla = new DataTable();
                adapter.Fill(tabla);

                if (tabla.Rows.Count > 0)
                {
                    DataRow fila = tabla.Rows[0];
                    fila["Nombre"] = nombre;
                    fila["Precio"] = precio;
                    fila["Stock"] = stock;

                    int result = adapter.Update(tabla);
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar: " + ex.Message);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                string query = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";
                adapter = new SqlDataAdapter(query, conexion);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", id);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.DeleteCommand = builder.GetDeleteCommand();

                DataTable tabla = new DataTable();
                adapter.Fill(tabla);

                if (tabla.Rows.Count > 0)
                {
                    tabla.Rows[0].Delete();
                    int result = adapter.Update(tabla);
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar: " + ex.Message);
            }
        }

        public DataTable BuscarPorNombre(string nombre)
        {
            try
            {
                DataSet ds = ObtenerTodos();
                DataTable tabla = ds.Tables["Productos"];

                DataView vista = new DataView(tabla);
                vista.RowFilter = $"Nombre LIKE '%{nombre}%'";

                return vista.ToTable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar: " + ex.Message);
            }
        }

        public DataTable ObtenerDataTable()
        {
            try
            {
                DataSet ds = ObtenerTodos();
                return ds.Tables["Productos"];
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener DataTable: " + ex.Message);
            }
        }
    }
}
