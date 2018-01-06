using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

        public static async Task<IList<EtherPrice>> ReadListFromUrl()
        {
            var list = new List<EtherPrice>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(ConfigHelper.CalculateUSDCostFromTimeout);
                    using (var response = await client.GetAsync(ConfigHelper.CalculateUSDCostFrom))
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        var count = 0;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            count++;
                            if (count > 867)
                            {
                                var etherPrice = EtherPrice.LoadFromCsv(line);
                                list.Add(etherPrice);
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Could not read {0}.", ConfigHelper.CalculateUSDCostFrom);
                Console.ResetColor();
            }

            return list;
        }
    }
}
