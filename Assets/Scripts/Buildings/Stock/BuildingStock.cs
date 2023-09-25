using Resources;
using System.Collections.Generic;
using Config.Building;
using Building.Additional;
using UnityEngine;

namespace Building.Stock
{
    public sealed class BuildingStock : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption
    {
        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private readonly ConfigBuildingStockEditor _config;

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

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
                Debug.Log("Stock is work");
        }
    }
}
