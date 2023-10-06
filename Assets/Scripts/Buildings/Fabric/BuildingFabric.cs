using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Expense;
using Resources;
using UnityEngine;

namespace Building.Fabric
{
    public sealed class BuildingFabric : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption, IUsesExpensesManagement
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private IObjectsExpensesImplementation _IobjectsExpensesImplementation;
        IObjectsExpensesImplementation ISpending.IobjectsExpensesImplementation => _IobjectsExpensesImplementation;
        IObjectsExpensesImplementation IUsesExpensesManagement.IobjectsExpensesImplementation
        {
            get => _IobjectsExpensesImplementation; set => _IobjectsExpensesImplementation = value;
        }

        private ConfigBuildingFabricEditor _config;

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

        private TypeProductionResources.TypeResource _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private ushort _productionPerformance;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


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
            if (_isBuyed && _isWorked)
                Production();
        }
    }
}
