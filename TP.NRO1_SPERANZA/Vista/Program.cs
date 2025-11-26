namespace Vista
{

    public partial class FormProductos : Form
    {
        private ProductoData data = new ProductoData();
        private int idSeleccionado = 0;

        public FormProductos()
        {
            InitializeComponent();
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            CargarProductos();
            LimpiarCampos();
        }

        private void CargarProductos()
        {
            try
            {
                dgvProductos.DataSource = data.ObtenerTodos();
                dgvProductos.Columns["Id"].Width = 50;
                dgvProductos.Columns["Nombre"].Width = 200;
                dgvProductos.Columns["Precio"].Width = 100;
                dgvProductos.Columns["Stock"].Width = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            try
            {
                Producto producto = new Producto
                {
                    Id = idSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text)
                };

                bool resultado;
                string mensaje;

                if (idSeleccionado == 0)
                    resultado = data.Insertar(producto);
                    mensaje = resultado ? "Producto creado exitosamente" : "No se pudo crear el producto";
                }
                else 
                    resultado = data.Actualizar(producto);
                    mensaje = resultado ? "Producto actualizado exitosamente" : "No se pudo actualizar el producto";
                }

                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show("¿Está seguro que desea eliminar este producto?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    if (data.Eliminar(idSeleccionado))
                    {
                        MessageBox.Show("Producto eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProductos();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el producto", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(row.Cells["Id"].Value);
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value.ToString();
                txtStock.Text = row.Cells["Stock"].Value.ToString();

                btnGuardar.Text = "Actualizar";
            }
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtNombre.Focus();
            btnGuardar.Text = "Guardar";
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("El precio es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text, out precio) || precio <= 0)
            {
                MessageBox.Show("Ingrese un precio válido mayor a 0", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("El stock es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            int stock;
            if (!int.TryParse(txtStock.Text, out stock) || stock < 0)
            {
                MessageBox.Show("Ingrese un stock válido (número entero >= 0)", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            return true;
        }
    }
}