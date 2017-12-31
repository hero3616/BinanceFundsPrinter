using System.Configuration;

public static class ConfigHelper
{
    public static string BinanceApiKey => Config("BinanceApiKey");

    public static string BinanceApiSecret => Config("BinanceApiSecret");

    public static bool LoadTradingRules => bool.Parse(Config("LoadTradingRules"));

    public static bool DisplayUnitUSDValue => bool.Parse(Config("DisplayUnitUSDValue"));

    public static bool DisplayPercentage1h => bool.Parse(Config("DisplayPercentage1h"));

    public static bool DisplayPercentage24h => bool.Parse(Config("DisplayPercentage24h"));

    public static bool DisplayPercentage7d => bool.Parse(Config("DisplayPercentage7d"));

    public static bool DisplayETHCost => bool.Parse(Config("DisplayETHCost"));

    public static bool CalculateUSDCost => bool.Parse(Config("CalculateUSDCost"));

    public static bool DisplayProfit => bool.Parse(Config("DisplayProfit"));

    public static bool DisplayRank => bool.Parse(Config("DisplayRank"));

    public static bool RepeatCoinLastColumn => bool.Parse(Config("RepeatCoinLastColumn"));

    public static string CalculateUSDCostFrom => Config("CalculateUSDCostFrom");

    private static string Config(string val)
    {
        return ConfigurationManager.AppSettings[val];
    }
}