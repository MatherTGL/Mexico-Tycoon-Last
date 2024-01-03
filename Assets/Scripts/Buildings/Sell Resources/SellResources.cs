using Building.City.Deliveries;
using Config.Building;
using Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Building.Additional
{
    public sealed class SellResources : ISellResources
    {
        private IDeliveries _Ideliveries;

        private IIndividualDeliveries _IindividualDeliveries;

        private double _salesProfit = 0;


        public SellResources(in IDeliveries Ideliveries, in ConfigBuildingCityEditor configCity)
        {
            _Ideliveries = Ideliveries;
            _Ideliveries.Init(configCity.configDeliveries, configCity.costResourcesConfig);
        }

        void ISellResources.Sell(ref Dictionary<TypeProductionResources.TypeResource, double> amountResources)
        {
            for (int i = _Ideliveries.l_deliveriesType.Count - 1; i >= 0; i--)
            {
                foreach (var drug in amountResources.Keys.ToArray())
                {
                    if (_Ideliveries.l_deliveriesType[i].typeDeliveries == Deliveries.TypeDeliveries.Individual)
                        IndividualSell(ref amountResources, i, drug);

                    _salesProfit += amountResources[drug] * _Ideliveries.GetResourceCosts(drug);
                    amountResources[drug] = 0;
                }
            }

            amountResources[TypeProductionResources.TypeResource.DirtyMoney] += _salesProfit;
            _salesProfit = 0;
            Debug.Log($"DirtyMoney: {amountResources[TypeProductionResources.TypeResource.DirtyMoney]}");
        }

        private void IndividualSell(ref Dictionary<TypeProductionResources.TypeResource, double> amountResources, in int indexDeliveries,
                                    in TypeProductionResources.TypeResource drug)
        {
            if (drug == TypeProductionResources.TypeResource.DirtyMoney)
                return;

            _IindividualDeliveries = _Ideliveries.l_deliveriesType[indexDeliveries] as IIndividualDeliveries;

            if (_IindividualDeliveries.GetResourceBeingSent() != drug || _IindividualDeliveries.IsContractIsFinalized())
                return;

            if (amountResources[drug] >= _IindividualDeliveries.GetDailyAllowanceKg())
            {
                _salesProfit += _IindividualDeliveries.GetDailyAllowanceKg() * _Ideliveries.GetResourceCosts(drug);
                amountResources[drug] -= _IindividualDeliveries.GetDailyAllowanceKg();
            }
        }
    }
}
