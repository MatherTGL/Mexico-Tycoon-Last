using System.Collections.Generic;
using Config.Building;
using Resources;
using UnityEngine;

namespace Building.Border
{
    public sealed class BuildingBorder : IBuilding
    {
        private IBuildingBorderMarket _IbuildingBorderMarket;

        private ConfigBuildingBorderEditor _config;


        #region (Not used!)

        /*
        A placeholder for implementing an interface so that you don't have to add
        a separate interface. Should not and will not be used in code.
         */
        Dictionary<TypeProductionResources.TypeResource, double> IBuilding.amountResources
        {
            get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException();
        }
        Dictionary<TypeProductionResources.TypeResource, uint> IBuilding.stockCapacity
        {
            get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException();
        }

        uint[] IBuilding.localCapacityProduction => throw new System.NotImplementedException();

        #endregion


        public BuildingBorder(in ScriptableObject config)
        {
            _config = (ConfigBuildingBorderEditor)config;
            _IbuildingBorderMarket = new BuildingBorderMarket(_config);
        }

        void IBuilding.ConstantUpdatingInfo() { }

        float IBuilding.GetResources(in float transportCapacity,
                                     in TypeProductionResources.TypeResource typeResource)
        {
            if (_IbuildingBorderMarket.CalculateBuyCost(typeResource, transportCapacity))
                return transportCapacity;
            else
                return 0.0f;
        }

        bool IBuilding.SetResources(in float quantityResource,
                                    in TypeProductionResources.TypeResource typeResource)
        {
            if (quantityResource > 0)
            {
                _IbuildingBorderMarket.SellResources(typeResource, quantityResource);
                return true;
            }
            else { return false; }
        }

        void IBuilding.InitDictionaries()
        {
            throw new System.NotImplementedException();
        }
    }
}
