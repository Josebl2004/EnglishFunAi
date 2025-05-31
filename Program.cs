using System;
using System.Windows.Forms;

namespace EnglishFunAI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(login.NombreUsuario))
            {
                Application.Run(new MainForm(login.NombreUsuario));
            }
        }    
    }
}