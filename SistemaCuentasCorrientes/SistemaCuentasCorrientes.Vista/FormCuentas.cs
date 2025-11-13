using System;
using System.Linq;
using System.Windows.Forms;
using SistemaCuentasCorrientes.Controlador;
using SistemaCuentasCorrientes.Modelo;

namespace SistemaCuentasCorrientes.Vista
{
    public partial class FormCuentas : Form
    {
        private CuentaCorrienteController controladorCuenta;
        private ClienteController controladorCliente;
        private MovimientoController controladorMovimiento;
        private DataGridView dgvCuentas, dgvMovimientos;
        private ComboBox cboClientes;
        private Button btnCrearCuenta, btnRegistrarDebito, btnRegistrarCredito, btnActualizar;
        private TextBox txtMonto, txtDescripcion;
        private Label lblSaldo, lblTotalDebitos, lblTotalCreditos;
        private int cuentaIdSeleccionada = 0;

        public FormCuentas()
        {
            InitializeComponent();
            controladorCuenta = new CuentaCorrienteController();
            controladorCliente = new ClienteController();
            controladorMovimiento = new MovimientoController();
            ConfigurarFormulario();
            CargarClientes();
            CargarCuentas();
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Gestión de Cuentas Corrientes";
            this.Size = new System.Drawing.Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel superior - Crear cuenta
            GroupBox gbCrearCuenta = new GroupBox
            {
                Text = "CREAR NUEVA CUENTA",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(400, 120)
            };

            Label lblCliente = new Label { Text = "Seleccionar Cliente:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            cboClientes = new ComboBox { Location = new System.Drawing.Point(20, 55), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };

            btnCrearCuenta = new Button { Text = "Crear Cuenta", Location = new System.Drawing.Point(20, 85), Width = 150, Height = 30 };
            btnCrearCuenta.Click += BtnCrearCuenta_Click;

            gbCrearCuenta.Controls.AddRange(new Control[] { lblCliente, cboClientes, btnCrearCuenta });

            // Panel listado de cuentas
            GroupBox gbCuentas = new GroupBox
            {
                Text = "LISTADO DE CUENTAS",
                Location = new System.Drawing.Point(20, 150),
                Size = new System.Drawing.Size(400, 480)
            };

            dgvCuentas = new DataGridView
            {
                Location = new System.Drawing.Point(10, 25),
                Size = new System.Drawing.Size(380, 440),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvCuentas.CellClick += DgvCuentas_CellClick;

            gbCuentas.Controls.Add(dgvCuentas);

            // Panel derecho - Movimientos
            GroupBox gbMovimientos = new GroupBox
            {
                Text = "MOVIMIENTOS Y RESUMEN",
                Location = new System.Drawing.Point(440, 20),
                Size = new System.Drawing.Size(620, 610)
            };

            // Resumen
            Panel panelResumen = new Panel
            {
                Location = new System.Drawing.Point(10, 25),
                Size = new System.Drawing.Size(600, 100),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblSaldo = new Label
            {
                Text = "SALDO ACTUAL: $0.00",
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };

            lblTotalCreditos = new Label { Text = "Total Créditos: $0.00", Location = new System.Drawing.Point(10, 45), AutoSize = true };
            lblTotalDebitos = new Label { Text = "Total Débitos: $0.00", Location = new System.Drawing.Point(10, 70), AutoSize = true };

            panelResumen.Controls.AddRange(new Control[] { lblSaldo, lblTotalCreditos, lblTotalDebitos });

            // Registrar movimiento
            GroupBox gbRegistrar = new GroupBox
            {
                Text = "Registrar Movimiento",
                Location = new System.Drawing.Point(10, 135),
                Size = new System.Drawing.Size(600, 120)
            };

            Label lblDescripcion = new Label { Text = "Descripción:", Location = new System.Drawing.Point(10, 25), AutoSize = true };
            txtDescripcion = new TextBox { Location = new System.Drawing.Point(10, 50), Width = 250 };

            Label lblMonto = new Label { Text = "Monto:", Location = new System.Drawing.Point(280, 25), AutoSize = true };
            txtMonto = new TextBox { Location = new System.Drawing.Point(280, 50), Width = 150 };

            btnRegistrarDebito = new Button
            {
                Text = "Registrar DÉBITO (Cargo)",
                Location = new System.Drawing.Point(10, 85),
                Width = 200,
                Height = 30,
                BackColor = System.Drawing.Color.LightCoral
            };
            btnRegistrarDebito.Click += BtnRegistrarDebito_Click;

            btnRegistrarCredito = new Button
            {
                Text = "Registrar CRÉDITO (Pago)",
                Location = new System.Drawing.Point(230, 85),
                Width = 200,
                Height = 30,
                BackColor = System.Drawing.Color.LightGreen
            };
            btnRegistrarCredito.Click += BtnRegistrarCredito_Click;

            btnActualizar = new Button
            {
                Text = "Actualizar",
                Location = new System.Drawing.Point(450, 85),
                Width = 120,
                Height = 30
            };
            btnActualizar.Click += BtnActualizar_Click;

            gbRegistrar.Controls.AddRange(new Control[] {
                lblDescripcion, txtDescripcion, lblMonto, txtMonto,
                btnRegistrarDebito, btnRegistrarCredito, btnActualizar
            });

            // Historial de movimientos
            Label lblHistorial = new Label
            {
                Text = "HISTORIAL DE MOVIMIENTOS",
                Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(10, 265),
                AutoSize = true
            };

            dgvMovimientos = new DataGridView
            {
                Location = new System.Drawing.Point(10, 290),
                Size = new System.Drawing.Size(600, 310),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            gbMovimientos.Controls.AddRange(new Control[] {
                panelResumen, gbRegistrar, lblHistorial, dgvMovimientos
            });

            // Agregar todo al formulario
            this.Controls.AddRange(new Control[] { gbCrearCuenta, gbCuentas, gbMovimientos });
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = controladorCliente.ObtenerTodosLosClientes();
                cboClientes.DataSource = clientes;
                cboClientes.DisplayMember = "NombreCompleto";
                cboClientes.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarCuentas()
        {
            try
            {
                var cuentas = controladorCuenta.ObtenerTodasLasCuentas();

                var cuentasDisplay = cuentas.Select(c => new
                {
                    c.Id,
                    Cliente = $"{c.Cliente.Nombre} {c.Cliente.Apellido}",
                    FechaApertura = c.FechaApertura.ToString("dd/MM/yyyy"),
                    Saldo = c.SaldoActual.ToString("C")
                }).ToList();

                dgvCuentas.DataSource = null;
                dgvCuentas.DataSource = cuentasDisplay;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar cuentas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCrearCuenta_Click(object sender, EventArgs e)
        {
            if (cboClientes.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un cliente", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int clienteId = (int)cboClientes.SelectedValue;
                bool resultado = controladorCuenta.CrearCuentaCorriente(clienteId);

                if (resultado)
                {
                    MessageBox.Show("Cuenta creada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarCuentas();
                }
                else
                {
                    MessageBox.Show("Error al crear la cuenta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCuentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCuentas.Rows[e.RowIndex];
                cuentaIdSeleccionada = Convert.ToInt32(row.Cells["Id"].Value);
                CargarMovimientos();
                ActualizarResumen();
            }
        }

        private void CargarMovimientos()
        {
            if (cuentaIdSeleccionada == 0) return;

            try
            {
                var movimientos = controladorMovimiento.ObtenerHistorialMovimientos(cuentaIdSeleccionada);

                var movimientosDisplay = movimientos.Select(m => new
                {
                    m.Id,
                    Fecha = m.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    m.Descripcion,
                    Tipo = m.Tipo.ToString(),
                    Monto = m.Monto.ToString("C")
                }).ToList();

                dgvMovimientos.DataSource = null;
                dgvMovimientos.DataSource = movimientosDisplay;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarResumen()
        {
            if (cuentaIdSeleccionada == 0) return;

            try
            {
                var resumen = controladorMovimiento.ObtenerResumenCuenta(cuentaIdSeleccionada);

                lblTotalCreditos.Text = $"Total Créditos: {resumen.TotalCreditos:C}";
                lblTotalDebitos.Text = $"Total Débitos: {resumen.TotalDebitos:C}";
                lblSaldo.Text = $"SALDO ACTUAL: {resumen.Saldo:C}";

                if (resumen.Saldo > 0)
                    lblSaldo.ForeColor = System.Drawing.Color.Green;
                else if (resumen.Saldo < 0)
                    lblSaldo.ForeColor = System.Drawing.Color.Red;
                else
                    lblSaldo.ForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar resumen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegistrarDebito_Click(object sender, EventArgs e)
        {
            if (cuentaIdSeleccionada == 0)
            {
                MessageBox.Show("Debe seleccionar una cuenta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarMovimiento()) return;

            try
            {
                bool resultado = controladorMovimiento.RegistrarDebito(
                    cuentaIdSeleccionada,
                    txtDescripcion.Text.Trim(),
                    decimal.Parse(txtMonto.Text)
                );

                if (resultado)
                {
                    MessageBox.Show("Débito registrado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCamposMovimiento();
                    CargarMovimientos();
                    ActualizarResumen();
                    CargarCuentas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegistrarCredito_Click(object sender, EventArgs e)
        {
            if (cuentaIdSeleccionada == 0)
            {
                MessageBox.Show("Debe seleccionar una cuenta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarMovimiento()) return;

            try
            {
                bool resultado = controladorMovimiento.RegistrarCredito(
                    cuentaIdSeleccionada,
                    txtDescripcion.Text.Trim(),
                    decimal.Parse(txtMonto.Text)
                );

                if (resultado)
                {
                    MessageBox.Show("Crédito registrado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCamposMovimiento();
                    CargarMovimientos();
                    ActualizarResumen();
                    CargarCuentas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            CargarCuentas();
            if (cuentaIdSeleccionada > 0)
            {
                CargarMovimientos();
                ActualizarResumen();
            }
        }

        private bool ValidarMovimiento()
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                MessageBox.Show("El monto es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMonto.Text, out decimal monto) || monto <= 0)
            {
                MessageBox.Show("El monto debe ser un número positivo", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimpiarCamposMovimiento()
        {
            txtDescripcion.Clear();
            txtMonto.Clear();
            txtDescripcion.Focus();
        }

        private void FormCuentas_Load(object sender, EventArgs e)
        {

        }
    }
}