using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USe.Common
{
    /// <summary>
    /// USe控制台。
    /// </summary>
    public static class USeConsole
    {
        /// <summary>
        /// 
        /// </summary>
        public static void WriteLine()
        {
            Console.WriteLine();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void WriteLine(string value)
        {
            Console.WriteLine(string.Format("==> {0:HH:mm:ss.fff} {1}", DateTime.Now, value));
        }
    }
}
