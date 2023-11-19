using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Regulation
{
    public sealed class RegulationCostSaleControl : MonoBehaviour, IRegulationCostSale
    {
        [ShowInInspector, ReadOnly]
        private List<uint[]> l_resourceCostSell = new();


        private RegulationCostSaleControl() { }

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
    }
}
