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
        public static bool ApiProblemDetected = false;

        public static IList<Currency> GetCurrencyList()
        {
            if (ConfigHelper.DisplayUnitUSDValue ||
                ConfigHelper.DisplayPercentage1h ||
                ConfigHelper.DisplayPercentage24h ||
                ConfigHelper.DisplayPercentage7d ||
                ConfigHelper.DisplayRank ||
                ConfigHelper.DisplayMarketCap
               )
            {
                try
                {
                    ICoinmarketcapClient client = new CoinmarketcapClient();
                    return client.GetCurrencies(ConfigHelper.CoinMarketCapFetchCount).ToList();
                }
                catch
                {
                    ApiProblemDetected = true;
                    ConfigHelper.DisplayUnitUSDValue = false;
                    ConfigHelper.DisplayPercentage1h = false;
                    ConfigHelper.DisplayPercentage24h = false;
                    ConfigHelper.DisplayPercentage7d = false;
                    ConfigHelper.DisplayRank = false;
                    ConfigHelper.DisplayMarketCap = false;
                }
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
