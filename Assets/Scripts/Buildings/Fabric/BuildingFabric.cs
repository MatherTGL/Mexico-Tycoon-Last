using System.Collections.Generic;
using Building.Additional;
using Config.Building;
using Resources;
using UnityEngine;


namespace Building.Fabric
{
    public sealed class BuildingFabric : IBuilding, IBuildingPurchased, IBuildingJobStatus
    {
        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        private TypeProductionResources.TypeResource _typeProductionResource;

        private ushort _productionPerformance;

        private ushort _productConversionStep;

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingFabric(in ConfigBuildingFabricEditor configBuilding)
        {
            _typeProductionResource = TypeProductionResources.TypeResource.Cocaine;
            _productionPerformance = configBuilding.productionPerformance;
            _productConversionStep = configBuilding.productConversionStep;

            d_amountResources.Add(_typeProductionResource, 0);
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
                Production();
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
            return true;
        }

        private void Production()
        {
            if (d_amountResources.ContainsKey(TypeProductionResources.TypeResource.CocaLeaves))
            {
                if (d_amountResources[TypeProductionResources.TypeResource.CocaLeaves] > _productConversionStep) //!
                {
                    d_amountResources[TypeProductionResources.TypeResource.CocaLeaves] -= _productConversionStep;
                    d_amountResources[_typeProductionResource] += _productionPerformance;
                }
            }
            else
                CheckIncomingDrugType(TypeProductionResources.TypeResource.CocaLeaves);
        }

        private void CheckIncomingDrugType(in TypeProductionResources.TypeResource typeResource)
        {
            if (d_amountResources.ContainsKey(typeResource) is false)
                d_amountResources.Add(typeResource, 0);
        }
    }
}
