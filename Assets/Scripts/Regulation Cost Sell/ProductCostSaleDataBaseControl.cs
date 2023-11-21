using System.Collections.Generic;
using Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Regulation
{
    public sealed class ProductCostSaleDataBaseControl : MonoBehaviour, IRegulationCostSale
    {
        [ShowInInspector, ReadOnly]
        private List<uint[]> l_resourceCostSell = new();


        private ProductCostSaleDataBaseControl() { }

        int IRegulationCostSale.Registration(in uint[] resourceCosts)
        {
            l_resourceCostSell.Add(resourceCosts);
            return l_resourceCostSell.Count - 1;
        }

        uint[] IRegulationCostSale.GetResourceCosts(in int cellIndex)
        {
            //? Potential Bug
            if (l_resourceCostSell.IsNotEmpty(cellIndex))
                return l_resourceCostSell[cellIndex];
            return l_resourceCostSell[0];
        }

        void IRegulationCostSale.SetResourcesCosts(in int cellIndex, in TypeProductionResources.TypeResource typeResource,
                                                   in uint newCost)
        {
            l_resourceCostSell[cellIndex][(int)typeResource] = newCost;
            Debug.Log(l_resourceCostSell[cellIndex][(int)typeResource]);
        }
    }
}
