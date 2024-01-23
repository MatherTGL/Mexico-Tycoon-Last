using Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Building.City.Deliveries
{
    public sealed class Deliveries : MonoBehaviour, IDeliveries, IDeliveriesCurrentCosts, IContract
    {
        private List<IDeliveriesType> l_deliveriesType = new();
        List<IDeliveriesType> IDeliveries.l_deliveriesType => l_deliveriesType;
        List<IDeliveriesType> IContract.l_deliveriesType => l_deliveriesType;

        private uint[] _currentCostsSellResources;
        uint[] IDeliveriesCurrentCosts.currentCostsSellResources => _currentCostsSellResources;

        private uint[] _currentCostBuyResources;
        uint[] IDeliveriesCurrentCosts.currentCostBuyResources => _currentCostBuyResources;

        public enum TypeDeliveries : byte { General, Individual }


        void IContract.Init(in CostResourcesConfig costResourcesConfig)
        {
            _currentCostsSellResources = costResourcesConfig.GetCostsSellResources();
            _currentCostBuyResources = costResourcesConfig.GetCostsBuyResources();

            l_deliveriesType.Add(new GeneralDeliveries(this));
        }

        void IDeliveries.AddNewContract(in IDeliveriesType deliveriesType)
        {
            if (deliveriesType == null)
                return;

            l_deliveriesType.Add(deliveriesType);
        }

        double IContract.GetResourceCosts(in TypeProductionResources.TypeResource typeResource)
        {
            return l_deliveriesType[l_deliveriesType.Count - 1].GetResourceCost(typeResource);
        }
    }
}
