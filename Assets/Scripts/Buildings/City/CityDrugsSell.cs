using Data;
using UnityEngine;


namespace City
{
    public sealed class CityDrugsSell
    {
        public void Sell(float weightSell, in ICityDrugBuyers IcityDrugBuyers, in string nameBuyer, in string typeDrug)
        {
            var addMoneySum = IcityDrugBuyers.d_contractBuyers[nameBuyer].d_drugCost[typeDrug] * weightSell;
            Debug.Log(IcityDrugBuyers.d_contractBuyers[nameBuyer].d_drugCost);
            DataControl.IdataPlayer.AddPlayerMoney(addMoneySum);
        }
    }
}
