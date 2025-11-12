using System;
using System.Windows.Forms;
using SistemaCuentasCorrientes.Controlador;
using SistemaCuentasCorrientes.Modelo;

namespace SistemaCuentasCorrientes.Vista
{
    public partial class FormClientes : Form
    {
        private ClienteController controlador;
        private DataGridView dgvClientes;
        private TextBox txtNombre, txtApellido, txtDNI, txtTelefono;
        private Button btnGuardar, btnNuevo, btnEliminar, btnActualizar;
        private int clienteIdSeleccionado = 0;

        public FormClientes()
        {
            InitializeComponent();
            controlador = new ClienteController();
            ConfigurarFormulario();
            CargarClientes();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Gestión de Clientes";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel izquierdo - Formulario
            Panel panelForm = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                Padding = new Padding(10)
            };

            Label lblTitulo = new Label
            {
                Text = "DATOS DEL CLIENTE",
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };

            // Nombre
            Label lblNombre = new Label { Text = "Nombre:", Location = new System.Drawing.Point(20, 60), AutoSize = true };
            txtNombre = new TextBox { Location = new System.Drawing.Point(20, 85), Width = 300 };

            // Apellido
            Label lblApellido = new Label { Text = "Apellido:", Location = new System.Drawing.Point(20, 120), AutoSize = true };
            txtApellido = new TextBox { Location = new System.Drawing.Point(20, 145), Width = 300 };

            // DNI
            Label lblDNI = new Label { Text = "DNI:", Location = new System.Drawing.Point(20, 180), AutoSize = true };
            txtDNI = new TextBox { Location = new System.Drawing.Point(20, 205), Width = 300 };

            // Teléfono
            Label lblTelefono = new Label { Text = "Teléfono:", Location = new System.Drawing.Point(20, 240), AutoSize = true };
            txtTelefono = new TextBox { Location = new System.Drawing.Point(20, 265), Width = 300 };

            // Botones
            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new System.Drawing.Point(20, 310),
                Width = 140,
                Height = 35
            };
            btnGuardar.Click += BtnGuardar_Click;

            btnNuevo = new Button
            {
                Text = "Nuevo",
                Location = new System.Drawing.Point(180, 310),
                Width = 140,
                Height = 35
            };
            btnNuevo.Click += BtnNuevo_Click;

            btnActualizar = new Button
            {
                Text = "Actualizar",
                Location = new System.Drawing.Point(20, 355),
                Width = 140,
                Height = 35
            };
            btnActualizar.Click += BtnActualizar_Click;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new System.Drawing.Point(180, 355),
                Width = 140,
                Height = 35,
                BackColor = System.Drawing.Color.IndianRed
            };
            btnEliminar.Click += BtnEliminar_Click;

            // Agregar controles al panel
            panelForm.Controls.AddRange(new Control[] {
                lblTitulo, lblNombre, txtNombre, lblApellido, txtApellido,
                lblDNI, txtDNI, lblTelefono, txtTelefono,
                btnGuardar, btnNuevo, btnActualizar, btnEliminar
            });

            // Panel derecho - DataGridView
            Panel panelGrid = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            Label lblListado = new Label
            {
                Text = "LISTADO DE CLIENTES",
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };

            dgvClientes = new DataGridView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(500, 480),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvClientes.CellClick += DgvClientes_CellClick;

            panelGrid.Controls.Add(lblListado);
            panelGrid.Controls.Add(dgvClientes);

            // Agregar paneles al formulario
            this.Controls.Add(panelGrid);
            this.Controls.Add(panelForm);
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = controlador.ObtenerTodosLosClientes();
                dgvClientes.DataSource = null;
                dgvClientes.DataSource = clientes;

                // Ocultar columna de CuentasCorrientes si existe
                if (dgvClientes.Columns["CuentasCorrientes"] != null)
                {
                    dgvClientes.Columns["CuentasCorrientes"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    bool resultado = controlador.CrearCliente(
                        txtNombre.Text.Trim(),
                        txtApellido.Text.Trim(),
                        txtDNI.Text.Trim(),
                        txtTelefono.Text.Trim()
                    );

                    if (resultado)
                    {
                        MessageBox.Show("Cliente guardado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarClientes();
                    }
                    else
                    {
                        MessageBox.Show("Error al guardar el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (clienteIdSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un cliente para actualizar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidarCampos())
            {
                try
                {
                    bool resultado = controlador.ActualizarCliente(
                        clienteIdSeleccionado,
                        txtNombre.Text.Trim(),
                        txtApellido.Text.Trim(),
                        txtDNI.Text.Trim(),
                        txtTelefono.Text.Trim()
                    );

                    if (resultado)
                    {
                        MessageBox.Show("Cliente actualizado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarClientes();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (clienteIdSeleccionado == 0)
            {
                MessageBox.Show("Debe seleccionar un cliente para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show(
                "¿Está seguro que desea eliminar este cliente?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    bool eliminado = controlador.EliminarCliente(clienteIdSeleccionado);

                    if (eliminado)
                    {
                        MessageBox.Show("Cliente eliminado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarClientes();
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvClientes.Rows[e.RowIndex];
                clienteIdSeleccionado = Convert.ToInt32(row.Cells["Id"].Value);
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtApellido.Text = row.Cells["Apellido"].Value.ToString();
                txtDNI.Text = row.Cells["DNI"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("El DNI es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("El teléfono es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefono.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            clienteIdSeleccionado = 0;
            txtNombre.Clear();
            txtApellido.Clear();
            txtDNI.Clear();
            txtTelefono.Clear();
            txtNombre.Focus();
        }
    }
}