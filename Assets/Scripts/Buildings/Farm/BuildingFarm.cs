using Config.Building;
using System.Collections.Generic;
using Building.Additional;
using UnityEngine;
using Expense;
using Country;
using Config.Building.Events;
using Events.Buildings;
using Building.Additional.Production;
using static Resources.TypeProductionResources;
using System.Linq;
using SerializableDictionary.Scripts;
using static Config.Employees.ConfigEmployeeEditor;
using System.Threading.Tasks;
using static Config.Building.ConfigBuildingFarmEditor;
using Building.Additional.Crop;

namespace Building.Farm
{
    public sealed class BuildingFarm : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus,
    ISpending, IEnergyConsumption, IChangedFarmType, IUsesExpensesManagement, IProductionBuilding
    {
        private readonly INumberOfEmployees _InumberOfEmployees = new NumberOfEmployees();

        private readonly IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private readonly ChangeFarmType _changeFarmType = new();

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        private readonly IProduction _Iproduction;

        ConfigCropSpoilage IProductionBuilding.configCropSpoilage => _config.configCropSpoilage;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IProductionBuilding.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        IObjectsExpensesImplementation IUsesWeather.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        private ConfigBuildingFarmEditor _config;

        ConfigBuildingsEventsEditor IUsesBuildingsEvents.configBuildingsEvents => _config.configBuildingsEvents;

        Dictionary<TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeResource, double> IProductionBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeResource, double> IUsesBuildingsEvents.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        Dictionary<TypeEmployee, byte> IProductionBuilding.requiredEmployees
            => _config.requiredEmployees;

        public Dictionary<TypeResource, SerializableDictionary<TypeResource, int>> requiredRawMaterials
            => _config.requiredRawMaterials;

        Dictionary<TypeResource, uint> IBuilding.localCapacityProduction
            => _config.localCapacityProduction;

        Dictionary<TypeResource, uint> IProductionBuilding.localCapacityProduction
            => _config.localCapacityProduction;

        Dictionary<TypeResource, float> IProductionBuilding.harvestRipeningTime
            => _config.harvestRipeningTime;

        private TypeResource _typeProductionResource;

        TypeResource IProductionBuilding.typeProductionResource => _typeProductionResource;

        private double _costPurchase;
        double IBuildingPurchased.costPurchase { get => _costPurchase; set => _costPurchase = value; }

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
            _typeProductionResource = config.productionResources.Keys.First();
            _costPurchase = config.costPurchase;
        }

        private void CalculateTemporaryImpact(float impact)
        {
            double addingNumber = IobjectsExpensesImplementation.GetTotalExpenses() * impact / 100;
            IobjectsExpensesImplementation.ChangeSeasonExpenses(addingNumber);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (IsConditionsAreMet())
                _Iproduction.Production();
        }

        private bool IsConditionsAreMet()
        {
            bool isHiredEmployees = _InumberOfEmployees.IsThereAreEnoughEmployees(_config.requiredEmployees,
                                                                                  IobjectsExpensesImplementation.IhiringModel.GetAllEmployees());

            return isBuyed && isWorked && isHiredEmployees && IsGrowingSeason() ? true : false;
        }

        private bool IsGrowingSeason()
        {
            return _config.typeFarm is TypeFarm.Terrestrial
            ? _config.growingSeasons.Contains(_IcountryBuildings.IclimateZone.seasonControl.currentSeason)
            : true;
        }

        async ValueTask IChangedFarmType.ChangeType(TypeFarm typeFarm)
            => _config = await _changeFarmType.AsyncGetNewType(typeFarm);

        void IUsesCountryInfo.SetCountry(in ICountryBuildings IcountryBuildings)
        {
            _IcountryBuildings = IcountryBuildings;
            CalculateTemporaryImpact(_IcountryBuildings.IclimateZone.seasonControl.GetCurrentSeasonImpact());

            _IcountryBuildings.IclimateZone.seasonControl.updatedSeason += CalculateTemporaryImpact;
        }

        int IProductionBuilding.GetBaseProductionPerformance(in TypeResource typeResource)
            => _config.productionResources[typeResource];

        void IProductionBuilding.SetNewProductionResource(in TypeResource typeResource)
        {
            if (_config.productionResources.ContainsKey(typeResource))
                _typeProductionResource = typeResource;
        }
    }
}
