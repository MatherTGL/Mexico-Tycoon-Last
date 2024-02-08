using Resources;
using UnityEngine;
using static Building.City.Deliveries.DeliveriesControl;

namespace Building.City.Deliveries
{
    public sealed class GeneralDeliveries : IDeliveriesType
    {
        private TypeDeliveries _typeDeliveries = TypeDeliveries.General;
        TypeDeliveries IDeliveriesType.typeDeliveries => _typeDeliveries;

        private readonly IDeliveriesCurrentCosts _IdeliveriesCurrentCosts;


        public GeneralDeliveries(in IDeliveriesCurrentCosts deliveriesCurrentCosts)
            => _IdeliveriesCurrentCosts = deliveriesCurrentCosts; 

        double IDeliveriesType.GetResourceCost(in TypeProductionResources.TypeResource typeResource)
            => _IdeliveriesCurrentCosts.currentCostsSellResources[(int)typeResource];

        void IDeliveriesType.UpdateTime()
        {
            Debug.Log("General update time");
        }

        void IDeliveriesType.UpdateContract(in DataIndividualDeliveries contractData) => throw new System.NotImplementedException();
    }
}
