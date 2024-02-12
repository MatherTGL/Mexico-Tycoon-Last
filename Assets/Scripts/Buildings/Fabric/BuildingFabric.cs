using System.Collections.Generic;
using Building.Additional;
using Building.Additional.Production;
using Config.Building;
using Config.Building.Events;
using Config.Employees;
using Country;
using Events.Buildings;
using Expense;
using Resources;
using UnityEngine;

namespace Building.Fabric
{
    public sealed class BuildingFabric : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IUsesExpensesManagement,
        IProductionBuilding
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        private IProduction _Iproduction;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IProductionBuilding.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        private readonly ConfigBuildingFabricEditor _config;

        ConfigBuildingsEventsEditor IUsesBuildingsEvents.configBuildingsEvents => _config.configBuildingsEvents;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, double> IProductionBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, double> IUsesBuildingsEvents.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        Dictionary<ConfigEmployeeEditor.TypeEmployee, byte> IProductionBuilding.requiredEmployees => _config.requiredEmployees.Dictionary;

        List<TypeProductionResources.TypeResource> IProductionBuilding.requiredRawMaterials => _config.requiredRawMaterials;

        List<float> IProductionBuilding.quantityRequiredRawMaterials => _config.quantityRequiredRawMaterials;

        private TypeProductionResources.TypeResource _typeProductionResource;

        TypeProductionResources.TypeResource IProductionBuilding.typeProductionResource => _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        uint[] IProductionBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

        ushort IProductionBuilding.defaultProductionPerformance => _config.productionPerformance;

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
            _typeProductionResource = config.typeProductionResource;
            _costPurchase = _config.costPurchase;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            Debug.Log($"{this} / IsThereAreEnoughEmployees(): {IsThereAreEnoughEmployees()}");
            if (isBuyed && isWorked && IsThereAreEnoughEmployees())
                _Iproduction.Production();
        }

        //TODO: move to general class for buildings
        private bool IsThereAreEnoughEmployees()
        {
            foreach (var employee in _config.requiredEmployees.Dictionary.Keys)
            {
#if UNITY_EDITOR
                if (IobjectsExpensesImplementation.Ihiring.GetAllEmployees().ContainsKey(employee))
                    Debug.Log($"{IobjectsExpensesImplementation.Ihiring.GetAllEmployees()[employee].Count}");
#endif

                if (IobjectsExpensesImplementation.Ihiring.GetAllEmployees().ContainsKey(employee) == false
                    || IobjectsExpensesImplementation.Ihiring.GetAllEmployees()[employee].Count < _config.requiredEmployees.Dictionary[employee])
                    return false;
            }

            return true;
        }
    }
}
