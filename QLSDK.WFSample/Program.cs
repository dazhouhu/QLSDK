using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.WFSample
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (loginWindow.DialogResult == DialogResult.OK)
            {
                Application.Run(new MainWindow());
            }
            else
            {
                return;
            }
        }
    }
}
