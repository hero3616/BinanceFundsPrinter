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
        private bool _displayETHCost = ConfigHelper.DisplayETHCost;
        private bool _displayPercentage1h = ConfigHelper.DisplayPercentage1h;
        private bool _displayPercentage24h = ConfigHelper.DisplayPercentage24h;
        private bool _displayPercentage7d = ConfigHelper.DisplayPercentage7d;

        public FundsPrinter(IList<Coin> coins)
        {
            _coins = coins;
        }

        public void Print()
        {
			Console.Write("\r");

			if (_coins.Count == 0)
            {
                Console.WriteLine("No funds found");
                return;
            }

			var s = 0;
            var formatStr = $"{{{s++},4}} {{{s++},10}} {{{s++},11}} {{{s++},11}}";
			var cParams = new List<object> { "Coin", "Total Bal", "BTC Value", "USD Value" };

            if (_displayPercentage1h)
            {
                formatStr += $" {{{s++},7}}";
                cParams.AddRange(new object[] { "1h%" });
            }

            if (_displayPercentage24h)
            {
                formatStr += $" {{{s++},7}}";
                cParams.AddRange(new object[] { "24h%" });
            }

            if (_displayPercentage7d)
            {
                formatStr += $" {{{s++},8}}";
                cParams.AddRange(new object[] { "7d%" });
            }

            if(_displayETHCost)
            {
                formatStr += $" {{{s++},11}}";
                cParams.AddRange(new object[] { "ETH Cost" });
            }

            if(_calculateUSDCost)
            {
                formatStr += $" {{{s++},11}}";
                cParams.AddRange(new object[] { "USD Cost" });
            }

			DrawLine(formatStr);
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
                ColorWrite(R(f[s++]), abbr, coin.USDValue, coin.USDCost);
                Console.Write(R(f[s++]), totalBalance);
                Console.Write(R(f[s++]), coin.BTCValue);
                Console.Write(R(f[s++]), usdValue);

                if(_displayPercentage1h)
                    ColorWrite(R(f[s++]), coin.Percentage1h);

                if (_displayPercentage24h)
                    ColorWrite(R(f[s++]), coin.Percentage24h);

                if (_displayPercentage7d)
                    ColorWrite(R(f[s++]), coin.Percentage7d);

                if(_displayETHCost)
					Console.Write(R(f[s++]), Math.Round(coin.ETHCost, 8));               

                if(_calculateUSDCost)
                    Console.Write(R(f[s++]), Math.Round(coin.USDCost, 2));

                Console.WriteLine();
            }

            var btcTotal = scoins.Sum(c => c.BTCValue);
            var usdTotal = Math.Round(scoins.Sum(c => c.USDValue), 2).ToString("C");
            var ethCostTotal = Math.Round(scoins.Sum(c => c.ETHCost), 8);
            var usdCostTotal = Math.Round(scoins.Sum(c => c.USDCost), 2).ToString("C");

            DrawLine(formatStr);
            cParams = new List<object> { string.Empty, "Total", btcTotal, usdTotal };

            if(_displayPercentage1h)
                cParams.AddRange(new object[] { string.Empty });

            if (_displayPercentage24h)
                cParams.AddRange(new object[] { string.Empty });

            if (_displayPercentage7d)
                cParams.AddRange(new object[] { string.Empty });

            if(_displayETHCost)
                cParams.AddRange(new object[] { ethCostTotal });

            if(_calculateUSDCost)
                cParams.AddRange(new object[] { usdCostTotal });

            Console.WriteLine(formatStr.Replace(" ", ""), cParams.ToArray());
        }

        private void ColorWrite(string formatStr, string value, decimal usdValue, decimal usdCost)
        {
            var val = Math.Round(usdValue, 2);
            var cost = Math.Round(usdCost, 2);
            if(usdValue == 0.00m || usdCost == 0.00m)
            {
                Console.Write(formatStr, value);
                return;
            }

            if (val == cost)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (val > cost)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write(formatStr, value);
            Console.ResetColor();
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

            Console.WriteLine(new string('=', count));
        }
    }
}
