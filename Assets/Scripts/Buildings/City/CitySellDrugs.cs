using UnityEngine;


namespace Building.City
{
    public sealed class CitySellDrugs
    {
        public void Sell(ref float amountDrug, in uint costSellPerKg)
        {
            double salesProfit = amountDrug * costSellPerKg;
            amountDrug = 0;
            Debug.Log(salesProfit);
            //DataControl.IdataPlayer.AddPlayerMoney(salesProfit);
        }
    }
}
