using Config.Building;
using UnityEngine;

namespace Building.City
{
    public sealed class CityPopulationReproduction
    {
        private const float _mathematicalDivisor = 100;

        private readonly ConfigBuildingCityEditor _configBuilding;


        public CityPopulationReproduction(in ConfigBuildingCityEditor configBuilding)
            => _configBuilding = configBuilding;

        public void PopulationReproduction(ref uint populationCity)
        {
            float populationChangeStepPercent = Random.Range(_configBuilding.populationChangeStepPercentMin,
                                                             _configBuilding.populationChangeStepPercentMax);

            uint addedNumberPeople = (uint)(populationCity * populationChangeStepPercent / _mathematicalDivisor);
            populationCity += addedNumberPeople;
        }
    }
}
