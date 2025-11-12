using System;
using System.Windows.Forms;
using SistemaCuentasCorrientes.Vista;

namespace SistemaCuentasCorrientes
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormPrincipal());
        }
    }
}