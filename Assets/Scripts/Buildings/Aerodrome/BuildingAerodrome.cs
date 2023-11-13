using Resources;
using Building.Additional;
using Config.Building;
using UnityEngine;
using System.Collections.Generic;
using Expense;
using Country;

namespace Building.Aerodrome
{
    public sealed class BuildingAerodrome : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IUsesExpensesManagement
    {
        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value;
        }

        private ICountryBuildings _IcountryBuildings;
        ICountryBuildings IUsesCountryInfo.IcountryBuildings { get => _IcountryBuildings; set => _IcountryBuildings = value; }

        private readonly ConfigBuildingAerodromeEditor _config;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
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
            if (isBuyed && isWorked)
            {
                Debug.Log("Aerodrome is work");
            }
        }
    }
}
