using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TradeCalendarManager
{
    class Program
    {
        static int Main(string[] args)
        {
            AssemblyName asmName = Assembly.GetEntryAssembly().GetName();
            Debug.Assert(asmName != null);

            string appName = String.Format("{0} V{1}", asmName.Name, asmName.Version.ToString());
            Console.Title = appName;

            FileInfo fileInfo = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            System.Environment.CurrentDirectory = fileInfo.DirectoryName;

            int existCode = 0;
            TradingDayManagerServer server = new TradingDayManagerServer();
            try
            {
                existCode = server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey(true);
            }

            //Console.WriteLine("==>Press any key to release ...");
            //Console.ReadKey(true);

            return existCode;
        }
    }
}
