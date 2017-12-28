using System.Configuration;

public static class ConfigHelper
{
    public static string BinanceApiKey
    {
        get
        {
            return ConfigurationManager.AppSettings["BinanceApiKey"];
        }
    }

    public static string BinanceApiSecret
    {
        get
        {
            return ConfigurationManager.AppSettings["BinanceApiSecret"];
        }
    }

    public static bool LoadTradingRules
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["LoadTradingRules"]);
        }
    }

    public static bool IncludePercentChange
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["IncludePercentChange"]);
        }
    }

    public static bool CalculateUSDCost
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["CalculateUSDCost"]);
        }
    }

	public static string CalculateUSDCostFrom
	{
		get
		{
			return ConfigurationManager.AppSettings["CalculateUSDCostFrom"];
		}
	}
}