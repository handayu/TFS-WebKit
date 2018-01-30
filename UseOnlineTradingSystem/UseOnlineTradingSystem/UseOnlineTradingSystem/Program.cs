using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
            mf = new MainForm();
            Application.Run(mf);
            //Application.Run(new Form1());
            Logger.LogInfo("退出程序!");
            USeManager.Instance.Stop();
            CefRuntime.Shutdown();
            Environment.Exit(0);
        }
        public static MainForm mf;
    }
}
