using Resources;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Building.City.Deliveries.Deliveries;

namespace Building.City.Deliveries
{
    public sealed class GeneralDeliveries : IDeliveriesType
    {
        private TypeDeliveries _typeDeliveries = TypeDeliveries.General;
        TypeDeliveries IDeliveriesType.typeDeliveries => _typeDeliveries;


        double IDeliveriesType.GetResourceCost(in TypeProductionResources.TypeResource typeResource, in CostResourcesConfig costResourcesConfig)
        {
            Debug.Log($"Cost sell res: {costResourcesConfig.GetCostsSellResources()[(int)typeResource]}");
            return costResourcesConfig.GetCostsSellResources()[(int)typeResource];
        }

        void IDeliveriesType.UpdateTime()
        {
            Debug.Log("General update time");
        }
    }
}
