﻿using Binance.API.Csharp.Client;
using NoobsMuc.Coinmarketcap.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Binance
{
    class Funds
    {
        private BinanceClient _client;
        private IList<EtherPrice> _etherPriceList;
        private IList<Currency> _currencyList;

        public Funds(BinanceClient c)
        {
            _client = c;
            InitEtherPriceList();
            InitCurrencyList();
        }

        private void InitEtherPriceList()
        {
            if (ConfigHelper.CalculateUSDCost)
            {
                _etherPriceList = EtherPrice.ReadListFromUrl().GetAwaiter().GetResult();
            }
        }

        private void InitCurrencyList()
        {
            if (ConfigHelper.DisplayPercentage1h ||
                ConfigHelper.DisplayPercentage24h ||
                ConfigHelper.DisplayPercentage7d ||
                ConfigHelper.DisplayUnitUSDValue ||
                ConfigHelper.DisplayRank
               )
            {
                ICoinmarketcapClient client = new CoinmarketcapClient();
                _currencyList = client.GetCurrencies(ConfigHelper.CoinMarketCapFetchCount).ToList();
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
                            if (ConfigHelper.CalculateUSDCost)
                            {
                                var cost = GetETHCost(balance.Asset);
                                coin.ETHCost = cost.Item1;
                                coin.USDCost = cost.Item2;
                            }

                            var adjustedAbbreviation = GetAdjustedAbbreviation(coin.Abbreviation);
                            var currency = _currencyList.Where(c => c.Symbol == adjustedAbbreviation).FirstOrDefault();
                            var priceUsd = 0m;
                            if (ConfigHelper.DisplayUnitUSDValue && decimal.TryParse(currency.PriceUsd, out priceUsd))
                            {
                                coin.UnitUSDValue = priceUsd;
                            }

                            if (ConfigHelper.DisplayPercentage1h || ConfigHelper.DisplayPercentage24h || ConfigHelper.DisplayPercentage7d)
                            {
                                if (currency != null)
                                {
                                    coin.Percentage1h = currency.PercentChange1h;
                                    coin.Percentage24h = currency.PercentChange24h;
                                    coin.Percentage7d = currency.PercentChange7d;
                                }
                            }

                            var rank = 0;
                            if (ConfigHelper.DisplayRank && int.TryParse(currency.Rank, out rank))
                            {
                                coin.Rank = rank;
                            }

                            coins.Add(coin);
                        }
                    }
                }
            }

            return coins;
        }

        private string GetAdjustedAbbreviation(string abbreviation)
        {
            var abbr = abbreviation == "IOTA" ? "MIOTA" : abbreviation;
            return abbr;
        }

        internal Tuple<decimal, decimal> GetETHCost(string coin)
        {
            if (coin == "ETH")
                return Tuple.Create(0m, 0m);

            if (coin == "BNB")
                return Tuple.Create(0m, 0m);

            var ethCost = 0m;
            var usdCost = 0m;

			try
			{
				var tradeList = _client.GetTradeList(coin + "ETH").Result;

				foreach (var trade in tradeList)
				{
					if (!trade.IsBuyer)
						continue;
					var ethValue = trade.Quantity * trade.Price;
					ethCost += ethValue;
					usdCost += CovertETHtoUSDbyDate(ethValue, trade.Time);
				}

				if (ConfigHelper.AddManualCostForQSP && coin == "QSP")
				{
					usdCost += 100.00m;
				}

				return Tuple.Create(ethCost, usdCost);
			}
			catch
			{
				return Tuple.Create(0m, 0m);
			}
        }

        internal decimal CovertETHtoUSDbyDate(decimal ethAmount, long tradeTime)
        {
            var tradeDateTime = DateTimeHelper.UnixTimeToDateTime(tradeTime / 1000);
            var etherPrice = _etherPriceList.Where(p => p.DateUtcUnix.Date == tradeDateTime.Date).FirstOrDefault();
            if (etherPrice == null)
            {
                tradeDateTime = tradeDateTime.AddDays(-1);
                etherPrice = _etherPriceList.Where(p => p.DateUtcUnix.Date == tradeDateTime.Date).FirstOrDefault();
            }

            if (etherPrice != null)
            {
                return etherPrice.USDValue * ethAmount;
            }

            return 0;
        }
    }
}
