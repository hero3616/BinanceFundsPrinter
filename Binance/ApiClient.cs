using bin = Binance.API.Csharp.Client;

namespace Binance
{
    public class ApiClient
    {
        public bin.BinanceClient Client { get; }

        public ApiClient()
        {
            var apiClient = new bin.ApiClient(ConfigHelper.BinanceApiKey, ConfigHelper.BinanceApiSecret);
            Client = new bin.BinanceClient(apiClient, ConfigHelper.LoadTradingRules);
        }
    }
}
