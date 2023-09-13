using UnityEngine;
using Sirenix.OdinInspector;
using Resources;
using System.Collections.Generic;


namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFarmConfig", menuName = "Config/Buildings/Farm/Create New", order = 50)]
    public sealed class ConfigBuildingFarmEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Raw Materials"), ReadOnly, HideLabel]
        private List<TypeProductionResources.TypeResource> _requiredRawMaterials
            = new List<TypeProductionResources.TypeResource>();
        public List<TypeProductionResources.TypeResource> requiredRawMaterials => _requiredRawMaterials;

        [SerializeField, BoxGroup("Raw Materials"), ReadOnly, HideLabel]
        private List<float> _quantityRawMaterials = new List<float>();
        public List<float> quantityRawMaterials => _quantityRawMaterials;

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons]
        private TypeProductionResources.TypeResource _typeProductionResource;
        public TypeProductionResources.TypeResource typeProductionResource => _typeProductionResource;

        [SerializeField, BoxGroup("Parameters"), MinValue(1)]
        private ushort _productionPerformanceStart = 1;
        public ushort productionStartPerformance => _productionPerformanceStart;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint _localCapacityProduction;
        public uint localCapacityProduction => _localCapacityProduction;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private double _maintenanceExpenses = 10;
        public double maintenanceExpenses => _maintenanceExpenses;


#if UNITY_EDITOR
        [Button("Add New"), BoxGroup("Raw Materials"), HorizontalGroup("Raw Materials/Hor")]
        private void AddNewRawMaterial(in TypeProductionResources.TypeResource typeResource,
                                       in float quantityRawMaterial = 1)
        {
            if (_requiredRawMaterials.Contains(typeResource) == false && quantityRawMaterial > 0)
            {
                _requiredRawMaterials.Add(typeResource);
                _quantityRawMaterials.Add(quantityRawMaterial);
            }
        }

        [Button("Remove"), BoxGroup("Raw Materials"), HorizontalGroup("Raw Materials/Hor")]
        private void RemoveRawMaterial(in TypeProductionResources.TypeResource typeResource)
        {
            if (_requiredRawMaterials.Contains(typeResource) == true)
            {
                int index = _requiredRawMaterials.IndexOf(typeResource);
                _requiredRawMaterials.RemoveAt(index);
                _quantityRawMaterials.RemoveAt(index);
            }
        }
#endif
    }
}
