using System;
using System.Collections.Generic;
using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building
{
    public interface IBuilding
    {
        Dictionary<TypeResource, double> amountResources { get; set; }

        Dictionary<TypeResource, uint> stockCapacity { get; set; }

        Dictionary<TypeResource, uint> localCapacityProduction { get; }


        void InitDictionaries()
        {
            foreach (TypeResource typeDrug in Enum.GetValues(typeof(TypeResource)))
                if (amountResources.ContainsKey(typeDrug) == false)
                    amountResources.Add(typeDrug, 0);

            foreach (var resource in localCapacityProduction.Keys)
                stockCapacity.Add(resource, localCapacityProduction[resource]);
        }

        void ConstantUpdatingInfo();

        float GetResources(in float transportCapacity, in TypeResource typeResource)
        {
            if (amountResources[typeResource] >= transportCapacity)
            {
                amountResources[typeResource] -= transportCapacity;
                return transportCapacity;
            }
            else
                return 0;
        }

        bool IsSetResources(in float quantityResource, in TypeResource typeResource)
        {
            if (stockCapacity.TryGetValue(typeResource, out uint capacity))
            {
                if (amountResources[typeResource] < capacity)
                {
                    amountResources[typeResource] += quantityResource;
                    return true;
                }
            }
            Debug.Log("Здание не принимает данный ресурс");
            return false;
        }
    }
}
