namespace Binance
{
    public class Coin
    {
        public string Abbreviation { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal BTCValue { get; set; }
        public decimal UnitUSDValue { get; set; }
        public decimal USDValue { get; set; }
        public decimal ETHCost { get; set; }
        public decimal USDCost { get; set; }
        public string Percentage1h { get; set; }
        public string Percentage24h { get; set; }
        public string Percentage7d { get; set; }
        public int Rank { get; set; }
    }
}
