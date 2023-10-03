using Config.Building;
using System.Collections.Generic;
using Resources;
using Building.Additional;
using UnityEngine;
using Climate;
using Expense;

namespace Building.Farm
{
    public sealed class BuildingFarm : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption,
    IChangedFarmType, IUsesClimateInfo, IUsesExpensesManagement
    {
        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private IClimateZone _IclimateZone;

        private IObjectsExpensesImplementation _IobjectsExpensesImplementation;

        private ConfigBuildingFarmEditor _config;

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

        private TypeProductionResources.TypeResource _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private ushort _productionPerformance;

        private float _currentPercentageOfMaturity;

        double ISpending.maintenanceExpenses => _IobjectsExpensesImplementation.GetAllExpenses();

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }

        private bool _isCurrentlyInProduction;


        public BuildingFarm(in ScriptableObject config)
        {
            _config = (ConfigBuildingFarmEditor)config;
            LoadConfigData(_config);
        }

        private void LoadConfigData(in ConfigBuildingFarmEditor config)
        {
            _productionPerformance = config.productionStartPerformance;
            _typeProductionResource = config.typeProductionResource;
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
            double addingNumber = _IobjectsExpensesImplementation.GetAllExpenses() * _IclimateZone
                .configClimateZone.percentageImpactCostMaintenance;

            _IobjectsExpensesImplementation.ChangeExpenses(addingNumber, 
            ExpensesBuildings.TypeExpenses.General, 
            ExpenseManagementControl.AddOrReduceNumber.Add);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isWorked && _isBuyed)
                Production();
        }

        void IChangedFarmType.ChangeType(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            foreach (var config in UnityEngine.Resources.FindObjectsOfTypeAll<ConfigBuildingFarmEditor>())
                if (config.name.Contains(typeFarm.ToString()))
                    _config = config;
        }

        void IUsesClimateInfo.SetClimateZone(in IClimateZone IclimateZone)
        {
            _IclimateZone = IclimateZone;
            CalculateImpactClimateZones();
        }

        void IUsesExpensesManagement.LoadExpensesManagement(in IExpensesManagement IexpensesManagement)
        {
            _IobjectsExpensesImplementation = IexpensesManagement.Registration(
                this, ExpenseManagementControl.Type.Building);

            _IobjectsExpensesImplementation.SetAllExpensesInStart(_config.currentMaintenanceExpenses);
        }
    }
}
