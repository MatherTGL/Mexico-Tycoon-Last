using Building.City.Business;
using Building.City.Deliveries;
using Config.Building;
using Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Building.City
{
    public sealed class BuildingCity : AbstractBuilding, IBuilding, IUseBusiness, IPotentialConsumers
    {
        private readonly CityPopulationReproduction _cityPopulationReproduction;

        private readonly CityBusiness _cityBusiness;

        private readonly ConfigBuildingCityEditor _config;

        private readonly ILocalMarket _IlocalMarket;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private event Action _updatedTimeStep;

        event Action IUseBusiness.updatedTimeStep
        {
            add => _updatedTimeStep += value;
            remove => _updatedTimeStep -= value;
        }

        private uint _population;


        public BuildingCity(in ScriptableObject config, in ILocalMarket localMarket)
        {
            _config = config as ConfigBuildingCityEditor;
            _cityPopulationReproduction = new(_config);
            _cityBusiness = new(this);

            _population = (uint)UnityEngine.Random.Range(
                _config.populationStartMin, _config.populationStartMax);

            _IlocalMarket = localMarket;
            _IlocalMarket.Init(_config.costResourcesConfig, this);
        }

        //! refactoring (Move to special script) https://ru.yougile.com/team/bf00efa6ea26/#MEX-89
        private void ToLaunderMoney()
        {
            _cityBusiness.ToLaunderMoney(d_amountResources[TypeProductionResources.TypeResource.DirtyMoney]);
            d_amountResources[TypeProductionResources.TypeResource.DirtyMoney] = 0;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            _cityPopulationReproduction.PopulationReproduction(ref _population);
            _updatedTimeStep?.Invoke();
            ToLaunderMoney();
        }

        //! refactoring (Move to special script) https://ru.yougile.com/team/bf00efa6ea26/#MEX-90
        void IUseBusiness.BuyBusiness(in CityBusiness.TypeBusiness typeBusiness)
            => _cityBusiness.BuyBusiness(typeBusiness);

        void IUseBusiness.SellBusiness(in ushort indexBusiness)
            => _cityBusiness.SellBusiness(indexBusiness);

        uint IPotentialConsumers.GetCount() => _population;
    }
}
