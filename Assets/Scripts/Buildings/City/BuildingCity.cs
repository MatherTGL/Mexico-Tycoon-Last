using Config.Building;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Building.City
{
    public sealed class BuildingCity : IBuilding
    {
        private CityPopulationReproduction _cityPopulationReproduction;

        private CitySellDrugs _citySellDrugs = new CitySellDrugs();

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResource = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.d_amountResources
        {
            get => d_amountResource; set => d_amountResource = value;
        }

        private uint[] _costPerKg;

        private uint _population;

        private byte _indexSelectedDrugType;


        public BuildingCity(in ConfigBuildingCityEditor configBuilding)
        {
            _cityPopulationReproduction = new(configBuilding);

            InitArrays(configBuilding);

            _population = (uint)UnityEngine.Random.Range(
                configBuilding.populationStartMin, configBuilding.populationStartMax);
        }

        private void SellResources()
        {
            foreach (var drug in d_amountResource.Keys.ToArray())
            {
                _citySellDrugs.Sell(d_amountResource[drug], _costPerKg[(int)drug]);
                d_amountResource[drug] = 0;
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

        float IBuilding.GetResources(in float transportCapacity,
                                     in TypeProductionResources.TypeResource typeResource)
        {
            if (d_amountResource[typeResource] >= transportCapacity)
            {
                d_amountResource[typeResource] -= transportCapacity;
                return transportCapacity;
            }
            else return 0;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            d_amountResource[typeResource] += quantityResource;
            return true;
        }
    }
}
