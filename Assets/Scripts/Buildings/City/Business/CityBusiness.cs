using System.Collections.Generic;
using Business;
using System.Linq;
using Config.Building.Business;
using UnityEngine;

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
        }

        public void SellBusiness(in ushort indexBusiness)
        {
            if (l_purchasedBusinesses.IsNotEmpty(indexBusiness))
                l_purchasedBusinesses.RemoveAt(indexBusiness);
        }

        public void ToLaunderMoney(in double amountDirtyMoney)
        {
            for (byte i = 0; i < l_purchasedBusinesses.Count; i++)
                l_purchasedBusinesses[i].ToLaunderMoney(amountDirtyMoney, _config);
        }
    }
}
