using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoobsMuc.Coinmarketcap.Client;

namespace Binance
{
    public class Coinmarketcap
    {
        public static IList<Currency> GetCurrencyList()
        {
            if (ConfigHelper.DisplayPercentage1h ||
                ConfigHelper.DisplayPercentage24h ||
                ConfigHelper.DisplayPercentage7d ||
                ConfigHelper.DisplayUnitUSDValue ||
                ConfigHelper.DisplayRank ||
                ConfigHelper.DisplayMarketCap
               )
            {
                ICoinmarketcapClient client = new CoinmarketcapClient();
                return client.GetCurrencies(ConfigHelper.CoinMarketCapFetchCount).ToList();
            }

            return null;
        }

        public static async Task<string> GetMarketCap()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(ConfigHelper.CalculateUSDCostFromTimeout);
            var response = await client.GetAsync(ConfigHelper.MarketCapUrl);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
            return result.total_market_cap_usd;
        }
    }
}
