using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using USe.Common.AppLogger;

namespace USeFuturesSpirit
{
    static class Program
    {
        public static string AppTitle = "USe 期货套利";
        public static string AppName = "USe期货套利";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            Environment.CurrentDirectory = fileInfo.DirectoryName;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(OnAppThreadException);

            if (!InitAppLoggers())
            {
                return;
            }

            IAppLogger logger = AppLogger.SingleInstance;
            Debug.Assert(logger != null);

            logger.LineFeed();
            logger.WriteInformation(AppName + " started.");

            if (!InitUseManager())
            {
                goto End;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                LoginForm loginForm = new LoginForm();
                Application.Run(loginForm);

                if (USeManager.Instance.LoginUser != null)
                {
                    if (USeManager.Instance.ProcessBeforStartWork() == false)
                    {
                        goto End;
                    }

                    USeManager.Instance.Start();
                    MainForm form = new MainForm();
                    Application.Run(form);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Run Application failed, Error: " + ex.Message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        End:
            USeManager.Instance.Close();

            logger.WriteInformation(AppName + " shutdown.");
            logger.Flush();
        }

        #region 初始化方法
        private static bool InitAppLoggers()
        {
            IAppLogger logger = null;
            try
            {
                logger = AppLogger.InitInstance();
            }
            catch (Exception ex)
            {
                string text = "Create AppLogger failed, Error: " + ex.Message;
                MessageBox.Show(text, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static bool InitUseManager()
        {
            try
            {
                USeManager.Instance.EventLogger = AppLogger.SingleInstance;
                USeManager.Instance.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialize UseManager failed, " + ex.Message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        #endregion

        #region 异常处理方法
        private static Exception ms_ignoredException = null;

        private static void OnAppThreadException(object source, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string text;

            IAppLogger logger = AppLogger.SingleInstance;
            if (logger != null)
            {
                text = AppName + " catched AppThreadException:\r\n" + ex.ToString();
                logger.WriteError(text);
                logger.Flush();
                //AppLogger.ClearInstance();
            }

            text = String.Format(AppName + " catched AppThreadException:\r\n{0}\r\n\r\nDo you want EXIT?", ex.Message);
            DialogResult result = MessageBox.Show(text, AppTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                ms_ignoredException = null;
            }
            else
            {
                ms_ignoredException = ex;
                Application.Exit();
            }
        }

        private static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            if (ex == ms_ignoredException)
            {
                return;
            }

            string text;

            IAppLogger logger = AppLogger.SingleInstance;
            if (logger != null)
            {
                text = AppName + " catched Domain UnhandledException:\r\n" + ex.ToString();
                logger.WriteError(text);
                logger.Flush();
                //AppLogger.ClearInstance();
            }

            text = AppName + " catched Domain UnhandledException:\r\n\r\n" + ex.Message;
            MessageBox.Show(text, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion
    }
}
