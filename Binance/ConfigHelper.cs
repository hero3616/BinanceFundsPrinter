using System.Configuration;

public static class ConfigHelper
{
    public static string BinanceApiKey => Config(nameof(BinanceApiKey));
    public static string BinanceApiSecret => Config(nameof(BinanceApiSecret));
    public static bool LoadTradingRules => bool.Parse(Config(nameof(LoadTradingRules)));

    #region Begin CoinMarketCap API
    public static int CoinMarketCapFetchCount => int.Parse(Config(nameof(CoinMarketCapFetchCount)));
    public static bool DisplayUnitUSDValue = bool.Parse(Config(nameof(DisplayUnitUSDValue)));
    public static bool DisplayPercentage1h = bool.Parse(Config(nameof(DisplayPercentage1h)));
    public static bool DisplayPercentage24h = bool.Parse(Config(nameof(DisplayPercentage24h)));
    public static bool DisplayPercentage7d = bool.Parse(Config(nameof(DisplayPercentage7d)));
    public static bool DisplayRank = bool.Parse(Config(nameof(DisplayRank)));
    public static bool DisplayMarketCap = bool.Parse(Config(nameof(DisplayMarketCap)));
    public static string MarketCapUrl => Config(nameof(MarketCapUrl));
    #endregion

    #region Begin EtherScan.io call
    public static bool CalculateUSDCost => bool.Parse(Config(nameof(CalculateUSDCost)));
    public static string CalculateUSDCostFromFile => Config(nameof(CalculateUSDCostFromFile));
    public static string CalculateUSDCostFromUrl => Config(nameof(CalculateUSDCostFromUrl));
    public static int CalculateUSDCostFromTimeout => int.Parse(Config(nameof(CalculateUSDCostFromTimeout)));
    #endregion

    public static bool DisplayETHCost => bool.Parse(Config(nameof(DisplayETHCost)));
    public static bool DisplayProfit => bool.Parse(Config(nameof(DisplayProfit)));
    public static bool DisplayProfitPercent => bool.Parse(Config(nameof(DisplayProfitPercent)));
    public static bool RepeatCoinLastColumn => bool.Parse(Config(nameof(RepeatCoinLastColumn)));
    public static string OrderDescendingBy => Config(nameof(OrderDescendingBy));
    public static bool AddManualCostForQSP => bool.Parse(Config(nameof(AddManualCostForQSP)));
    public static decimal CalculateETHBalanceUSDValue => decimal.Parse(Config(nameof(CalculateETHBalanceUSDValue)));

    private static string Config(string val)
    {
        return ConfigurationManager.AppSettings[val];
    }
}