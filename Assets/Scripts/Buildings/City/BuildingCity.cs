using Building.City.Business;
using Config.Building;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Building.City
{
    public sealed class BuildingCity : IBuilding, ICityBusiness
    {
        private readonly CityPopulationReproduction _cityPopulationReproduction;

        private readonly CityBusiness _cityBusiness;

        private readonly ConfigBuildingCityEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, double> d_amountResources = new();

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new();

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

        private uint[] _costPerKg;

        private uint _population;


        public BuildingCity(in ScriptableObject config)
        {
            _config = config as ConfigBuildingCityEditor;
            _cityPopulationReproduction = new(_config);
            _cityBusiness = new(this);

            InitArrays(_config);
            _population = (uint)UnityEngine.Random.Range(
                _config.populationStartMin, _config.populationStartMax);
        }

        private void InitArrays(in ConfigBuildingCityEditor configBuilding)
        {
            int amountTypeDrugs = Enum.GetValues(typeof(TypeProductionResources.TypeResource)).Length;

            _costPerKg = new uint[amountTypeDrugs];
            _costPerKg = configBuilding.costResourcesConfig.GetCostsSellResources();
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            _updatedTimeStep?.Invoke();
            _cityPopulationReproduction.PopulationReproduction(ref _population);
            SellResources();
        }

        private void SellResources()
        {
            foreach (var drug in d_amountResources.Keys.ToArray())
            {
                if (drug != TypeProductionResources.TypeResource.DirtyMoney)
                {
                    double salesProfit = d_amountResources[drug] * _costPerKg[(int)drug];
                    d_amountResources[TypeProductionResources.TypeResource.DirtyMoney] += salesProfit;
                    d_amountResources[drug] = 0;
                }
            }
            ToLaunderMoney();
        }

        private void ToLaunderMoney()
        {
            _cityBusiness.ToLaunderMoney(d_amountResources[TypeProductionResources.TypeResource.DirtyMoney]);
            d_amountResources[TypeProductionResources.TypeResource.DirtyMoney] = 0;
        }

        void ICityBusiness.BuyBusiness(in CityBusiness.TypeBusiness typeBusiness)
        {
            _cityBusiness.BuyBusiness(typeBusiness);
        }

        void ICityBusiness.SellBusiness(in ushort indexBusiness)
        {
            _cityBusiness.SellBusiness(indexBusiness);
        }
    }
}
