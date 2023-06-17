using UnityEngine;


namespace Fabric
{
    public sealed class FabricProduction : IFabricProduction
    {
        private float _productQuality;
        private float _productivityKgPerDay;
        private float _productInStock;
        private float _maxCapacityStock;


        public FabricProduction(float productQuality,
                                float productivityKgPerDay,
                                float productInStock,
                                float maxCapacityStock)
        {
            SetOutputParameters(productQuality, productivityKgPerDay, productInStock, maxCapacityStock);
        }

        void IFabricProduction.UpdateProductionParameters(float productQuality,
                                                          float productivityKgPerDay,
                                                          float productInStock,
                                                          float maxCapacityStock)
        {
            SetOutputParameters(productQuality, productivityKgPerDay, productInStock, maxCapacityStock);
        }

        private void SetOutputParameters(float productQuality,
                                         float productivityKgPerDay,
                                         float productInStock,
                                         float maxCapacityStock)
        {
            _productQuality = productQuality;
            _productivityKgPerDay = productivityKgPerDay;
            _productInStock = productInStock;
            _maxCapacityStock = maxCapacityStock;
            Debug.Log($"{_productQuality} / {_productivityKgPerDay} / {_productInStock} / {_maxCapacityStock}");
        }

        float IFabricProduction.GetProductInStock() { return _productInStock; }

        void IFabricProduction.ProductionProduct()
        {
            if (_productInStock < _maxCapacityStock)
            {
                _productInStock += _productivityKgPerDay;
                Debug.Log(_productInStock);
            }
        }
    }
}
