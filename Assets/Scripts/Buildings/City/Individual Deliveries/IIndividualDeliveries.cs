using Resources;

namespace Building.City.Deliveries
{
    public interface IIndividualDeliveries
    {
        void SignedContract();

        bool IsSignedContract();

        bool IsContractIsFinalized();

        TypeProductionResources.TypeResource GetResourceBeingSent();

        double GetDailyAllowanceKg();
    }
}
