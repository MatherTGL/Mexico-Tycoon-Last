using Resources;
using Building.Additional;
using Config.Building;
using UnityEngine;
using System.Collections.Generic;

namespace Building.Aerodrome
{
    public sealed class BuildingAerodrome : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending
    {
        private ConfigBuildingAerodromeEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, double> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, double>();

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new Dictionary<TypeProductionResources.TypeResource, uint>();

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private double _maintenanceExpenses;
        double ISpending.maintenanceExpenses => _maintenanceExpenses;

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }


        public BuildingAerodrome(in ScriptableObject config)
        {
            _config = (ConfigBuildingAerodromeEditor)config;
            _maintenanceExpenses = _config.maintenanceExpenses;
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
