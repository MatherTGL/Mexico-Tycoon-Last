using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Resources;
using UnityEngine;

namespace Building.Fabric
{
    public sealed class BuildingFabric : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();

        private ConfigBuildingFabricEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.d_amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private TypeProductionResources.TypeResource _typeProductionResource;

        private ushort _productionPerformance;

        private double _maintenanceExpenses;

        private uint _localCapacityProduct;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingFabric(in ConfigBuildingFabricEditor config)
        {
            LoadConfigData(config);
            d_amountResources.Add(_typeProductionResource, 0);

            foreach (var typeDrug in _config.requiredRawMaterials)
                if (d_amountResources.ContainsKey(typeDrug) == false)
                    d_amountResources.Add(typeDrug, 0);
        }

        private void LoadConfigData(in ConfigBuildingFabricEditor config)
        {
            _typeProductionResource = TypeProductionResources.TypeResource.Cocaine;
            _productionPerformance = config.productionPerformance;
            _maintenanceExpenses = config.maintenanceExpenses;
            _localCapacityProduct = config.localCapacityProduction;

            _config = config;
        }

        private void Production()
        {
            if (d_amountResources[_typeProductionResource] < _localCapacityProduct)
            {
                foreach (var typeDrug in _config.requiredRawMaterials)
                {
                    for (ushort i = 0; i < _config.quantityRequiredRawMaterials.Count; i++)
                        if (d_amountResources[typeDrug] < _config.quantityRequiredRawMaterials[i])
                            return;

                    d_amountResources[typeDrug] -= _config.quantityRequiredRawMaterials[0];
                }
                d_amountResources[_typeProductionResource] += _productionPerformance;
            }
        }

        private void MonitorEnergyConsumption()
        {
            _IbuildingMonitorEnergy.CalculateConsumption(this);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
            {
                Spending();
                Production();
                MonitorEnergyConsumption();
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
            else return 0;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            d_amountResources[typeResource] += quantityResource;
            Debug.Log($"Fabric Set {d_amountResources[typeResource]}");
            return true;
        }

        public void Spending()
        {
            SpendingToObjects.SendNewExpense(_maintenanceExpenses);
        }
    }
}
