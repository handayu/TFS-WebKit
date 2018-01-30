using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using USe.Common.AppLogger;
using USe.Common;
using System.Configuration;
using USe.TradeDriver.Common;

namespace HistoryKLineImport
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

            string text = string.Empty;
            string importDataTypeValue = ConfigurationManager.AppSettings["ImportDataType"];
            if (string.IsNullOrEmpty(importDataTypeValue))
            {
                text = String.Format("导入数据类型为空");
                logger.WriteError(text);
                USeConsole.WriteLine(text);
                return -1;
            }


            ImportDataType importDataType = (ImportDataType)Enum.Parse(typeof(ImportDataType), importDataTypeValue);
            ImportServices importServices = null;
            switch(importDataType)
            {
                case ImportDataType.DayKLine:
                    importServices = new KLineImportServices(USeCycleType.Day, logger);
                    break;
                case ImportDataType.Min1KLine:
                    importServices = new KLineImportServices(USeCycleType.Min1, logger);
                    break;
                case ImportDataType.ClosePrice2:
                    importServices = new ClosePrice2ImportServices(logger);
                    break;
                case ImportDataType.ContractIndex:
                    importServices = new ContractIndexImportServices(logger);
                    break;
                case ImportDataType.MainContractKLine:
                    importServices = new MainContractKLineImportServices(logger);
                    break;
                default:
                    {
                        string error = string.Format("未知的数据导入类型:{0}", importDataType);
                        logger.WriteError(error);
                        USeConsole.WriteLine(text);
                        return -1;
                    }
            }
            try
            {
                if(importServices.Initialize() == false)
                {
                    return -1;
                }
                importServices.Run();
            }
            catch (Exception ex)
            {
                text = String.Format("导入数据失败,错误:{0}", ex.Message);
                logger.WriteError(text);
                USeConsole.WriteLine(text);
                return -1;
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
