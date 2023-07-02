using Data;
using UnityEngine;


namespace City
{
    public sealed class CityDrugsSell
    {
        public void Sell(float weightSell, in ICityDrugBuyers IcityDrugBuyers)
        {
            foreach (var buyers in IcityDrugBuyers.d_contractContactAndDrug.Keys)
            {
                //var addMoneySum = IcityDrugBuyers.d_contractDrugCityBuyers[buyers] * weightSell;
                Debug.Log(IcityDrugBuyers.d_contractContactAndDrug[buyers]);
                //DataControl.IdataPlayer.AddPlayerMoney(addMoneySum);
            }
        }
    }
}
