using System;
using System.Collections.Generic;
using Resources;
using UnityEngine;

namespace Building
{
    public interface IBuilding
    {
        Dictionary<TypeProductionResources.TypeResource, double> amountResources { get; set; }

        Dictionary<TypeProductionResources.TypeResource, uint> stockCapacity { get; set; }

        uint[] localCapacityProduction { get; }


        void InitDictionaries()
        {
            foreach (TypeProductionResources.TypeResource typeDrug
                        in Enum.GetValues(typeof(TypeProductionResources.TypeResource)))
            {
                if (amountResources.ContainsKey(typeDrug) == false)
                    amountResources.Add(typeDrug, 0);
            }

            foreach (var resource in amountResources.Keys)
                stockCapacity.Add(resource, localCapacityProduction[(int)amountResources[resource]]);
        }

        void ConstantUpdatingInfo();

        float GetResources(in float transportCapacity,
                           in TypeProductionResources.TypeResource typeResource)
        {
            Debug.Log($"GetRes: {amountResources[typeResource]}");
            if (amountResources[typeResource] >= transportCapacity)
            {
                amountResources[typeResource] -= transportCapacity;
                return transportCapacity;
            }
            else
                return 0;
        }

        bool SetResources(in float quantityResource,
                          in TypeProductionResources.TypeResource typeResource)
        {
            Debug.Log($"SetRes: {amountResources[typeResource]}");

            if (stockCapacity.TryGetValue(typeResource, out uint capacity))
                if (amountResources[typeResource] < capacity)
                    amountResources[typeResource] += quantityResource;

            return true;
        }
    }
}
