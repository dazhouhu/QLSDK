using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //处理未捕获的异常   
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常   
                Application.ThreadException += OnApplicationThreadException;
                //处理非UI线程异常   
                AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
                //应用程序退出
                Application.ApplicationExit += OnApplicationExit;

                var mainWindow = new MainWindow();
                mainWindow.Show();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if (loginWindow.DialogResult == DialogResult.OK)
                {
                    Application.Run(mainWindow);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")) + ",请重启程序。");
            }
        }
        //处理UI线程异常
        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
        }
        //处理非UI线程异常 
        private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show("系统出现异常：" + (ex.Message + " " + (ex.InnerException != null && ex.InnerException.Message != null && ex.Message != ex.InnerException.Message ? ex.InnerException.Message : "")));
        }
        //应用程序退出
        private static void OnApplicationExit(object sender,EventArgs e)
        {
            QLSDK.Core.QLManager.GetInstance().Release();
        }
    }
}
