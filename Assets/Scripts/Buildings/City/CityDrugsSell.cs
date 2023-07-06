using Data;
using UnityEngine;


namespace City
{
    public sealed class CityDrugsSell : ICityDrugSell
    {
        public void Sell(float weightSell, in string contractBuyers, in string typeFabricDrug, in ICityControlSell IcityControlSell)
        {
            Debug.Log(IcityControlSell);
            if (IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].isCooperation is true
                    && IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].l_drugName.Contains(typeFabricDrug))
            {
                if (IcityControlSell.d_amountDrugsInCity[typeFabricDrug] >= IcityControlSell.d_weightToSellDrugs[typeFabricDrug])
                {
                    if (IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugDemand[typeFabricDrug] >= IcityControlSell.d_weightToSellDrugs[typeFabricDrug])
                    {
                        IcityControlSell.d_amountDrugsInCity[typeFabricDrug] -= IcityControlSell.d_weightToSellDrugs[typeFabricDrug];

                        var clampDemand = IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugDemand[typeFabricDrug] + IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugIncreasedDemand[typeFabricDrug];
                        IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugDemand[typeFabricDrug] = Mathf.Clamp(clampDemand,
                                                                                             IcityControlSell.buyersDrugsClampDemandMin, IcityControlSell.buyersDrugsClampDemandMax);

                        IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugDemand[typeFabricDrug] -= IcityControlSell.d_weightToSellDrugs[typeFabricDrug]
                        + IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugIncreasedDemand[typeFabricDrug];
                    }
                    else
                        IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugDemand[typeFabricDrug] += IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugIncreasedDemand[typeFabricDrug];

                    var addMoneySum = IcityControlSell._IcityDrugBuyers.d_contractBuyers[contractBuyers].d_drugCost[typeFabricDrug] * weightSell;
                    DataControl.IdataPlayer.AddPlayerMoney(addMoneySum);
                    Debug.Log(addMoneySum);
                }
            }
        }
    }
}
