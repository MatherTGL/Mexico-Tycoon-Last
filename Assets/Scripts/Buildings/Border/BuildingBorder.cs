using Resources;
using UnityEngine;


namespace Building.Border
{
    public sealed class BuildingBorder : IBuilding
    {
        private IBuildingBorderMarket _IbuildingBorderMarket;


        public BuildingBorder()
        {
            _IbuildingBorderMarket = new BuildingBorderMarket();
        }

        void IBuilding.ConstantUpdatingInfo()
        {
            throw new System.NotImplementedException();
        }

        float IBuilding.GetResources(in float transportCapacity, in TypeProductionResources.TypeResource typeResource)
        {
            if (_IbuildingBorderMarket.CheckResourceInSale(typeResource))
            {
                Debug.Log("BuildingBorder|GetResources");
                return transportCapacity;
            }
            else
                return 0.0f;
        }

        bool IBuilding.SetResources(in float quantityResource, in TypeProductionResources.TypeResource typeResource)
        {
            if (quantityResource > 0)
                return true;
            else
                return false;
        }
    }
}
