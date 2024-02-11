using System;
using System.Collections.Generic;
using static Resources.TypeProductionResources;

namespace Building.City.Market
{
    public sealed class ProductDemand : IProductDemand
    {
        private IPotentialConsumers _IpotentialConsumers;

        private readonly Dictionary<TypeResource, double> d_productDemandInKG = new();


        public ProductDemand(in IPotentialConsumers IpotentialConsumers)
        {
            _IpotentialConsumers = IpotentialConsumers;

            foreach (TypeResource resource in Enum.GetValues(typeof(TypeResource)))
                d_productDemandInKG.TryAdd(resource, 0);
        }

        double IProductDemand.FormAndGet(in TypeResource typeResource, in float percentageUsers)
        {
            if (d_productDemandInKG.ContainsKey(typeResource))
            {
                var countConsumers = _IpotentialConsumers.GetCount();
                double demandInKg = countConsumers * percentageUsers;
                d_productDemandInKG[typeResource] = demandInKg;
                return d_productDemandInKG[typeResource];
            }
            else
            {
                return 0;
            }
        }
    }
}
