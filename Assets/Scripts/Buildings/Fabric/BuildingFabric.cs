using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Expense;
using Resources;
using UnityEngine;

namespace Building.Fabric
{
    public sealed class BuildingFabric : AbstractBuilding, IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IUsesExpensesManagement
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => IobjectsExpensesImplementation; set => IobjectsExpensesImplementation = value;
        }

        private ConfigBuildingFabricEditor _config;

        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        private TypeProductionResources.TypeResource _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private ushort _productionPerformance;

        bool IBuildingJobStatus.isWorked { get => isWorked; set => isWorked = value; }

        bool IBuildingPurchased.isBuyed { get => isBuyed; set => isBuyed = value; }


        public BuildingFabric(in ScriptableObject config)
        {
            _config = config as ConfigBuildingFabricEditor;
            LoadConfigData(_config);
        }

        private void LoadConfigData(in ConfigBuildingFabricEditor config)
        {
            _typeProductionResource = TypeProductionResources.TypeResource.Cocaine;
            _productionPerformance = config.productionPerformance;
        }

        private void Production()
        {
            Debug.Log($"Production fabric: {d_amountResources[_typeProductionResource]}");
            if (d_amountResources[_typeProductionResource]
                < _config.localCapacityProduction[(int)d_amountResources[_typeProductionResource]])
            {
                foreach (var typeDrug in _config.requiredRawMaterials)
                {
                    for (ushort i = 0; i < _config.quantityRequiredRawMaterials.Count; i++)
                        if (d_amountResources[typeDrug] < _config.quantityRequiredRawMaterials[i])
                            return;

                    d_amountResources[typeDrug] -= _config.quantityRequiredRawMaterials[0];
                }
                d_amountResources[_typeProductionResource] += _productionPerformance;
                Debug.Log(d_amountResources[_typeProductionResource]);
            }
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (isBuyed && isWorked)
                Production();
        }
    }
}
