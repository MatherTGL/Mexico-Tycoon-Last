using Building.Additional;
using Building.City.Business;
using Building.City.Deliveries;
using Config.Building;
using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building.City
{
    public sealed class BuildingCity : AbstractBuilding, IBuilding, ICityBusiness
    {
        //! private IDeliveries _Ideliveries; -> move to SellResources class
        
        private ISellResources _IsellResources;

        private readonly CityPopulationReproduction _cityPopulationReproduction;

        private readonly CityBusiness _cityBusiness;

        private readonly ConfigBuildingCityEditor _config;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        private event Action _updatedTimeStep;

        event Action ICityBusiness.updatedTimeStep
        {
            add => _updatedTimeStep += value;
            remove => _updatedTimeStep -= value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private uint _population;


        public BuildingCity(in ScriptableObject config, in IDeliveries deliveries)
        {
            _config = config as ConfigBuildingCityEditor;
            _cityPopulationReproduction = new(_config);
            _cityBusiness = new(this);
            _IsellResources = new SellResources(deliveries, _config);

            _population = (uint)UnityEngine.Random.Range(
                _config.populationStartMin, _config.populationStartMax);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            _updatedTimeStep?.Invoke();
            _cityPopulationReproduction.PopulationReproduction(ref _population);

            SellResources();
        }

        private void SellResources()
        {
            _IsellResources.Sell(ref d_amountResources);
            ToLaunderMoney();
        }

        private void ToLaunderMoney()
        {
            _cityBusiness.ToLaunderMoney(d_amountResources[TypeProductionResources.TypeResource.DirtyMoney]);
            d_amountResources[TypeProductionResources.TypeResource.DirtyMoney] = 0;
        }

        void ICityBusiness.BuyBusiness(in CityBusiness.TypeBusiness typeBusiness)
            => _cityBusiness.BuyBusiness(typeBusiness);

        void ICityBusiness.SellBusiness(in ushort indexBusiness)
            => _cityBusiness.SellBusiness(indexBusiness);
    }
}
