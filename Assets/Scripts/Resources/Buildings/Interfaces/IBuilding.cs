using System.Collections.Generic;
using DebugCustomSystem;
using Resources;
using UnityEngine;

namespace Building
{
    public interface IBuilding
    {
        Dictionary<TypeProductionResources.TypeResource, float> amountResources { get; set; }

        Dictionary<TypeProductionResources.TypeResource, uint> stockCapacity { get; set; }

        uint[] localCapacityProduction { get; }


        void InitDictionaryStockCapacity()
        {
            foreach (var resource in amountResources.Keys)
                stockCapacity.Add(resource, localCapacityProduction[(int)amountResources[resource]]);
        }

        void ConstantUpdatingInfo();

        float GetResources(in float transportCapacity,
                           in TypeProductionResources.TypeResource typeResource)
        {
            if (amountResources[typeResource] >= transportCapacity)
            {
                amountResources[typeResource] -= transportCapacity;
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green,
                                $"{amountResources[typeResource]}", "Building");
                return transportCapacity;
            }
            else
                return 0;
        }

        bool SetResources(in float quantityResource,
                          in TypeProductionResources.TypeResource typeResource)
        {
            Debug.Log("Enter to SetRes");
            if (stockCapacity.ContainsKey(typeResource))
            {
                Debug.Log("SetRes ContainsKey is true");
                if (amountResources[typeResource] < stockCapacity[typeResource])
                {
                    Debug.Log("amountResources[typeResource] < stockCapacity[typeResource] is true");
                    amountResources[typeResource] += quantityResource;
                    Debug.Log($"{amountResources[typeResource]}");
                    Debug.Log($"{quantityResource}");
                }
            }
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green,
                                $"{amountResources[typeResource]}", "Building");
            return true;
        }
    }
}
