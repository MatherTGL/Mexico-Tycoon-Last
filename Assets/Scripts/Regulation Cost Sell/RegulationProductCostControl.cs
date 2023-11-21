using UnityEngine;
using Sirenix.OdinInspector;
using Building;
using Resources;

namespace Regulation
{
    public sealed class RegulationProductCostControl : MonoBehaviour, IRegulationProductCost
    {
        [ShowInInspector, ReadOnly]
        private IRegulationBuilding _IregulationBuilding;


        void IRegulationProductCost.Init(in IBuilding Ibuilding)
        {
            _IregulationBuilding = Ibuilding as IRegulationBuilding;
        }

        [Button("Change Cost"), BoxGroup("Selling Price Product")]
        private void ChangeSellingPriceProduct(in TypeProductionResources.TypeResource typeResource,
                                               in uint cost)
        {
            var cellIndex = _IregulationBuilding.cellIndexRegulationCostSale;
            _IregulationBuilding.IregulationCostSale.SetResourcesCosts(cellIndex, typeResource, cost);
        }
    }
}
