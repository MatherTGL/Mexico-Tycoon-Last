using Config.City.Business;


namespace City.Business
{
    public interface IBuisinessBuilding
    {
        bool isWork { get; set; }
        double income { get; }
        double consumption { get; }
        byte occupiedNumberSlots { get; }
        double amountMoneyLaunderedPerYear { get; }
        double maxAmountMoneyLaundered { get; }
        float percentageMoneyLaundered { get; }
        ushort numberVisitors { get; }
        ushort averageCheckVisitor { get; }
        ushort averageSpendPerVisitor { get; }
        float institutionPopularity { get; }
        ushort maxNumberVisitors { get; }
        CityTreasury cityTreasury { get; }
        CityControl cityControl { get; }
        BusinessDataSO businessDataSO { get; }

        void WorkBusiness();
        byte GetOccupiedNumberSlots();
        void ChangePercentageMoneyLaundered(float setPercentage);
        float GetPercentageMoneyLaundered();
        void SetInstitutionPopularity(float percentPopularity);
        void UpgradeMaxNumberVisitors();
    }
}
