using System.Collections.Generic;
using Config.Building;
using Resources;
using UnityEngine;

namespace Building.SeaPort
{
    public sealed class BuildingSeaPort : IBuilding
    {
        private readonly ConfigBuildingSeaPortEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, double> d_amountResources = new();

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new();

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;


        public BuildingSeaPort(in ScriptableObject config)
        {
            _config = (ConfigBuildingSeaPortEditor)config;
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            Debug.Log("Sea port is work");
        }
    }
}
