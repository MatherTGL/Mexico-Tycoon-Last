using Resources;
using System.Collections.Generic;
using Config.Building;
using Building.Additional;
using UnityEngine;
using Expense;

namespace Building.Stock
{
    public sealed class BuildingStock : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IUsesExpensesManagement
    {
        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => _IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => _IobjectsExpensesImplementation; set => _IobjectsExpensesImplementation = value;
        }

        private readonly ConfigBuildingStockEditor _config;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingStock(in ScriptableObject config)
        {
            _config = config as ConfigBuildingStockEditor;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
                Debug.Log("Stock is work");
        }
    }
}
