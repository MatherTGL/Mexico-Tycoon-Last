using Config.Building;
using System.Collections.Generic;
using Resources;
using DebugCustomSystem;
using Building.Additional;
using UnityEngine;


namespace Building.Farm
{
    public sealed class BuildingFarm : IBuilding, IBuildingPurchased, IBuildingJobStatus
    {
        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        private TypeProductionResources.TypeResource _typeProductionResource;

        private ushort _productionPerformance;

        private uint _localCapacityProduct;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingFarm(in ConfigBuildingFarmEditor config)
        {
            _productionPerformance = config.productionStartPerformance;
            _typeProductionResource = config.typeProductionResource;
            _localCapacityProduct = config.localCapacityProduction;

            d_amountResources.Add(_typeProductionResource, 0);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isWorked) Production();

            MonitorEnergyConsumption();
        }

        float IBuilding.GetResources(in float transportCapacity,
                                     in TypeProductionResources.TypeResource typeResource)
        {
            CheckIncomingDrugType(typeResource);

            if (d_amountResources[typeResource] >= transportCapacity)
            {
                d_amountResources[typeResource] -= transportCapacity;
                return transportCapacity;
            }
            else return 0;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            CheckIncomingDrugType(typeResource);
            d_amountResources[typeResource] += quantityResource;
            Debug.Log($"Farm: {d_amountResources[typeResource]}");
            return true;
        }

        private void Production()
        {
            if (d_amountResources[_typeProductionResource] < _localCapacityProduct)
                d_amountResources[_typeProductionResource] += _productionPerformance;
        }

        private void CheckIncomingDrugType(in TypeProductionResources.TypeResource typeResource)
        {
            if (d_amountResources.ContainsKey(typeResource) is false)
                d_amountResources.Add(typeResource, 0);
        }

        private void MonitorEnergyConsumption()
        {
            if (_isWorked)
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green,
                    "Потребление энергии фермой составляет 10квт", "Building", "Farm", "Energy");
            else
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green,
                    "Потребление энергии фермой составляет 0квт", "Building", "Farm", "Energy");
        }
    }
}
