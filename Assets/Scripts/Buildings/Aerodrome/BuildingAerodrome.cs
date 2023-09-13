using Resources;
using Building.Additional;
using Config.Building;
using UnityEngine;
using System.Collections.Generic;


namespace Building.Aerodrome
{
    public sealed class BuildingAerodrome : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending
    {
        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.d_amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private double _maintenanceExpenses;

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }


        public BuildingAerodrome(in ConfigBuildingAerodromeEditor config)
        {
            _maintenanceExpenses = config.maintenanceExpenses;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
            {
                Debug.Log("Aerodrome is work");
            }
        }

        float IBuilding.GetResources(in float transportCapacity,
                                     in TypeProductionResources.TypeResource typeResource)
        {
            if (d_amountResources[typeResource] >= transportCapacity)
            {
                d_amountResources[typeResource] -= transportCapacity;
                return transportCapacity;
            }
            else
                return 0;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            d_amountResources[typeResource] += quantityResource;
            return true;
        }

        void ISpending.Spending()
        {
            SpendingToObjects.SendNewExpense(_maintenanceExpenses);
        }
    }
}
