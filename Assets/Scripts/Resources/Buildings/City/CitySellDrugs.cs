using Data;


namespace Building.City
{
    public sealed class CitySellDrugs
    {
        public void Sell(float amountDrug, in uint costSellPerKg)
        {
            double salesProfit = amountDrug * costSellPerKg;
            DataControl.IdataPlayer.AddPlayerMoney(salesProfit);
        }
    }
}
