namespace SistemaCuentasCorrientes.Vista
{
    partial class FormCuentas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // FormCuentas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "FormCuentas";
            Text = "FormCuentas";
            Load += FormCuentas_Load;
            ResumeLayout(false);
        }
    }
}