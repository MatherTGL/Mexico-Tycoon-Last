using System.Collections.Generic;
using Building.Additional.Crop;
using UnityEngine;
using static Resources.TypeProductionResources;

namespace Building.Additional.Production
{
    public sealed class Production : IProduction, IGetAllResourcesForCropSpoilage
    {
        private readonly IProductionBuilding _IproductionBuilding;

        private readonly ICrop cropSpoilage;

        private readonly CalculateEfficiencyAdditionalEmployees _calculateEfficiencyAdditionalEmployees = new();

        private TypeResource _resource => _IproductionBuilding.typeProductionResource;

        private readonly Dictionary<TypeResource, int> d_currentCultivatedProducts = new();

        private Dictionary<TypeResource, uint> d_localCapacityProduction => _IproductionBuilding.localCapacityProduction;

        Dictionary<TypeResource, double> IGetAllResourcesForCropSpoilage.amountResources
        {
            get => _IproductionBuilding.amountResources;
            set => _IproductionBuilding.amountResources = value;
        }

        private float _currentPercentageOfMaturity;

        private bool _isCurrentlyInProduction;


        public Production(in IProductionBuilding iproductionBuilding)
        {
            _IproductionBuilding = iproductionBuilding;
            d_currentCultivatedProducts.Add(_resource, 0);
            cropSpoilage = new CropSpoilage(iproductionBuilding.configCropSpoilage);
        }

        void IProduction.Production()
        {
            if (d_localCapacityProduction.ContainsKey(_resource) == false)
                throw new System.Exception("d_localCapacityProduction.ContainsKey(_resource) not find");

            if (_IproductionBuilding.amountResources[_resource] < d_localCapacityProduction[_resource])
            {
                if (_isCurrentlyInProduction == false && AreRequiredRawMaterialsAvailable() == false)
                    return;

                _isCurrentlyInProduction = true;
                d_currentCultivatedProducts[_resource] = GetProductionPerformance();

                if (_IproductionBuilding.harvestRipeningTime.ContainsKey(_resource) == false)
                    throw new System.Exception("harvestRipeningTime not containsKey (resource)");

                if (_currentPercentageOfMaturity < _IproductionBuilding.harvestRipeningTime[_resource])
                    _currentPercentageOfMaturity++;
                else
                {
                    _IproductionBuilding.amountResources[_resource] += d_currentCultivatedProducts[_resource];

                    d_currentCultivatedProducts[_resource] = 0;
                    _currentPercentageOfMaturity = 0;
                    _isCurrentlyInProduction = false;
                }
            }
            else
                Debug.Log("Склад здания полный!");

            CropSpoilage();
        }

        private bool AreRequiredRawMaterialsAvailable()
        {
            foreach (TypeResource drugType in _IproductionBuilding.requiredRawMaterials.Keys)
            {
                if (!IsResourceAvailableForDrug(drugType))
                    return false;
            }
            return true;
        }

        private bool IsResourceAvailableForDrug(TypeResource drugType)
        {
            foreach (TypeResource rawMaterial in _IproductionBuilding.requiredRawMaterials[drugType].Dictionary.Keys)
            {
                if (!HasEnoughResource(drugType, rawMaterial))
                    return false;

                DeductResource(drugType, rawMaterial);
            }
            return true;
        }

        private bool HasEnoughResource(TypeResource drugType, TypeResource rawMaterial)
        {
            var currentAmount = _IproductionBuilding.amountResources[drugType];
            var requiredAmount = _IproductionBuilding.requiredRawMaterials[drugType].Dictionary[rawMaterial];

            return currentAmount >= requiredAmount;
        }

        private void DeductResource(TypeResource drugType, TypeResource rawMaterial)
        {
            _IproductionBuilding.amountResources[drugType] -= _IproductionBuilding.requiredRawMaterials[drugType].Dictionary[rawMaterial];
        }

        //TODO нужно добавить качество продукта
        private void CropSpoilage()
            => cropSpoilage.Spoilage(this);

        private int GetProductionPerformance()
        {
            var efficiency = _calculateEfficiencyAdditionalEmployees.GetEfficiencyAdditionalEmployees(
                _IproductionBuilding.IobjectsExpensesImplementation, _IproductionBuilding.requiredEmployees, _resource);

            return _IproductionBuilding.GetBaseProductionPerformance(_resource) + efficiency;
        }
    }
}
