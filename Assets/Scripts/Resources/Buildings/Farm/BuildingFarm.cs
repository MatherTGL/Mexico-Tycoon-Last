using Config.Building;
using System.Collections.Generic;
using Resources;
using Building.Additional;
using UnityEngine;

namespace Building.Farm
{
    public sealed class BuildingFarm : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption,
    IChangedFarmType
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();
        IBuildingMonitorEnergy IEnergyConsumption.IbuildingMonitorEnergy => _IbuildingMonitorEnergy;

        private ConfigBuildingFarmEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private Dictionary<TypeProductionResources.TypeResource, uint> d_stockCapacity = new Dictionary<TypeProductionResources.TypeResource, uint>();

        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => d_stockCapacity; set => d_stockCapacity = value;
        }

        private TypeProductionResources.TypeResource _typeProductionResource;

        uint[] IBuilding.localCapacityProduction => _config.localCapacityProduction;

        private ushort _productionPerformance;

        private double _maintenanceExpenses;
        double ISpending.maintenanceExpenses => _maintenanceExpenses;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingFarm(in ScriptableObject config)
        {
            _config = (ConfigBuildingFarmEditor)config;
            LoadConfigData(_config);
        }

        private void LoadConfigData(in ConfigBuildingFarmEditor config)
        {
            _productionPerformance = config.productionStartPerformance;
            _typeProductionResource = config.typeProductionResource;
            _maintenanceExpenses = config.maintenanceExpenses;
        }

        private void Production()
        {
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
            if (_isWorked && _isBuyed)
                Production();
        }

        void IChangedFarmType.ChangeType(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            foreach (var config in UnityEngine.Resources.FindObjectsOfTypeAll<ConfigBuildingFarmEditor>())
                if (config.name.Contains(typeFarm.ToString()))
                    _config = config;
            Debug.Log(_config);
        }
    }
}
