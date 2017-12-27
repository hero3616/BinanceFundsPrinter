using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Binance
{
    public class EtherPrice
    {
        public DateTime DateUtcUnix { get; set; }
        public decimal USDValue { get; set; }

        public static EtherPrice LoadFromCsv(string csvLine)
        {
            var o = new EtherPrice();
            var values = csvLine.Split(',').Select(s => s.Replace("\"", "")).ToList();
            o.DateUtcUnix = DateTimeHelper.UnixTimeToDateTime(double.Parse(values[1]));
            o.USDValue = decimal.Parse((values[2].Replace("\"", "")));
            return o;
        }

        public static IList<EtherPrice> ReadList()
        {
            if (File.Exists(ConfigHelper.EtherscanFile))
            {
                return File.ReadAllLines(ConfigHelper.EtherscanFile)
                           .Skip(867)
                           .Select(v => EtherPrice.LoadFromCsv(v))
                           .ToList();
            }

            return null;
        }
    }
}
