using UnityEngine;


namespace Fabric
{
    public sealed class FabricProduction : IFabricProduction
    {
        private float _productInStock;


        public FabricProduction(ref float productInStock)
        {
            productInStock = _productInStock;
        }


        float IFabricProduction.GetProductInStock() { return _productInStock; }

        void IFabricProduction.ProductionProduct(in float productivityKgPerDay, in float maxCapacityStock)
        {
            if (_productInStock < maxCapacityStock)
            {
                _productInStock += productivityKgPerDay;
            }
        }
    }
}
