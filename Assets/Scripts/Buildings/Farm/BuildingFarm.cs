using Config.Building;
using System.Collections.Generic;
using Resources;
using Building.Additional;
using UnityEngine;
using Climate;
using Expense;
using static Expense.ExpensesEnumTypes;
using Hire;
using Country;

namespace Building.Farm
{
    public sealed class BuildingFarm : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IChangedFarmType, IUsesCountryInfo, IUsesExpensesManagement, IUsesHiring
    {
        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private ICountryBuildings _IcountryBuildings;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value;
        }

        private ConfigBuildingFarmEditor _config;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        private TypeProductionResources.TypeResource _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase => _costPurchase;

        private ushort _productionPerformance;

        private float _currentPercentageOfMaturity;

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }

        private bool _isCurrentlyInProduction;


        public BuildingFarm(in ScriptableObject config)
        {
            _config = config as ConfigBuildingFarmEditor;
            LoadConfigData(_config);
        }

        private void LoadConfigData(in ConfigBuildingFarmEditor config)
        {
            _productionPerformance = config.productionStartPerformance;
            _typeProductionResource = config.typeProductionResource;
            _costPurchase = config.costPurchase;
        }

        private void Production()
        {
            Debug.Log($"Production farm: {d_amountResources[_typeProductionResource]}");
            double localCapacity = _config.localCapacityProduction[(int)_typeProductionResource];

            if (d_amountResources[_typeProductionResource] < localCapacity)
            {
                if (_isCurrentlyInProduction == false && CheckQuantityRequiredRawMaterials() == false)
                    return;

                _isCurrentlyInProduction = true;

                if (_currentPercentageOfMaturity < _config.harvestRipeningTime)
                {
                    _currentPercentageOfMaturity++;
                }
                else
                {
                    d_amountResources[_typeProductionResource] += _productionPerformance;
                    _currentPercentageOfMaturity = 0;
                    _isCurrentlyInProduction = false;
                }
            }
        }

        private bool CheckQuantityRequiredRawMaterials()
        {
            foreach (var typeDrug in _config.requiredRawMaterials)
            {
                for (ushort i = 0; i < _config.quantityRequiredRawMaterials.Count; i++)
                    if (d_amountResources[typeDrug] < _config.quantityRequiredRawMaterials[i])
                        return false;
                d_amountResources[typeDrug] -= _config.quantityRequiredRawMaterials[0];
            }
            return true;
        }

        private void CalculateImpactClimateZones()
        {
            double addingNumber = IobjectsExpensesImplementation.GetTotalExpenses() * _IcountryBuildings
                .configClimate.percentageImpactCostMaintenance;

            Debug.Log(addingNumber);

            IobjectsExpensesImplementation.ChangeExpenses(addingNumber, AreaExpenditureType.Production,
                                                           isAdd: true);
        }

        private bool IsGrowingSeason()
        {
            if (_config.typeFarm is ConfigBuildingFarmEditor.TypeFarm.Terrestrial)
                return _config.growingSeasons.Contains(_IcountryBuildings.IclimateZone.GetCurrentSeason());
            else
                return true;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (isWorked && isBuyed && IsGrowingSeason())
                Production();

            if (!isBuyed)
            {
                _costPurchase += _costPurchase * _IcountryBuildings.IcountryInflation.GetTotalInflation() / 100;
                Debug.Log(_costPurchase);
            }
        }

        void IChangedFarmType.ChangeType(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            foreach (var config in UnityEngine.Resources.FindObjectsOfTypeAll<ConfigBuildingFarmEditor>())
                if (config.typeFarm == typeFarm)
                    _config = config;
        }

        void IUsesCountryInfo.SetCountry(in ICountryBuildings IcountryBuildings)
        {
            _IcountryBuildings = IcountryBuildings;
            CalculateImpactClimateZones();
        }
    }
}
