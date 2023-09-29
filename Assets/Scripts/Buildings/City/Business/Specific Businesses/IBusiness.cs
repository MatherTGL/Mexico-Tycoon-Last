using Config.Building.Business;

namespace Business
{
    public interface IBusiness
    {
        bool BuyBusiness(in ConfigCityBusinessEditor config);

        void SellBusiness(in double costSell);

        void ToLaunderMoney(in double amountDirtyMoney, in ConfigCityBusinessEditor config);

        void MaintenanceConsumption(in ConfigCityBusinessEditor config);
    }
}
