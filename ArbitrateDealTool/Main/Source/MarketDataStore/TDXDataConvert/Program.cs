using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USe.Common.DBDriver;
using System.Data;
namespace TDXDataConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            //select* from alpha.day_kline where contract like '%l9'
            string strl = "select contract, exchange from alpha.day_kline  where contract like '%999' and exchange = 'CZCE' group by contract,exchange";
             DataTable table =  MySQLDriver.GetTableFromDB("", strl);
            foreach(DataRow row in table.Rows)
            {
                string contract = row["contract"].ToString();
                string update = "update set ";

                //郑州
                string newContract = contract.Substring(0, contract.Length - 2) + "9999";

                //郑州
                //string newContract = contract.Substring(0, contract.Length - 2).ToLower() + "9999";
            }
        }
    }
}
