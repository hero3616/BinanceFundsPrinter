using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = ConfigurationManager.AppSettings["apikey"];
            var secret = ConfigurationManager.AppSettings["secret"];
            var funds = new Funds(key, secret).Run();
            var printer = new FundsPrinter(funds);
            printer.Print();
        }
    }
}
