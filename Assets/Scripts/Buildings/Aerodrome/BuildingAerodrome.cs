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
        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => _IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => _IobjectsExpensesImplementation; set => _IobjectsExpensesImplementation = value;
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

        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }

        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }


        public BuildingAerodrome(in ScriptableObject config)
        {
            _config = config as ConfigBuildingAerodromeEditor;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
            {
                Debug.Log("Aerodrome is work");
            }
        }
    }
}
