using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.TradeDriver.Common;
using USe.TradeDriver.Ctp;
namespace USeFuturesSpirit
{
    class YMTestUnit
    {
        public static void TestOrderNum()
        {
            CtpOrderNum num1 = new CtpOrderNum(1, 2, "3", "4", "5");
            CtpOrderNum num2 = new CtpOrderNum(1, 2, "3", "4", "5");

            USeOrderNum num3 = num1;
            USeOrderNum num4 = num2;

            bool result = false;
            result = (num1 == num2);
            Console.WriteLine("(num1 == num2) " + result.ToString());
            result = num1.Equals(num2);
            Console.WriteLine("(num1.Equals(num2)) " + result.ToString());
            result = (num3 == num4);
            Console.WriteLine("(num3 == num4) " + result.ToString());
            result = num3.Equals(num4);
            Console.WriteLine("num3.Equals(num4) " + result.ToString());
            Console.WriteLine();
        }
    }
}
