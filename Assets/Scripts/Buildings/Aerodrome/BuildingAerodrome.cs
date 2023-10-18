using Resources;
using Building.Additional;
using Config.Building;
using UnityEngine;
using System.Collections.Generic;
using Expense;

namespace Building.Aerodrome
{
    public sealed class BuildingAerodrome : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IUsesExpensesManagement
    {
        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value;
        }

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

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }


        public BuildingAerodrome(in ScriptableObject config)
        {
            _config = config as ConfigBuildingAerodromeEditor;
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
