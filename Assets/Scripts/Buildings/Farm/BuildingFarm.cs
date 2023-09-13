using Config.Building;
using System.Collections.Generic;
using Resources;
using Building.Additional;


namespace Building.Farm
{
    public sealed class BuildingFarm : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();

        private ConfigBuildingFarmEditor _config;

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.d_amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private TypeProductionResources.TypeResource _typeProductionResource;

        private ushort _productionPerformance;

        private uint _localCapacityProduct;

        private double _maintenanceExpenses;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingFarm(in ConfigBuildingFarmEditor config)
        {
            _productionPerformance = config.productionStartPerformance;
            _typeProductionResource = config.typeProductionResource;
            _localCapacityProduct = config.localCapacityProduction;
            _config = config;

            _maintenanceExpenses = _config.maintenanceExpenses;

            AddTypesResources();
        }

        private void AddTypesResources()
        {
            d_amountResources.Add(_typeProductionResource, 0);

            foreach (var typeDrug in _config.requiredRawMaterials)
                if (d_amountResources.ContainsKey(typeDrug) == false)
                    d_amountResources.Add(typeDrug, 0);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isWorked && _isBuyed)
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
            else
                return 0;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            d_amountResources[typeResource] += quantityResource;
            return true;
        }

        private void Production()
        {
            if (d_amountResources[_typeProductionResource] < _localCapacityProduct)
            {
                foreach (var typeDrug in _config.requiredRawMaterials)
                {
                    for (ushort i = 0; i < _config.quantityRawMaterials.Count; i++)
                        if (d_amountResources[typeDrug] < _config.quantityRawMaterials[i])
                            return;

                    d_amountResources[typeDrug] -= _config.quantityRawMaterials[0];
                }
                d_amountResources[_typeProductionResource] += _productionPerformance;
            }
        }

        public void Spending()
        {
            SpendingToObjects.SendNewExpense(_maintenanceExpenses);
        }

        private void MonitorEnergyConsumption()
        {
            _IbuildingMonitorEnergy.CalculateConsumption(this);
        }
    }
}
