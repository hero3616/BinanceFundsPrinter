using Binance.API.Csharp.Client;
using Binance.API.Csharp.Client.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binance
{
    class Funds
    {
        private string _key;
        private string _secret;
  
        public Funds(string k, string s)
        {
            _key = k;
            _secret = s;
        }

        internal IList<Coin> Run()
        {
            var apiClient = new ApiClient(_key, _secret);
            var binanceClient = new BinanceClient(apiClient);

            var accountInfo = binanceClient.GetAccountInfo().Result;
            var balances = accountInfo.Balances.ToList();
            var tickerPrices = binanceClient.GetAllPrices().Result;
            var symbolPrices = tickerPrices.ToList();
            var btcusdt = symbolPrices.Where(p => p.Symbol == "BTCUSDT").First().Price;

            var coins = new List<Coin>();

            for (int i = 0; i < balances.Count; i++)
            {
                var balance = balances[i];
                if (balance.Free > 0)
                {
                    var coin = new Coin
                    {
                        Abbreviation = balance.Asset,
                        TotalBalance = balance.Free
                    };

                    for (int j = 0; j < symbolPrices.Count; j++)
                    {
                        var symbolPrice = symbolPrices[j];
                        if (symbolPrice.Symbol == balance.Asset + "BTC")
                        {
                            coin.BTCValue = Math.Round(balance.Free * symbolPrice.Price, 8);
                            coin.USDValue = Math.Round(coin.BTCValue * btcusdt, 8);
                            coins.Add(coin);
                        }
                    }
                }
            }

            return coins;
        }
    }
}
