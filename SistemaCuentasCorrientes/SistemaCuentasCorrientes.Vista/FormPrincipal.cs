using System;
using System.Windows.Forms;

namespace SistemaCuentasCorrientes.Vista
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Configuración del formulario
            this.Text = "Sistema de Cuentas Corrientes";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel principal
            Panel panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Título
            Label lblTitulo = new Label
            {
                Text = "SISTEMA DE CUENTAS CORRIENTES",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(80, 30)
            };

            // Botones del menú
            Button btnClientes = new Button
            {
                Text = "Gestión de Clientes",
                Size = new System.Drawing.Size(300, 50),
                Location = new System.Drawing.Point(90, 100),
                Font = new System.Drawing.Font("Arial", 12)
            };
            btnClientes.Click += BtnClientes_Click;

            Button btnCuentas = new Button
            {
                Text = "Gestión de Cuentas Corrientes",
                Size = new System.Drawing.Size(300, 50),
                Location = new System.Drawing.Point(90, 170),
                Font = new System.Drawing.Font("Arial", 12)
            };
            btnCuentas.Click += BtnCuentas_Click;

            Button btnMovimientos = new Button
            {
                Text = "Registrar Movimientos",
                Size = new System.Drawing.Size(300, 50),
                Location = new System.Drawing.Point(90, 240),
                Font = new System.Drawing.Font("Arial", 12)
            };
            btnMovimientos.Click += BtnMovimientos_Click;

            // Agregar controles al panel
            panelPrincipal.Controls.Add(lblTitulo);
            panelPrincipal.Controls.Add(btnClientes);
            panelPrincipal.Controls.Add(btnCuentas);
            panelPrincipal.Controls.Add(btnMovimientos);

            // Agregar panel al formulario
            this.Controls.Add(panelPrincipal);
        }

        private void BtnClientes_Click(object sender, EventArgs e)
        {
            FormClientes formClientes = new FormClientes();
            formClientes.ShowDialog();
        }

        private void BtnCuentas_Click(object sender, EventArgs e)
        {
            FormCuentas formCuentas = new FormCuentas();
            formCuentas.ShowDialog();
        }

        private void BtnMovimientos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Formulario de movimientos en desarrollo", "Información");
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}