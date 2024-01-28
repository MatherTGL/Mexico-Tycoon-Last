using System;
using System.Collections.Generic;
using DebugCustomSystem;
using Resources;

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
            DebugSystem.Log($"{this} - Request GetRes (amount {transportCapacity}) & current amount res ({typeResource}): {amountResources[typeResource]}",
                DebugSystem.SelectedColor.Blue, tag: "Building");

            if (amountResources[typeResource] >= transportCapacity)
            {
                amountResources[typeResource] -= transportCapacity;
                DebugSystem.Log($"{this} - current amount res ({typeResource}): {amountResources[typeResource]}",
                    DebugSystem.SelectedColor.Blue, tag: "Building");

                return transportCapacity;
            }
            else
                return 0;
        }

        bool SetResources(in float quantityResource,
                          in TypeProductionResources.TypeResource typeResource)
        {
            DebugSystem.Log($"{this} - Request SetRes (amount {quantityResource}) & current amount res ({typeResource}): {amountResources[typeResource]}",
                DebugSystem.SelectedColor.Blue, tag: "Building");

            if (stockCapacity.TryGetValue(typeResource, out uint capacity))
            {
                if (amountResources[typeResource] < capacity)
                {
                    amountResources[typeResource] += quantityResource;
                    return true;
                }
            }

            DebugSystem.Log($"{this} - current amount res ({typeResource}): {amountResources[typeResource]}",
                    DebugSystem.SelectedColor.Blue, tag: "Building");

            return false;
        }
    }
}
