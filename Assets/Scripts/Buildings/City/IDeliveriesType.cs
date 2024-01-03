using Resources;
using static Building.City.Deliveries.Deliveries;

namespace Building.City.Deliveries
{
    public interface IDeliveriesType
    {
        TypeDeliveries typeDeliveries { get; }


        double GetResourceCost(in TypeProductionResources.TypeResource typeResource, in CostResourcesConfig costResourcesConfig);

        void UpdateTime();
    }
}
