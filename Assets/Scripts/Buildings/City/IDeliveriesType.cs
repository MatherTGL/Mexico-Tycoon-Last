using Resources;
using static Building.City.Deliveries.DeliveriesControl;

namespace Building.City.Deliveries
{
    public interface IDeliveriesType
    {
        TypeDeliveries typeDeliveries { get; }


        double GetResourceCost(in TypeProductionResources.TypeResource typeResource);

        void UpdateTime();

        void UpdateContract(in DataIndividualDeliveries contractData);
    }
}
