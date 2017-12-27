using System;
using System.Collections.Generic;
using System.Linq;
using Binance.API.Csharp.Client;
using NoobsMuc.Coinmarketcap.Client;

namespace Binance
{
    class Funds
    {
        private BinanceClient _client;
        private bool _calculateUSDCost = ConfigHelper.CalculateUSDCost;
        private bool _includePercentChange = ConfigHelper.IncludePercentChange;
        private IList<EtherPrice> _etherPriceList;
        private IList<Currency> _currencyList;

        public Funds(BinanceClient c)
        {
            _client = c;
            if(_calculateUSDCost)
            {
                _etherPriceList = EtherPrice.ReadList();
            }

            if(_includePercentChange)
            {
				ICoinmarketcapClient client = new CoinmarketcapClient();
                _currencyList = client.GetCurrencies(400).ToList();
            }
        }

        internal IList<Coin> Run()
        {
            var accountInfo = _client.GetAccountInfo().Result;
            var balances = accountInfo.Balances.ToList();
            var tickerPrices = _client.GetAllPrices().Result;
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
                            if(_calculateUSDCost)
                            {
								var cost = GetETHCost(balance.Asset);
								coin.ETHCost = cost.Item1;
								coin.USDCost = cost.Item2;
                            }
                            if(_includePercentChange)
                            {
                                var currency = _currencyList.Where(c => c.Symbol == balance.Asset).FirstOrDefault();
                                if (currency != null)
                                {
                                    coin.Percentage1h = currency.PercentChange1h;
                                    coin.Percentage24h = currency.PercentChange24h;
                                    coin.Percentage7d = currency.PercentChange7d;
                                }                                
                            }
                            coins.Add(coin);
                        }
                    }
                }
            }

            return coins;
        }

        internal Tuple<decimal, decimal> GetETHCost(string coin)
        {
            if (coin == "ETH")
                return Tuple.Create(0m, 0m);

            var ethCost = 0m;
            var usdCost = 0m;

            var tradeList = _client.GetTradeList(coin + "ETH").Result;
            foreach (var trade in tradeList)
            {
                if (!trade.IsBuyer)
                    continue;
                var ethValue = trade.Quantity * trade.Price;
                ethCost += ethValue;
                usdCost += CovertETHtoUSDbyDate(ethValue, trade.Time);
            }

            return Tuple.Create(ethCost, usdCost);
        }

        internal decimal CovertETHtoUSDbyDate(decimal ethAmount, long tradeTime)
        {
            var tradeDateTime = DateTimeHelper.UnixTimeToDateTime(tradeTime / 1000);
            var etherPrice = _etherPriceList.Where(p => p.DateUtcUnix.Date == tradeDateTime.Date).FirstOrDefault();
            if(etherPrice != null)
            {
                return etherPrice.USDValue * ethAmount;
            }

            return 0;
        }
    }
}
