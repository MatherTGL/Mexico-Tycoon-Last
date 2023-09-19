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
            if (typeBusiness == TypeBusiness.CarWash)
            {
                LoadDataFromConfig(typeBusiness);
                l_purchasedBusinesses.Add(new CarWashBusiness());
            }
            Debug.Log("Business buyed");
        }

        private void LoadDataFromConfig(TypeBusiness typeBusiness)
        {
            _config = UnityEngine.Resources.FindObjectsOfTypeAll<ConfigCityBusinessEditor>()
                                              .Where(config => config.typeBusiness.Equals(typeBusiness))
                                              .Single();

            _percentageMoneyCleared = _config.percentageMoneyCleared;
            Debug.Log("Config loaded");
        }

        public void SellBusiness(in ushort indexBusiness)
        {
            if (l_purchasedBusinesses.IsNotEmpty(indexBusiness))
                l_purchasedBusinesses.RemoveAt(indexBusiness);
            Debug.Log("Business selled");
        }

        public void ToLaunderMoney(in double amountDirtyMoney)
        {
            for (byte i = 0; i < l_purchasedBusinesses.Count; i++)
                l_purchasedBusinesses[i].ToLaunderMoney(amountDirtyMoney);
        }
    }
}
