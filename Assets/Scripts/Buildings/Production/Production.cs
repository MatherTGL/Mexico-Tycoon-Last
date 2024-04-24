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

        private readonly Dictionary<TypeResource, int> d_currentCultivatedProducts = new();

        private Dictionary<TypeResource, uint> d_localCapacityProduction => _IproductionBuilding.localCapacityProduction;

        private float _currentPercentageOfMaturity;

        private bool _isCurrentlyInProduction;


        public Production(in IProductionBuilding iproductionBuilding)
        {
            _IproductionBuilding = iproductionBuilding;
            d_currentCultivatedProducts.Add(_resource, 0);
        }

        void IProduction.Production()
        {
            if (d_localCapacityProduction.ContainsKey(_resource) == false)
                throw new System.Exception("d_localCapacityProduction.ContainsKey(_resource) not find");

            if (_IproductionBuilding.amountResources[_resource] < d_localCapacityProduction[_resource])
            {
                if (_isCurrentlyInProduction == false && IsQuantityRequiredRawMaterials() == false)
                    return;
                Debug.Log("production is get true & continue");

                _isCurrentlyInProduction = true;
                d_currentCultivatedProducts[_resource] = GetProductionPerformance();

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
            foreach (TypeResource typeDrug in _IproductionBuilding.requiredRawMaterials.Keys)
            {
                foreach (TypeResource resource in _IproductionBuilding.requiredRawMaterials[typeDrug].Dictionary.Keys)
                {
                    Debug.Log(@$"typeDrug: {typeDrug} typeRes: {resource} curr: {_IproductionBuilding.amountResources[typeDrug]} 
                        req: {_IproductionBuilding.requiredRawMaterials[typeDrug].Dictionary[resource]}");

                    if (_IproductionBuilding.amountResources[typeDrug] < _IproductionBuilding.requiredRawMaterials[typeDrug].Dictionary[resource])
                        return false;

                    _IproductionBuilding.amountResources[typeDrug] -= _IproductionBuilding.requiredRawMaterials[typeDrug].Dictionary[resource];
                    Debug.Log($"current amount res: {_IproductionBuilding.amountResources[typeDrug]}");
                }
            }
            return true;
        }

        private int GetProductionPerformance()
        {
            var efficiency = _calculateEfficiencyAdditionalEmployees.GetEfficiencyAdditionalEmployees(
                _IproductionBuilding.IobjectsExpensesImplementation, _IproductionBuilding.requiredEmployees, _resource);

            return _IproductionBuilding.GetBaseProductionPerformance(_resource) + efficiency;
        }
    }
}
