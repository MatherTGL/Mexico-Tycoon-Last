using Config.Building;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Building.City
{
    public sealed class BuildingCity : IBuilding
    {
        private CityPopulationReproduction _cityPopulationReproduction;

        private CitySellDrugs _citySellDrugs = new CitySellDrugs();

        private ConfigBuildingCityEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new Dictionary<TypeProductionResources.TypeResource, uint>();

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private uint[] _costPerKg;

        private uint _population;

        private byte _indexSelectedDrugType;


        public BuildingCity(in ScriptableObject config)
        {
            _config = (ConfigBuildingCityEditor)config;

            _cityPopulationReproduction = new(_config);

            InitArrays(_config);

            _population = (uint)UnityEngine.Random.Range(
                _config.populationStartMin, _config.populationStartMax);
        }

        private void SellResources()
        {
            foreach (var drug in d_amountResources.Keys.ToArray())
            {
                _citySellDrugs.Sell(d_amountResources[drug], _costPerKg[(int)drug]);
                d_amountResources[drug] = 0;
            }
        }

        private void InitArrays(in ConfigBuildingCityEditor configBuilding)
        {
            int amountTypeDrugs = Enum.GetValues(typeof(TypeProductionResources.TypeResource)).Length;

            _costPerKg = new uint[amountTypeDrugs];
            _costPerKg = configBuilding.costResourcesConfig.GetCostsSellResources();
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            _cityPopulationReproduction.PopulationReproduction(ref _population);
            SellResources();
        }
    }
}
