using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance
{
    public class Coin
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal BTCValue { get; set; }
        public decimal USDValue { get; set; }
    }
}
