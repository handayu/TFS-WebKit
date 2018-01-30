using System;
using System.Collections.Generic;
using log4net;
namespace MarketDataStore
{
    public enum LogLevel : byte
    {
        Fatal = 1,
        Error = 2,
        Warning = 3,
        Info = 4,
        Debug = 5
    }

    public class Log
    {
        public LogLevel level;
        public string msg;
        public DateTime time;
        public Log(LogLevel l, string s) { level = l; msg = s; time = DateTime.Now; }
        public override string ToString()
        {
            return "[" + time.ToString("HH:mm:ss.fff") + "] " + level.ToString() + " - " + msg;
        }
    }

    public class Logger
    {
        public static LogLevel Level = LogLevel.Info;
        private const Int32 MaxLogEntry = 1000;
        private static List<Log> lstLog = new List<Log>(MaxLogEntry);
        public static event EventHandler<NewLogEventArgs> NewLog;
        private static ILog netLogger;

        static Logger()
        {
            netLogger = log4net.LogManager.GetLogger("Logger");
        }

        public static void Release()
        {
            log4net.LogManager.Shutdown();
        }

        private static void LogMsg(string msg, LogLevel level)
        {
            Log log = new Log(level, msg);
            lock (lstLog)
            {
                if (lstLog.Count >= MaxLogEntry)
                {
                    lstLog.RemoveRange(0, (Int32)Math.Floor(lstLog.Count / 2.0));
                }
                lstLog.Add(log);
            }
            if (null != NewLog)
            {
                NewLog(null, new NewLogEventArgs(log));
            }
        }

        public static List<string> GetAllLog(LogLevel level = LogLevel.Info)
        {
            List<string> list = new List<string>();
            lock (lstLog)
            {
                foreach (Log l in lstLog)
                {
                    if ((byte)l.level > (byte)level)
                    {
                        continue;
                    }
                    list.Add(l.ToString());
                }
            }
            return list;
        }

        public static void LogError(string msg)
        {
            LogMsg(msg, LogLevel.Error);
            netLogger.Error(msg);
        }

        public static void LogError(Exception e)
        {
            string msg = e.Message + e.GetType().ToString() + e.StackTrace;
            LogError(msg);
        }

        public static void LogWarning(string msg)
        {
            LogMsg(msg, LogLevel.Warning);
            netLogger.Warn(msg);
        }

        public static void LogInfo(string msg)
        {
            LogMsg(msg, LogLevel.Info);
            netLogger.Info(msg);
        }


        public static void LogDebug(string msg)
        {
            LogMsg(msg, LogLevel.Debug);
            netLogger.Debug(msg);
        }

    }

    public class NewLogEventArgs : EventArgs
    {
        public Log log;
        public NewLogEventArgs(Log l)
        {
            log = l;
        }
    }
}