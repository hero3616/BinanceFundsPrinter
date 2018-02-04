using System;

namespace Binance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Please wait...");
            Console.ResetColor();

            try
            {
                var apiClient = new ApiClient();
                var funds = new Funds(apiClient.Client).Run();
                var printer = new FundsPrinter(funds);
                printer.Print();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += " " + ex.InnerException.Message;
                Console.WriteLine("Error: {0}", msg);
                Console.ResetColor();
                Environment.Exit(-1);
            }
        }
    }
}
