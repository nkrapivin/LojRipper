using System;
using System.Windows.Forms;

namespace LojRipper
{
    public static class Program
    {
        /// <summary>
        /// Main WinForms application entry point.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
