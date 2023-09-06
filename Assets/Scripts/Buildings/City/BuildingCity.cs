using Config.Building;
using Resources;
using System;


namespace Building.City
{
    public sealed class BuildingCity : IBuilding
    {
        private CityPopulationReproduction _cityPopulationReproduction;

        private CitySellDrugs _citySellDrugs;

        private float[] _quantityInStock;

        private uint[] _costPerKg;

        private uint _population;

        private byte _indexSelectedDrugType;


        public BuildingCity(in ConfigBuildingCityEditor configBuilding)
        {
            _cityPopulationReproduction = new(configBuilding);
            _citySellDrugs = new();

            InitArrays();

            _population = (uint)UnityEngine.Random.Range(
                configBuilding.populationStartMin, configBuilding.populationStartMax);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            _cityPopulationReproduction.PopulationReproduction(ref _population);
            SellResources();
        }

        float IBuilding.GetResources(in float transportCapacity, in TypeProductionResources.TypeResource typeResource)
        {
            _indexSelectedDrugType = GetIndexTypeResource(typeResource);
            if (_quantityInStock[_indexSelectedDrugType] >= transportCapacity)
            {
                _quantityInStock[_indexSelectedDrugType] -= transportCapacity;
                return transportCapacity;
            }
            else return 0;
        }

        bool IBuilding.SetResources(in float quantityResource, in TypeProductionResources.TypeResource typeResource)
        {
            _indexSelectedDrugType = GetIndexTypeResource(typeResource);
            _quantityInStock[_indexSelectedDrugType] += quantityResource;
            return true;
        }

        private void SellResources()
        {
            for (int i = 0; i < _quantityInStock.Length; i++)
                _citySellDrugs.Sell(ref _quantityInStock[i], _costPerKg[i]);
        }

        private void InitArrays()
        {
            int amountTypeDrugs = Enum.GetValues(typeof(TypeProductionResources.TypeResource)).Length;
            _quantityInStock = new float[amountTypeDrugs];
            _costPerKg = new uint[amountTypeDrugs];
        }

        private byte GetIndexTypeResource(in TypeProductionResources.TypeResource typeResource)
        {
            return (byte)typeResource;
        }
    }
}
