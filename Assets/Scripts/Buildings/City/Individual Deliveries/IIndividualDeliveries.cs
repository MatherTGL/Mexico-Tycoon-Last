using Resources;

namespace Building.City.Deliveries
{
    public interface IIndividualDeliveries
    {
        bool IsContractIsFinalized();

        TypeProductionResources.TypeResource GetResourceBeingSent();

        double GetDailyAllowanceKg();

        void UpdateContract(in DataIndividualDeliveries contractData);
    }
}
