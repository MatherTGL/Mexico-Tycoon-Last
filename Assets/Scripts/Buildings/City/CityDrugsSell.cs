using System.Collections.Generic;
using Sirenix.OdinInspector;
using Fabric;
using System;
using Data;


namespace City
{
    public sealed class CityDrugsSell
    {
        [ShowInInspector]
        private Dictionary<string, double> d_costDrugs;


        public CityDrugsSell(in uint[] costDrugKg)
        {
            d_costDrugs = new Dictionary<string, double>();

            var typeDrug = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

            for (int i = 0; i < typeDrug.Length; i++)
            {
                d_costDrugs.Add(typeDrug[i], costDrugKg[i]);
            }
        }

        public void Sell(string typeFabricDrug, float weightSell)
        {
            var addMoneySum = d_costDrugs[typeFabricDrug] * weightSell;
            DataControl.IdataPlayer.AddPlayerMoney(addMoneySum);
        }
    }
}