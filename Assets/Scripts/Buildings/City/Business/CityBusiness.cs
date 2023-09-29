using System.Collections.Generic;
using Business;
using System.Linq;
using Config.Building.Business;

namespace Building.City.Business
{
    public sealed class CityBusiness
    {
        private ConfigCityBusinessEditor _config;

        private List<IBusiness> l_purchasedBusinesses = new();

        public enum TypeBusiness : byte
        {
            CarWash
        }

        private double _costSell;


        public void BuyBusiness(in TypeBusiness typeBusiness)
        {
            LoadDataFromConfig(typeBusiness);
            IBusiness business = null;

            if (typeBusiness == TypeBusiness.CarWash)
                business = new CarWashBusiness();

            if (business != null && business.BuyBusiness(_config))
                l_purchasedBusinesses.Add(business);
        }

        private void LoadDataFromConfig(TypeBusiness typeBusiness)
        {
            _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigCityBusinessEditor>()
                        .Where(config => config.typeBusiness == typeBusiness)?.ElementAt(0);
            InitAdditionalParameters();
        }

        private void InitAdditionalParameters()
        {
            _costSell = _config.costPurchase / 2;
        }

        public void SellBusiness(in ushort indexBusiness)
        {
            if (l_purchasedBusinesses.IsNotEmpty(indexBusiness))
            {
                l_purchasedBusinesses.ElementAt(indexBusiness).SellBusiness(_costSell);
                l_purchasedBusinesses.RemoveAt(indexBusiness);
            }
        }

        public void ToLaunderMoney(in double amountDirtyMoney)
        {
            for (byte i = 0; i < l_purchasedBusinesses.Count; i++)
                l_purchasedBusinesses[i].ToLaunderMoney(amountDirtyMoney, _config);
        }
    }
}
