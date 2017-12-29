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

    public static bool DisplayPercentage1h
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["DisplayPercentage1h"]);
        }
    }

    public static bool DisplayPercentage24h
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["DisplayPercentage24h"]);
        }
    }

    public static bool DisplayPercentage7d
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["DisplayPercentage7d"]);
        }
    }

    public static bool DisplayETHCost
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["DisplayETHCost"]);
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