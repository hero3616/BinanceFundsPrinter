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

			var apiClient = new ApiClient();
            var funds = new Funds(apiClient.Client).Run();
            var printer = new FundsPrinter(funds);
            printer.Print();
        }
    }
}
