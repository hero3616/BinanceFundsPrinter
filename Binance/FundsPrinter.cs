using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            Console.Write("\r");

            if (_coins.Count == 0)
            {
                Console.WriteLine("No funds found");
                return;
            }

            var s = 0;
            var formatStr = $"{{{s++},4}} {{{s++},10}} {{{s++},11}} {{{s++},11}}";
            var cParams = new List<object> { "Coin", "Balance ", "BTC Value", "  $ Value" };

            if (ConfigHelper.DisplayUnitUSDValue)
            {
                formatStr += $" {{{s++},9}}";
                cParams.AddRange(new object[] { "Unit" });
            }

            if (ConfigHelper.DisplayPercentage1h)
            {
                formatStr += $" {{{s++},7}}";
                cParams.AddRange(new object[] { "1h%" });
            }

            if (ConfigHelper.DisplayPercentage24h)
            {
                formatStr += $" {{{s++},7}}";
                cParams.AddRange(new object[] { "24h%" });
            }

            if (ConfigHelper.DisplayPercentage7d)
            {
                formatStr += $" {{{s++},8}}";
                cParams.AddRange(new object[] { "7d%" });
            }

            if (ConfigHelper.DisplayETHCost)
            {
                formatStr += $" {{{s++},11}}";
                cParams.AddRange(new object[] { "ETH Cost" });
            }

            if (ConfigHelper.CalculateUSDCost)
            {
                formatStr += $" {{{s++},11}}";
                cParams.AddRange(new object[] { "  $ Cost" });
            }

            if (ConfigHelper.DisplayProfit)
            {
                formatStr += $" {{{s++},9}}";
                cParams.AddRange(new object[] { "Profit" });
            }

            if (ConfigHelper.DisplayProfitPercent)
            {
                formatStr += $" {{{s++},8}}";
                cParams.AddRange(new object[] { "USD% " });
            }

            if (ConfigHelper.DisplayRank)
            {
                formatStr += $" {{{s++},5}}";
                cParams.AddRange(new object[] { "Rank" });
            }

            if (ConfigHelper.RepeatCoinLastColumn)
            {
                formatStr += $" {{{s++},5}}";
                cParams.AddRange(new object[] { "Coin" });
            }

            DrawLine(formatStr);
            Console.WriteLine(formatStr.Replace(" ", ""), cParams.ToArray());
            DrawLine(formatStr);

            IList<Coin> scoins;

            switch (ConfigHelper.OrderDescendingBy)
            {
                case "USDValue":
                    scoins = _coins.OrderByDescending(c => c.USDValue).ToList();
                    break;
                case "USDCost":
                    scoins = _coins.OrderByDescending(c => c.USDCost).ToList();
                    break;
                case "ProfitPercent":
                    scoins = _coins.OrderByDescending(c => c.ProfitPercent != 0).ThenByDescending(c => c.ProfitPercent).ToList();
                    break;
                default:
                    scoins = _coins.OrderByDescending(c => c.USDValue).ToList();
                    break;
            }

            for (int i = 0; i < scoins.Count; i++)
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

                if (ConfigHelper.DisplayUnitUSDValue)
                    Console.Write(R(f[s++]), Math.Round(coin.UnitUSDValue, 2));

                if (ConfigHelper.DisplayPercentage1h)
                    ColorWrite(R(f[s++]), coin.Percentage1h);

                if (ConfigHelper.DisplayPercentage24h)
                    ColorWrite(R(f[s++]), coin.Percentage24h);

                if (ConfigHelper.DisplayPercentage7d)
                    ColorWrite(R(f[s++]), coin.Percentage7d);

                if (ConfigHelper.DisplayETHCost)
                    Console.Write(R(f[s++]), Math.Round(coin.ETHCost, 8));

                if (ConfigHelper.CalculateUSDCost)
                    Console.Write(R(f[s++]), Math.Round(coin.USDCost, 2));

                if (ConfigHelper.DisplayProfit)
                    ColorWrite(R(f[s++]), coin.USDValue, coin.USDCost);

                if (ConfigHelper.DisplayProfitPercent)
                    ColorWrite(R(f[s++]), Math.Round(coin.ProfitPercent, 2).ToString());

                if (ConfigHelper.DisplayRank)
                    Console.Write(R(f[s++]), coin.Rank);

                if (ConfigHelper.RepeatCoinLastColumn)
                    ColorWrite(R(f[s++]), abbr, coin.USDValue, coin.USDCost);

                Console.WriteLine();
            }

            var btcTotal = scoins.Sum(c => c.BTCValue);
            var usdTotal = Math.Round(scoins.Sum(c => c.USDValue), 2).ToString("C");
            var ethCostTotal = Math.Round(scoins.Sum(c => c.ETHCost), 8);
            var usdCostTotal = Math.Round(scoins.Sum(c => c.USDCost), 2).ToString("C");

            DrawLine(formatStr);
            cParams = new List<object> { string.Empty, "Total", btcTotal, usdTotal };

            if (ConfigHelper.DisplayUnitUSDValue)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.DisplayPercentage1h)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.DisplayPercentage24h)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.DisplayPercentage7d)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.DisplayETHCost)
                cParams.AddRange(new object[] { ethCostTotal });

            if (ConfigHelper.CalculateUSDCost)
                cParams.AddRange(new object[] { usdCostTotal });

            if (ConfigHelper.DisplayProfit)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.DisplayProfitPercent)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.DisplayRank)
                cParams.AddRange(new object[] { string.Empty });

            if (ConfigHelper.RepeatCoinLastColumn)
                cParams.AddRange(new object[] { string.Empty });

            Console.WriteLine(formatStr.Replace(" ", ""), cParams.ToArray());
        }

        private void ColorWrite(string formatStr, string value, decimal usdValue, decimal usdCost)
        {
            var val = Math.Round(usdValue, 2);
            var cost = Math.Round(usdCost, 2);
            if (usdValue == 0.00m || usdCost == 0.00m)
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

            if (value == "0" || value == "0.00")
            {
                Console.Write(formatStr, value);
                return;
            }

            if (value.StartsWith("-", StringComparison.CurrentCulture))
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(formatStr, value);
            Console.ResetColor();
        }

        private void ColorWrite(string formatStr, decimal usdValue, decimal usdCost)
        {
            var val = Math.Round(usdValue, 2);
            var cost = Math.Round(usdCost, 2);
            var profit = 0m;
            if (usdValue > 0.00m && usdCost > 0.00m)
                profit = Math.Round(usdValue - usdCost, 2);

            ColorWrite(formatStr, profit.ToString(), usdValue, usdCost);
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
