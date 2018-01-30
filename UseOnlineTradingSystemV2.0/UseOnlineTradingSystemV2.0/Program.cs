using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            vt1 = new VisualStudio2012DarkTheme();
            //vt2 = new Office2013DarkTheme();
            //vt3 = new VisualStudio2012LightTheme();
            //vt4 = new Windows7Theme();
            //vt5 = new Windows8Theme();

            CefRuntime.Load();
            var mainArgs = new CefMainArgs(new string[] { });
            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;
            settings.SingleProcess = false;
            settings.LogSeverity = CefLogSeverity.Verbose;
            settings.LogFile = "cef.log";
            settings.Locale = "zh-CN";
            settings.ResourcesDirPath = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);
            settings.RemoteDebuggingPort = 20480;
            settings.NoSandbox = true;

            //settings.CachePath = @"E:\mine\工作相关\工作代码\Spot Trade System\UseOnlineTradingSystem\UseOnlineTradingSystem\bin\Debug";

            var app = new BsCefApp();
            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
                return;
            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!settings.MultiThreadedMessageLoop)
            {
                Application.Idle += (sender, e) => { CefRuntime.DoMessageLoopWork(); };
            }

            LoginFm = new FormLogin();
            main = new MainForm();
            //mf = new MainForm();
            //Application.Run(mf);
            // Application.Run(LoginFm);
            try
            {
                Application.Run(LoginFm);
            }
            catch(Exception err)
            {
                Logger.LogError(err.ToString());
            }
            Logger.LogInfo("退出程序!");
            USeManager.Instance.Stop();
            KillProcess();
            //CefRuntime.Shutdown();
            //Environment.Exit(0);
        }
        public static MainForm main;
        public static FormLogin LoginFm;
        public static VisualStudio2012DarkTheme vt1;
        //public static Office2013DarkTheme vt2;
        //public static VisualStudio2012LightTheme vt3;
        //public static Windows7Theme vt4;
        //public static Windows8Theme vt5;

        //查找进程、结束进程
        private static void KillProcess()
        {
            Process[] pro = Process.GetProcesses();//获取已开启的所有进程
            //遍历所有查找到的进程
            foreach (var p in pro)
            {
                //判断此进程是否是要查找的进程
                if (p.ProcessName.ToString().ToLower() == "useonlinetradingsystem")
                {
                    try
                    {
                        p.Kill();//结束进程
                    }                
                    catch
                    { }
                }
            }
        }
    }
}
