using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace CtpInstrumentDownLoad
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

            //下载
            DownloadServies downloadServices = new DownloadServies();
            int result = downloadServices.Start();

            //DownloadServiesHistory downloadServiceHistory = new DownloadServiesHistory();
            //int result_his = downloadServiceHistory.Start();

            //Console.WriteLine("==>Press any key to release ...");
            //Console.ReadKey(true);

            return result;
        }
    }
}
