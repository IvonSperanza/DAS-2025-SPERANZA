using System.Data;

namespace Vista
{
    public partial class FormProductos : Form
    {
        private ProductoController controller;
        private int idSeleccionado = 0;

        public FormProductos()
        {
            InitializeComponent();
            controller = new ProductoController();
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
                DataTable dt = controller.ListarProductos();
                dgvProductos.DataSource = dt;

                // Configurar columnas
                if (dgvProductos.Columns.Count > 0)
                {
                    dgvProductos.Columns["Id"].Width = 50;
                    dgvProductos.Columns["Nombre"].Width = 200;
                    dgvProductos.Columns["Precio"].Width = 100;
                    dgvProductos.Columns["Stock"].Width = 100;

                    dgvProductos.Columns["Precio"].DefaultCellStyle.Format = "C2";
                }

                lblTotal.Text = $"Total de productos: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                decimal precio = decimal.Parse(txtPrecio.Text);
                int stock = int.Parse(txtStock.Text);

                bool resultado;
                string mensaje;

                if (idSeleccionado == 0) // Nuevo producto
                {
                    resultado = controller.CrearProducto(nombre, precio, stock);
                    mensaje = "Producto creado exitosamente";
                }
                else // Actualizar producto
                {
                    resultado = controller.ActualizarProducto(idSeleccionado, nombre, precio, stock);
                    mensaje = "Producto actualizado exitosamente";
                }

                if (resultado)
                {
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarProductos();
                    LimpiarCampos();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Verifique que los valores numéricos sean correctos", "Error de formato",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro que desea eliminar el producto '{txtNombre.Text}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    if (controller.EliminarProducto(idSeleccionado))
                    {
                        MessageBox.Show("Producto eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarProductos();
                        LimpiarCampos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();

                if (string.IsNullOrEmpty(busqueda))
                {
                    CargarProductos();
                }
                else
                {
                    DataTable dt = controller.BuscarProductos(busqueda);
                    dgvProductos.DataSource = dt;
                    lblTotal.Text = $"Productos encontrados: {dt.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarProductos();
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
                btnGuardar.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
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
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permite números, punto decimal y backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Solo permite un punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permite números y backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
