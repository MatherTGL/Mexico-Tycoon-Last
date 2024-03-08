using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Config.Building.Events;
using Country;
using Events.Buildings;
using Expense;
using Resources;
using UnityEngine;

namespace Building.SeaPort
{
    public sealed class BuildingSeaPort : AbstractBuilding, IBuilding, IUsesExpensesManagement, IBuildingJobStatus, ISpending, IBuildingPurchased
    {
        private readonly INumberOfEmployees _InumberOfEmployees = new NumberOfEmployees();

        private readonly ConfigBuildingSeaPortEditor _config;

        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        ConfigBuildingsEventsEditor IUsesBuildingsEvents.configBuildingsEvents => _config.configEvents;

        private ICountryBuildings _IcountryBuildings;

        ICountryBuildings IUsesCountryInfo.IcountryBuildings
        { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        Dictionary<TypeProductionResources.TypeResource, double> IUsesBuildingsEvents.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }
        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }


        public BuildingSeaPort(in ScriptableObject config)
        {
            _config = (ConfigBuildingSeaPortEditor)config;

            if (_config == null)
                throw new System.Exception("Config is null");

            _costPurchase = _config.costPurchase;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (IsConditionsAreMet())
                Debug.Log("Sea port is work");
        }

        private bool IsConditionsAreMet()
        {
            bool isHiredEmployees = _InumberOfEmployees.IsThereAreEnoughEmployees(_config.requiredEmployees.Dictionary,
                                                                                  IobjectsExpensesImplementation.IhiringModel.GetAllEmployees());

            if (isBuyed && isWorked && isHiredEmployees)
                return true;
            else
                return false;
        }
    }
}
