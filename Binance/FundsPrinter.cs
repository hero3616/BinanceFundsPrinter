using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Binance
{
    class FundsPrinter
    {
        private IList<Coin> _coins;
        private bool _calculateUSDCost = ConfigHelper.CalculateUSDCost;
        private bool _includePercentChange = ConfigHelper.IncludePercentChange;

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

            var s = 0;
            var formatStr = $"{{{s++},4}} {{{s++},11}} {{{s++},11}} {{{s++},10}}";
			var cParams = new List<object> { "Coin", "Total Bal", "BTC Value", "USD Value" };

            if (_includePercentChange)
            {
                formatStr += $" {{{s++},7}} {{{s++},7}} {{{s++},8}}";
                cParams.AddRange(new object[] { "1h %", "24h %", "7d %" });
            }

            if(_calculateUSDCost)
            {
                formatStr += $" {{{s++},11}} {{{s++},10}}";
                cParams.AddRange(new object[] { "ETH Cost", "USD Cost" });
            }

            Console.WriteLine(formatStr.Replace(" ", ""), cParams.ToArray());
            DrawLine(formatStr);

            var scoins = _coins.OrderByDescending(c => c.USDValue).ToList();

            for(int i = 0; i < scoins.Count; i++)
            {
                var coin = scoins[i];
                var usdValue = Math.Round(coin.USDValue, 2);
                var totalBalance = Math.Round(coin.TotalBalance, 4);
                var f = formatStr.Split(' ');

                var abbr = coin.Abbreviation;
                if (!string.IsNullOrEmpty(abbr) && abbr.Length > 4)
                    abbr = abbr.Substring(0, 4);               

				s = 0;
                Console.Write(R(f[s++]), abbr);
                Console.Write(R(f[s++]), totalBalance);
                Console.Write(R(f[s++]), coin.BTCValue);
                Console.Write(R(f[s++]), usdValue);

                if (_includePercentChange)
                {
                    ColorWrite(R(f[s++]), coin.Percentage1h);
                    ColorWrite(R(f[s++]), coin.Percentage24h);
                    ColorWrite(R(f[s++]), coin.Percentage7d);
                }

                if(_calculateUSDCost)
                {
                    Console.Write(R(f[s++]), Math.Round(coin.ETHCost, 8));
                    Console.Write(R(f[s++]), Math.Round(coin.USDCost, 2));
                }

                Console.WriteLine();
            }

            var btcTotal = scoins.Sum(c => c.BTCValue);
            var usdTotal = Math.Round(scoins.Sum(c => c.USDValue), 2).ToString("C");
            var ethCostTotal = Math.Round(scoins.Sum(c => c.ETHCost), 8);
            var usdCostTotal = Math.Round(scoins.Sum(c => c.USDCost), 2).ToString("C");

            DrawLine(formatStr);
            cParams = new List<object> { string.Empty, "Total", btcTotal, usdTotal };

            if(_includePercentChange)
            {
                cParams.AddRange(new object[] { string.Empty, string.Empty, string.Empty });
            }

            if(_calculateUSDCost)
            {
                cParams.AddRange(new object[] { ethCostTotal, usdCostTotal });
            }

            Console.WriteLine(formatStr.Replace(" ", ""), cParams.ToArray());
        }

        private void ColorWrite(string formatStr, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
				Console.Write(formatStr, string.Empty);
                return;
            }

            if (value.StartsWith("-", StringComparison.CurrentCulture))
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(formatStr, value);
            Console.ResetColor();
        }

        private string R(string s)
        {
            var r = Regex.Replace(s, @"^\{\d+,", "{0,");
            return r;
        }

        private void DrawLine(string formatStr)
        {
            var matches = Regex.Matches(formatStr, @",\d+\}");
            var count = 0;
            foreach (Match match in matches)
            {
                var digit = Regex.Match(match.Value, @"\d+").Value;
                count += int.Parse(digit);
            }

            Console.WriteLine(new string('-', count));
        }
    }
}
