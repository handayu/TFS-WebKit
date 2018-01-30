using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RefreashSendimentaryMoney
{
    class Program
    {
        static void Main(string[] args)
        {
            SendimentaryMoneyCalService service = new SendimentaryMoneyCalService();
            service.Start();
        }
    }
}
