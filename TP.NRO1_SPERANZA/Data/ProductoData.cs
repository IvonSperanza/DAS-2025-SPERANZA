namespace Data
{
    public class ProductoData
    {
        private readonly string connectionString;

        public ProductoData()
        {
            connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public bool Insertar(Producto producto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Productos (Nombre, Precio, Stock) 
                                   VALUES (@Nombre, @Precio, @Stock)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar: " + ex.Message);
            }
        }

        public List<Producto> ObtenerTodos()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Id, Nombre, Precio, Stock FROM Productos";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Stock = Convert.ToInt32(reader["Stock"])
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener datos: " + ex.Message);
            }

            return lista;
        }

      
        public Producto ObtenerPorId(int id)
        {
            Producto producto = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        producto = new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Stock = Convert.ToInt32(reader["Stock"])
                        };
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener producto: " + ex.Message);
            }

            return producto;
        }

        public bool Actualizar(Producto producto)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Productos 
                                   SET Nombre = @Nombre, 
                                       Precio = @Precio, 
                                       Stock = @Stock 
                                   WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", producto.Id);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Productos WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar: " + ex.Message);
            }
        }
    }
}
