using Config.Building;
using System.Collections.Generic;
using Resources;
using DebugCustomSystem;


namespace Building.Farm
{
    public class BuildingFarm : IBuilding
    {
        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        private TypeProductionResources.TypeResource _typeProductionResource;

        private ushort _productionPerformance;

        private uint _localCapacityProduct;

        private bool _isWorked;


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

        (bool confirm, float quantityAmount) IBuilding.GetResources(in float transportCapacity)
        {
            if (d_amountResources[_typeProductionResource] > transportCapacity)
            {
                d_amountResources[_typeProductionResource] -= transportCapacity;
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Ресурсы были отправлены на погрузку машины Farm", "Transport");
                return (true, transportCapacity);
            }
            else
                return (false, 0);
        }

        bool IBuilding.SetResources(in float quantityResource)
        {
            d_amountResources[_typeProductionResource] += quantityResource;
            DebugSystem.Log(d_amountResources[_typeProductionResource], DebugSystem.SelectedColor.Green, "Ресурсы были разгружены на склад Farm", "Transport");
            return true;
        }

        private void Production()
        {
            if (d_amountResources[_typeProductionResource] < _localCapacityProduct)
            {
                d_amountResources[_typeProductionResource] += _productionPerformance;
                DebugSystem.Log(d_amountResources[_typeProductionResource],
                            DebugSystem.SelectedColor.Green, "Farm");
            }
        }

        void IBuilding.ChangeJobStatus(in bool isState)
        {
            _isWorked = isState;
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"Current Job Status: {_isWorked}", "Building", "Farm");
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
