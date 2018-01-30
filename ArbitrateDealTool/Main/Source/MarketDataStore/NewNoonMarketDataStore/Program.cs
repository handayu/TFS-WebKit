using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NewNoonMarketDataStore
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

            DownLoadService downloadServices = new DownLoadService();
            int result = downloadServices.Start();

            return result;
        }
    }

}
