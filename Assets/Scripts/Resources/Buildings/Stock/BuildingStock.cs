using Resources;
using System.Collections.Generic;
using Config.Building;
using Building.Additional;
using UnityEngine;

namespace Building.Stock
{
    public sealed class BuildingStock : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();

        private ConfigBuildingStockEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

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

        private double _maintenanceExpenses;
        double ISpending.maintenanceExpenses => _maintenanceExpenses;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingStock(in ScriptableObject config)
        {
            _config = (ConfigBuildingStockEditor)config;
            _maintenanceExpenses = _config.maintenanceExpenses;
        }

        private void MonitorEnergyConsumption()
        {
            _IbuildingMonitorEnergy.CalculateConsumption(this);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
                MonitorEnergyConsumption();
        }
    }
}