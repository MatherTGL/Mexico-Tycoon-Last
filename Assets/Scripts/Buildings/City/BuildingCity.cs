using Building.City.Business;
using Config.Building;
using Regulation;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Building.City
{
    public sealed class BuildingCity : AbstractBuilding, IBuilding, ICityBusiness, IRegulationBuilding
    {
        private IRegulationCostSale _IregulationCostSale;
        IRegulationCostSale IRegulationBuilding.IregulationCostSale => _IregulationCostSale;

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

        private int _cellIndexRegulationCostSale;
        int IRegulationBuilding.cellIndexRegulationCostSale => _cellIndexRegulationCostSale;


        public BuildingCity(in ScriptableObject config, in IRegulationCostSale regulationCostSale)
        {
            _config = config as ConfigBuildingCityEditor;
            _cityPopulationReproduction = new(_config);
            _cityBusiness = new(this);
            _IregulationCostSale = regulationCostSale;

            InitArrays(_config, _IregulationCostSale);
            _population = (uint)UnityEngine.Random.Range(
                _config.populationStartMin, _config.populationStartMax);
        }

        private void InitArrays(in ConfigBuildingCityEditor configBuilding,
                                in IRegulationCostSale regulationCostSale)
        {
            int amountTypeDrugs = Enum.GetValues(typeof(TypeProductionResources.TypeResource)).Length;

            var costPerKg = new uint[amountTypeDrugs];
            costPerKg = configBuilding.costResourcesConfig.GetCostsSellResources();
            _cellIndexRegulationCostSale = regulationCostSale.Registration(costPerKg);
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
                    double salesProfit = d_amountResources[drug] * _IregulationCostSale.GetResourceCosts(_cellIndexRegulationCostSale)[(int)drug];
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
