using Resources;
using System.Collections.Generic;
using Config.Building;
using Building.Additional;


namespace Building.Stock
{
    public sealed class BuildingStock : IBuilding, IBuildingPurchased, IBuildingJobStatus, ISpending, IEnergyConsumption
    {
        private IBuildingMonitorEnergy _IbuildingMonitorEnergy = new BuildingMonitorEnergy();

        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        Dictionary<TypeProductionResources.TypeResource, float> IBuilding.d_amountResources
        {
            get => d_amountResources; set => d_amountResources = value;
        }

        private double _maintenanceExpenses;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingStock(in ConfigBuildingStockEditor config)
        {
            _maintenanceExpenses = config.maintenanceExpenses;
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

        public void Spending()
        {
            SpendingToObjects.SendNewExpense(_maintenanceExpenses);
        }
    }
}
