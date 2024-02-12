using System.Collections.Generic;
using Business;
using System.Linq;
using Config.Building.Business;
using UnityEngine;

namespace Building.City.Business
{
    public sealed class CityBusiness
    {
        private const string pathToConfigs = "Configs/Buildings/City/Business";

        private ConfigCityBusinessEditor _config;

        private readonly List<IBusiness> l_purchasedBusinesses = new();

        public enum TypeBusiness : byte { CarWash }

        private double _costSell;


        public CityBusiness(in IUseBusiness IcityBusiness)
            => IcityBusiness.updatedTimeStep += ConstantUpdatingInfo;

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
            try
            {
                _config = UnityEngine.Resources.LoadAll<ConfigCityBusinessEditor>(pathToConfigs)
                           .Where(config => config.typeBusiness == typeBusiness)?.ElementAt(0);
                Debug.Log($"config loadeed: {_config.name}");
            }
            catch (System.Exception) { throw new System.Exception(); }

            InitAdditionalParameters();
        }

        private void InitAdditionalParameters()
            => _costSell = _config.costPurchase / 2;

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

        private void ConstantUpdatingInfo() => MaintenanceConsumption();

        private void MaintenanceConsumption()
        {
            for (byte i = 0; i < l_purchasedBusinesses.Count; i++)
                l_purchasedBusinesses[i].MaintenanceConsumption(_config);
        }
    }
}
