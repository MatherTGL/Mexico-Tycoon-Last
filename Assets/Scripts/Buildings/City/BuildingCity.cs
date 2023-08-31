using UnityEngine;
using Config.Building;
using System.Collections.Generic;
using Resources;


namespace Building.City
{
    public sealed class BuildingCity : IBuilding
    {
        private ConfigBuildingCityEditor _configBuilding;

        private CityPopulationReproduction _cityPopulationReproduction;

        private uint _population;


        public BuildingCity(in ConfigBuildingCityEditor configBuilding)
        {
            _cityPopulationReproduction = new(configBuilding);
            _configBuilding = configBuilding;

            _population = (uint)Random.Range(_configBuilding.populationStartMin, _configBuilding.populationStartMax);
        }

        public void ConstantUpdatingInfo()
        {
            _cityPopulationReproduction.PopulationReproduction(ref _population);
        }

        void IBuilding.ChangeJobStatus(in bool isState)
        {
            throw new System.NotImplementedException();
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            throw new System.NotImplementedException();
        }

        (bool confirm, float quantityAmount) IBuilding.GetResources(in float transportCapacity)
        {
            throw new System.NotImplementedException();
        }

        bool IBuilding.SetResources(in float quantityResource)
        {
            throw new System.NotImplementedException();
        }
    }
}
