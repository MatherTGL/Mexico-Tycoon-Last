using System.Collections.Generic;
using System.Linq;
using Business;
using Config.Building.Business;
using UnityEngine;

namespace Building.City.Business
{
    public class CityBusiness
    {
        private ConfigCityBusinessEditor _config;

        private List<IBusiness> l_purchasedBusinesses = new();

        public enum TypeBusiness : byte
        {
            CarWash
        }

        protected double _clearedMoney;

        protected float _percentageMoneyCleared;


        public void BuyBusiness(in TypeBusiness typeBusiness)
        {
            LoadDataFromConfig(typeBusiness);

            if (typeBusiness == TypeBusiness.CarWash)
                l_purchasedBusinesses.Add(new CarWashBusiness());

            Debug.Log("Business buyed");
        }

        private void LoadDataFromConfig(TypeBusiness typeBusiness)
        {
            _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigCityBusinessEditor>()
                                            .Where(config => config.typeBusiness == typeBusiness)
                                            .ToArray().Single();
            Debug.Log(_config);
            Debug.Log(_config.percentageMoneyCleared);

            if (_config != null)
                _percentageMoneyCleared = _config.percentageMoneyCleared;
            Debug.Log(_percentageMoneyCleared);
        }

        public void SellBusiness(in ushort indexBusiness)
        {
            if (l_purchasedBusinesses.IsNotEmpty(indexBusiness))
                l_purchasedBusinesses.RemoveAt(indexBusiness);
            Debug.Log("Business selled");
        }

        public void ToLaunderMoney(in double amountDirtyMoney)
        {
            Debug.Log("Launder money CityBusiness enter");
            for (byte i = 0; i < l_purchasedBusinesses.Count; i++)
            {
                Debug.Log("for launder money CityBusiness");
                l_purchasedBusinesses[i].ToLaunderMoney(amountDirtyMoney);
                Debug.Log("for launder money CityBusiness end");
                // !!!!!!
            }
        }
    }
}
