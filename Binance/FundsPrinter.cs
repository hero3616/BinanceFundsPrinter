using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance
{
    class FundsPrinter
    {
        private IList<Coin> _coins;

        public FundsPrinter(IList<Coin> coins)
        {
            _coins = coins;
        }
        public void Print()
        {
            Console.WriteLine();
            if(_coins.Count == 0)
            {
                Console.WriteLine("No funds found");
                return;
            }

            var formatStr = "{0,6} {1, 18} {2, 18} {3, 18}";

            Console.WriteLine(formatStr, "Coin", "Total Balance", "BTC Value", "USD Value");
            Console.WriteLine(new string('-', 66));

            var scoins = _coins.OrderByDescending(c => c.USDValue).ToList();

            for(int i = 0; i < scoins.Count; i++)
            {
                var coin = scoins[i];
                var usdValue = Math.Round(coin.USDValue, 2);
                Console.WriteLine(formatStr, coin.Abbreviation, coin.TotalBalance, coin.BTCValue, usdValue);
            }

            var btcTotal = scoins.Sum(c => c.BTCValue);
            var usdTotal = Math.Round(scoins.Sum(c => c.USDValue), 2).ToString("C");

            Console.WriteLine(new string('-', 66));
            Console.WriteLine(formatStr, string.Empty, "Total", btcTotal, usdTotal);
        }
    }
}
