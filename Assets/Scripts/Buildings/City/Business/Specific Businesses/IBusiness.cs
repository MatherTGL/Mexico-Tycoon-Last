using Config.Building.Business;

namespace Business
{
    public interface IBusiness
    {
        bool BuyBusiness(in ConfigCityBusinessEditor config);

        void SellBusiness();

        void ToLaunderMoney(in double amountDirtyMoney, in ConfigCityBusinessEditor config);
    }
}
