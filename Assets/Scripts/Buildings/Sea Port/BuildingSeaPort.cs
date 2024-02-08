using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Expense;
using Resources;
using UnityEngine;

namespace Building.SeaPort
{
    public sealed class BuildingSeaPort : AbstractBuilding, IBuilding, IUsesExpensesManagement, IBuildingJobStatus, ISpending
    {
        private readonly ConfigBuildingSeaPortEditor _config;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;

        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        { get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value; }

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        { get => d_amountResources; set => d_amountResources = value; }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        { get => d_stockCapacity; set => d_stockCapacity = value; }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }


        public BuildingSeaPort(in ScriptableObject config)
            => _config = (ConfigBuildingSeaPortEditor)config;

        private bool IsThereAreEnoughEmployees()
        {
            foreach (var employee in _config.requiredEmployees.Dictionary.Keys)
                if (IobjectsExpensesImplementation.Ihiring.GetAllEmployees().ContainsKey(employee) == false ||
                    IobjectsExpensesImplementation.Ihiring.GetAllEmployees()[employee].Count < _config.requiredEmployees.Dictionary[employee])
                    return false;

            return true;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (isWorked && isBuyed && IsThereAreEnoughEmployees())
                Debug.Log("Sea port is work");
        }
    }
}
