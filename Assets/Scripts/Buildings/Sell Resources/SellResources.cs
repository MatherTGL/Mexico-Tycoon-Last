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
            amountResources[TypeProductionResources.TypeResource.CocaLeaves] += 2000;

            for (int i = _Ideliveries.l_deliveriesType.Count - 1; i >= 0; i--)
            {
                foreach (var drug in amountResources.Keys.ToArray())
                {
                    Debug.Log($"IIIIIIII: {i} / {_Ideliveries.l_deliveriesType[i].typeDeliveries}");
                    if (_Ideliveries.l_deliveriesType[i].typeDeliveries == Deliveries.TypeDeliveries.Individual)
                        IndividualSell(ref amountResources, i, drug);
                    else
                        GeneralSell(ref amountResources, drug);
                }
            }

            amountResources[TypeProductionResources.TypeResource.DirtyMoney] += _salesProfit;
            Debug.Log($"salllllo: {_salesProfit}");
            Debug.Log($"DirtyMoney: {amountResources[TypeProductionResources.TypeResource.DirtyMoney]}");
            _salesProfit = 0;
        }

        private void IndividualSell(ref Dictionary<TypeProductionResources.TypeResource, double> amountResources, in int indexDeliveries,
                                    in TypeProductionResources.TypeResource drug)
        {
            if (drug == TypeProductionResources.TypeResource.DirtyMoney)
                return;

            _IindividualDeliveries = _Ideliveries.l_deliveriesType[indexDeliveries] as IIndividualDeliveries;

            if (_IindividualDeliveries.GetResourceBeingSent() != drug || _IindividualDeliveries.IsContractIsFinalized())
                return;

            Debug.Log($"In individual block and amount res: {amountResources[drug]}");

            if (amountResources[drug] >= _IindividualDeliveries.GetDailyAllowanceKg())
            {
                _salesProfit += _IindividualDeliveries.GetDailyAllowanceKg() * _Ideliveries.GetResourceCosts(drug);
                Debug.Log($"IndividualDeliveries contract sale 1: {_salesProfit} - {_IindividualDeliveries.GetDailyAllowanceKg() * _Ideliveries.GetResourceCosts(drug)}");
                Debug.Log(_salesProfit);
                amountResources[drug] -= _IindividualDeliveries.GetDailyAllowanceKg();

                if (amountResources[drug] > 0)
                {
                    _salesProfit += amountResources[drug] * _Ideliveries.GetResourceCosts(drug);
                    Debug.Log($"BLOOOOOH {amountResources[drug]} / {_salesProfit} / {amountResources[drug] * _Ideliveries.GetResourceCosts(drug)}");
                    amountResources[drug] = 0;
                }
            }
            else
            {
                var remainingResource = amountResources[drug];
                _salesProfit += remainingResource * _Ideliveries.GetResourceCosts(drug);
                Debug.Log($"IndividualDeliveries contract sale 2: {_salesProfit} - {remainingResource * _Ideliveries.GetResourceCosts(drug)}");
                amountResources[drug] = 0;
            }
            Debug.Log($"All resources in city {drug} - {amountResources[drug]}");
            Debug.Log($"individualDeliveries : {_IindividualDeliveries}");
        }

        private void GeneralSell(ref Dictionary<TypeProductionResources.TypeResource, double> amountResources, in TypeProductionResources.TypeResource drug)
        {
            _salesProfit += amountResources[drug] * _Ideliveries.GetResourceCosts(drug);
            Debug.Log($"GeneralDeliveries {drug} amount {amountResources[drug]}");
            Debug.Log($"GeneralDeliveries contract sale: {_salesProfit} - {amountResources[drug] * _Ideliveries.GetResourceCosts(drug)}");
            amountResources[drug] = 0;
            Debug.Log($"All resources in city {drug} - {amountResources[drug]}");
        }
    }
}
