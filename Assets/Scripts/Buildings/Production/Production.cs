using System.Collections.Generic;
using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building.Additional.Production
{
    public sealed class Production : IProduction
    {
        private readonly IProductionBuilding _IproductionBuilding;

        private readonly CalculateEfficiencyAdditionalEmployees _calculateEfficiencyAdditionalEmployees = new();

        private TypeResource _resource => _IproductionBuilding.typeProductionResource;

        private readonly Dictionary<TypeResource, ushort> d_currentCultivatedProducts = new();

        private uint[] localCapacityProduction => _IproductionBuilding.localCapacityProduction;

        private float _currentPercentageOfMaturity;

        private bool _isCurrentlyInProduction;


        public Production(in IProductionBuilding iproductionBuilding)
        {
            _IproductionBuilding = iproductionBuilding;
            d_currentCultivatedProducts.Add(_resource, 0);
        }

        void IProduction.Production()
        {
            if (_IproductionBuilding.amountResources[_resource] < localCapacityProduction[(int)_resource])
            {
                if (_isCurrentlyInProduction == false && IsQuantityRequiredRawMaterials() == false)
                    return;

                _isCurrentlyInProduction = true;
                d_currentCultivatedProducts[_resource] = CalculateAndGetProductionPerformance();

                if (_currentPercentageOfMaturity < _IproductionBuilding.harvestRipeningTime)
                    _currentPercentageOfMaturity++;
                else
                {
                    _IproductionBuilding.amountResources[_resource] += d_currentCultivatedProducts[_resource];

                    d_currentCultivatedProducts[_resource] = 0;
                    _currentPercentageOfMaturity = 0;
                    _isCurrentlyInProduction = false;
                }
            }
        }

        private bool IsQuantityRequiredRawMaterials()
        {
            foreach (var typeDrug in _IproductionBuilding.requiredRawMaterials)
            {
                for (ushort i = 0; i < _IproductionBuilding.quantityRequiredRawMaterials.Count; i++)
                    if (_IproductionBuilding.amountResources[typeDrug] < _IproductionBuilding.quantityRequiredRawMaterials[i])
                        return false;

                _IproductionBuilding.amountResources[typeDrug] -= _IproductionBuilding.quantityRequiredRawMaterials[0];
            }
            return true;
        }

        private ushort CalculateAndGetProductionPerformance()
        {
            var efficiency = _calculateEfficiencyAdditionalEmployees.GetEfficiencyAdditionalEmployees(
                _IproductionBuilding.IobjectsExpensesImplementation, _IproductionBuilding.requiredEmployees, _resource);

            return (ushort)(_IproductionBuilding.defaultProductionPerformance + efficiency);
        }
    }
}
