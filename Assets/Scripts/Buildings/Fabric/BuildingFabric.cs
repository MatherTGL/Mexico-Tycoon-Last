using System.Collections.Generic;
using System.Linq;
using Building.Additional;
using Building.Additional.Production;
using Config.Building;
using Config.Building.Events;
using Config.Employees;
using Country;
using Events.Buildings;
using Expense;
using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building.Fabric
{
    public sealed class BuildingFabric : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IUsesExpensesManagement,
        IProductionBuilding
    {
        private readonly INumberOfEmployees _InumberOfEmployees = new NumberOfEmployees();

        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        private readonly IProduction _Iproduction;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IProductionBuilding.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        private readonly ConfigBuildingFabricEditor _config;

        ConfigBuildingsEventsEditor IUsesBuildingsEvents.configBuildingsEvents => _config.configBuildingsEvents;

        Dictionary<TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeResource, double> IProductionBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeResource, double> IUsesBuildingsEvents.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        Dictionary<ConfigEmployeeEditor.TypeEmployee, byte> IProductionBuilding.requiredEmployees => _config.requiredEmployees.Dictionary;

        List<TypeResource> IProductionBuilding.requiredRawMaterials => _config.requiredRawMaterials;

        List<float> IProductionBuilding.quantityRequiredRawMaterials => _config.quantityRequiredRawMaterials;

        private TypeResource _typeProductionResource;

        TypeResource IProductionBuilding.typeProductionResource => _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        uint[] IProductionBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

        ushort IProductionBuilding.defaultProductionPerformance => _config.productionResources[_typeProductionResource];

        float IProductionBuilding.harvestRipeningTime => _config.harvestRipeningTime;

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }


        public BuildingFabric(in ScriptableObject config)
        {
            _config = config as ConfigBuildingFabricEditor;
            _Iproduction = new Production(this);

            if (_config != null)
                LoadConfigData(_config);
        }

        //TODO: change typeProductionResource https://ru.yougile.com/team/bf00efa6ea26/#MEX-87
        private void LoadConfigData(in ConfigBuildingFabricEditor config)
        {
            _typeProductionResource = config.productionResources.Keys.First();
            _costPurchase = _config.costPurchase;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (IsConditionsAreMet())
                _Iproduction.Production();
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
