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

        private float _percentageMoneyCleared;


        public void BuyBusiness(in TypeBusiness typeBusiness)
        {
            if (typeBusiness == TypeBusiness.CarWash)
                l_purchasedBusinesses.Add(new CarWashBusiness());

            LoadDataFromConfig(typeBusiness);
        }

        private void LoadDataFromConfig(TypeBusiness typeBusiness)
        {
            _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigCityBusinessEditor>()
                        .ToArray().Where(config => config.typeBusiness.Equals(typeBusiness)).Single();

            if (_config != null)
                _percentageMoneyCleared = _config.percentageMoneyCleared;
        }

        public void SellBusiness(in ushort indexBusiness)
        {
            if (l_purchasedBusinesses.IsNotEmpty(indexBusiness))
                l_purchasedBusinesses.RemoveAt(indexBusiness);
        }

        public void ToLaunderMoney(in double amountDirtyMoney)
        {
            for (byte i = 0; i < l_purchasedBusinesses.Count; i++)
                l_purchasedBusinesses[i].ToLaunderMoney(amountDirtyMoney, _percentageMoneyCleared);
        }
    }
}
