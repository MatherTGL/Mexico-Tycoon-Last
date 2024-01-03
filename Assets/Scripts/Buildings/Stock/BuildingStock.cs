using Resources;
using System.Collections.Generic;
using Config.Building;
using Building.Additional;
using UnityEngine;
using Expense;
using Country;
using Config.Building.Events;
using Events.Buildings;
using Building.Farm;

namespace Building.Stock
{
    public sealed class BuildingStock : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IUsesExpensesManagement, ICleaningResources
    {
        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value;
        }
        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        private readonly ConfigBuildingStockEditor _config;

        ConfigBuildingsEventsEditor IUsesBuildingsEvents.configBuildingsEvents => _config.configBuildingsEvents;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        Dictionary<TypeProductionResources.TypeResource, double> IUsesBuildingsEvents.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }


        public BuildingStock(in ScriptableObject config)
        {
            _config = config as ConfigBuildingStockEditor;

            if (_config != null)
                _costPurchase = _config.costPurchase;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (isBuyed && isWorked)
                Debug.Log("Stock is work");
        }

        void ICleaningResources.Clear(in TypeProductionResources.TypeResource typeResource, in double amount)
        {
            if (d_amountResources.ContainsKey(typeResource))
                if (d_amountResources[typeResource] - amount >= 0)
                    d_amountResources[typeResource] -= amount;
        }
    }
}
