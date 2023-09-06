using UnityEngine;
using Resources;
using System.Collections.Generic;
using Config.Building;
using Building.Additional;


namespace Building.Stock
{
    public sealed class BuildingStock : IBuilding, IBuildingPurchased, IBuildingJobStatus
    {
        private Dictionary<TypeProductionResources.TypeResource, float> d_amountResources
            = new Dictionary<TypeProductionResources.TypeResource, float>();

        private bool _isWorked;
        bool IBuildingJobStatus.isWorked { get => _isWorked; set => _isWorked = value; }

        private bool _isBuyed;
        bool IBuildingPurchased.isBuyed { get => _isBuyed; set => _isBuyed = value; }


        public BuildingStock(in ConfigBuildingStockEditor configBuilding)
        {

        }

        void IBuilding.ConstantUpdatingInfo()
        {
            if (_isBuyed && _isWorked)
                Debug.Log("Stock is work");
        }

        float IBuilding.GetResources(in float transportCapacity,
                                     in TypeProductionResources.TypeResource typeResource)
        {
            CheckIncomingDrugType(typeResource);

            if (d_amountResources[typeResource] >= transportCapacity)
            {
                d_amountResources[typeResource] -= transportCapacity;
                Debug.Log($"Stock: {d_amountResources[typeResource]}");
                return transportCapacity;
            }
            else return 0;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            CheckIncomingDrugType(typeResource);

            d_amountResources[typeResource] += quantityResource;
            Debug.Log($"Stock: {d_amountResources[typeResource]}");
            return true;
        }

        private void CheckIncomingDrugType(in TypeProductionResources.TypeResource typeResource)
        {
            if (d_amountResources.ContainsKey(typeResource) is false)
                d_amountResources.Add(typeResource, 0);
        }
    }
}
