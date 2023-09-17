using System.Collections.Generic;
using Config.Building;
using Resources;
using UnityEngine;

namespace Building.SeaPort
{
    public sealed class BuildingSeaPort : IBuilding
    {
        private ConfigBuildingSeaPortEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new Dictionary<TypeProductionResources.TypeResource, uint>();

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;


        public BuildingSeaPort(in ScriptableObject config)
        {
            _config = (ConfigBuildingSeaPortEditor)config;
            Debug.Log("Sea Port success init");
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            Debug.Log("Sea port is work");
        }
    }
}
