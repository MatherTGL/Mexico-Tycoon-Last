using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Resources;


namespace Building.Fabric
{
    public sealed class BuildingFabric : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.d_amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private TypeProductionResources.TypeResource _typeProductionResource;

        private ushort _productionPerformance;

        private ushort _productConversionStep;

        private double _maintenanceExpenses;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingFabric(in ConfigBuildingFabricEditor config)
        {
            LoadConfigData(config);
            d_amountResources.Add(_typeProductionResource, 0);
        }

        private void LoadConfigData(in ConfigBuildingFabricEditor config)
        {
            _typeProductionResource = TypeProductionResources.TypeResource.Cocaine;
            _productionPerformance = config.productionPerformance;
            _productConversionStep = config.productConversionStep; ;
            _maintenanceExpenses = config.maintenanceExpenses;
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
            return true;
        }

        private void Production()
        {
            if (d_amountResources.ContainsKey(TypeProductionResources.TypeResource.CocaLeaves))
            {
                if (d_amountResources[TypeProductionResources.TypeResource.CocaLeaves] >= _productConversionStep)
                {
                    d_amountResources[TypeProductionResources.TypeResource.CocaLeaves] -= _productConversionStep;
                    d_amountResources[_typeProductionResource] += _productionPerformance;
                }
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
