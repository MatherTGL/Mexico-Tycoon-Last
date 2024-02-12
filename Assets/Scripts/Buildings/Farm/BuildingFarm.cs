using Config.Building;
using System.Collections.Generic;
using Resources;
using Building.Additional;
using UnityEngine;
using Expense;
using Country;
using Config.Building.Events;
using Events.Buildings;
using Building.Additional.Production;
using Config.Employees;

namespace Building.Farm
{
    public sealed class BuildingFarm : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IChangedFarmType, IUsesExpensesManagement,
        IProductionBuilding
    {
        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        private readonly IProduction _Iproduction;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IProductionBuilding.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        private ConfigBuildingFarmEditor _config;

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

        private TypeProductionResources.TypeResource _typeProductionResource; //! move to Production class

        List<TypeProductionResources.TypeResource> IProductionBuilding.requiredRawMaterials => _config.requiredRawMaterials;

        List<float> IProductionBuilding.quantityRequiredRawMaterials => _config.quantityRequiredRawMaterials;

        TypeProductionResources.TypeResource IProductionBuilding.typeProductionResource => _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        uint[] IProductionBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

        ushort IProductionBuilding.defaultProductionPerformance => _config.productionStartPerformance;

        float IProductionBuilding.harvestRipeningTime => _config.harvestRipeningTime;

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }


        public BuildingFarm(in ScriptableObject config)
        {
            _config = config as ConfigBuildingFarmEditor;
            _Iproduction = new Production(this);

            LoadConfigData(_config);
        }

        private void LoadConfigData(in ConfigBuildingFarmEditor config)
        {
            _typeProductionResource = config.typeProductionResource;
            _costPurchase = config.costPurchase;
        }

        private bool IsGrowingSeason()
        {
            if (_config.typeFarm is ConfigBuildingFarmEditor.TypeFarm.Terrestrial)
                return _config.growingSeasons.Contains(_IcountryBuildings.IclimateZone.GetCurrentSeason());
            else
                return true;
        }

        private void CalculateTemporaryImpact(float impact)
        {
            double addingNumber = IobjectsExpensesImplementation.GetTotalExpenses() * impact / 100;
            IobjectsExpensesImplementation.ChangeSeasonExpenses(addingNumber);
        }

        private bool IsThereAreEnoughEmployees()
        {
            foreach (var employee in _config.requiredEmployees.Dictionary.Keys)
            {
                if (IobjectsExpensesImplementation.Ihiring.GetAllEmployees().ContainsKey(employee) == false ||
                    IobjectsExpensesImplementation.Ihiring.GetAllEmployees()[employee].Count < _config.requiredEmployees.Dictionary[employee])
                {
                    return false;
                }
            }

            return true;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            Debug.Log($"{this} / IsThereAreEnoughEmployees(): {IsThereAreEnoughEmployees()}");
            if (isWorked && isBuyed && IsGrowingSeason() && IsThereAreEnoughEmployees())
                _Iproduction.Production();
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
            CalculateTemporaryImpact(_IcountryBuildings.IclimateZone.GetCurrentSeasonImpact());

            _IcountryBuildings.IclimateZone.updatedSeason += CalculateTemporaryImpact;
        }
    }
}
