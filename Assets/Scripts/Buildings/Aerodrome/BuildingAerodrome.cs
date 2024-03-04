using Resources;
using Building.Additional;
using Config.Building;
using UnityEngine;
using System.Collections.Generic;
using Expense;
using Country;
using Config.Building.Events;
using Events.Buildings;

namespace Building.Aerodrome
{
    public sealed class BuildingAerodrome : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, IEnergyConsumption, ISpending, IUsesExpensesManagement,
        IPackaging
    {
        private IProductPackaging _IproductPackaging;

        private readonly INumberOfEmployees _InumberOfEmployees = new NumberOfEmployees();

        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();

        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        private readonly ConfigBuildingAerodromeEditor _config;

        ConfigBuildingsEventsEditor IUsesBuildingsEvents.configBuildingsEvents => _config.configBuildingsEvents;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, double> IUsesBuildingsEvents.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }


        public BuildingAerodrome(in ScriptableObject config)
        {
            _config = config as ConfigBuildingAerodromeEditor;

            if (_config != null)
                _costPurchase = _config.costPurchase;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (IsConditionsAreMet())
                Debug.Log("Aerodrome is work");
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
