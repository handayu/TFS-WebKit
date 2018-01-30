using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using USe.Common.AppLogger;
using USe.Common;

namespace MainContractCalculater
{
    internal class Program
    {
        private string m_appName = string.Empty;

        static int Main(string[] args)
        {
            Program program = new Program();
            program.Copyright();
            if (!program.Init())
            {
                return -1;
            }
            int exit = program.Run();   // 运行程序。
            program.Close();            // 关闭程序。
            return exit;
        }

        /// <summary>
        /// 程序属性。
        /// </summary>
        private Program()
        {
            AssemblyName asmName = Assembly.GetEntryAssembly().GetName();
            Debug.Assert(asmName != null);
            m_appName = String.Format("{0} V{1}", asmName.Name, asmName.Version.ToString());
#if DEBUG
            m_appName += "[Debug]";
#endif
        }

        /// <summary>
        /// 程序版权。
        /// </summary>
        private void Copyright()
        {
            Console.WriteLine(string.Format("{0} .", m_appName));
            Console.WriteLine("  Copyright(c) 2017 USe Ltd. All Rights Reserved.");
            Console.WriteLine();
        }

        /// <summary>
        /// 程序初始化。
        /// </summary>
        /// <returns></returns>
        private bool Init()
        {
            Debug.Assert(!String.IsNullOrEmpty(m_appName));
            Console.Title = m_appName;

            FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            System.Environment.CurrentDirectory = fileInfo.DirectoryName;

            IAppLogger logger = null;
            try
            {
                logger = AppLogger.InitInstance();
            }
            catch (Exception ex)
            {
                USeConsole.WriteLine(string.Format("初始化日志失败,错误:{0}.", ex.Message));
                return false;
            }

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            logger.LineFeed();
            logger.WriteInformation(m_appName + " 启动。");

            return true;
        }

        /// <summary>
        /// 程序关闭。
        /// </summary>
        private void Close()
        {
            Debug.Assert(!String.IsNullOrEmpty(m_appName));
            if (AppLogger.SingleInstance != null)
            {
                AppLogger.SingleInstance.WriteInformation(m_appName + " 关闭。");
                AppLogger.ClearInstance();
            }
        }

        /// <summary>
        /// 运行程序。
        /// </summary>
        /// <returns></returns>
        private int Run()
        {
            int exitCode = 0;
            IAppLogger logger = AppLogger.SingleInstance;
            Debug.Assert(logger != null);
            MainContractCalcServies m_mainContractServer = null;
            string message = string.Empty;   // 消息。

            USeConsole.WriteLine();
            try
            {
                m_mainContractServer = new MainContractCalcServies(logger);
                m_mainContractServer.Initialize();
            }
            catch (Exception ex)
            {
                message = String.Format("主力合约计算服务初始化失败,错误:{0}", ex.Message);
                logger.WriteError(message);
                USeConsole.WriteLine(message);
                return -1;
            }

            try
            {
                exitCode = m_mainContractServer.Run();
            }
            catch (Exception ex)
            {
                message = String.Format("主力合约计算服务启动失败,错误:{0}", ex.Message);
                logger.WriteError(message);
                USeConsole.WriteLine(message);
            }

            return exitCode;
        }

        /// <summary>
        /// 未捕获异常。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            USeConsole.WriteLine(string.Format("observed unhandled exception:\r\n{0}", ex.ToString()));

            if (AppLogger.SingleInstance != null)
            {
                AppLogger.SingleInstance.WriteError("[MainContractMarginCalculater] catched unhandled exception:\r\n" + ex.ToString());
            }
        }


    }
}
