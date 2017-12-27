namespace Binance
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiClient = new ApiClient();
            var funds = new Funds(apiClient.Client).Run();
            var printer = new FundsPrinter(funds);
            printer.Print();
        }
    }
}
