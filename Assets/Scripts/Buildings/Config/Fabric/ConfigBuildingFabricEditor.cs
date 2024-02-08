using UnityEngine;
using Sirenix.OdinInspector;
using Resources;
using System.Collections.Generic;
using System;
using System.Linq;
using Config.Building.Events;
using SerializableDictionary.Scripts;
using static Config.Employees.ConfigEmployeeEditor;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFabricConfig", menuName = "Config/Buildings/Fabric/Create New", order = 50)]
    public sealed class ConfigBuildingFabricEditor : ScriptableObject
    {
        [SerializeField, Required, BoxGroup("Configs")]
        private ConfigBuildingsEventsEditor _configBuildingsEvents;
        public ConfigBuildingsEventsEditor configBuildingsEvents => _configBuildingsEvents;

        [SerializeField, BoxGroup("Raw Materials"), ReadOnly, HideLabel]
        private List<TypeProductionResources.TypeResource> l_requiredRawMaterials = new();
        public List<TypeProductionResources.TypeResource> requiredRawMaterials => l_requiredRawMaterials;

        [SerializeField, BoxGroup("Raw Materials"), ReadOnly, HideLabel]
        private List<float> l_quantityRequiredRawMaterials = new();
        public List<float> quantityRequiredRawMaterials => l_quantityRequiredRawMaterials;

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons]
        private TypeProductionResources.TypeResource _typeProductionResource;
        public TypeProductionResources.TypeResource typeProductionResource => _typeProductionResource;

        [SerializeField, BoxGroup("Parameters")]
        private ushort _productionPerformance = 1;
        public ushort productionPerformance => _productionPerformance;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint[] _localCapacityProduction;
        public uint[] localCapacityProduction => _localCapacityProduction;

        [SerializeField, BoxGroup("Parameters"), MinValue(1)]
        private float _harvestRipeningTime = 3;
        public float harvestRipeningTime => _harvestRipeningTime;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        private double _costPurchase = 50_000;
        public double costPurchase => _costPurchase;

        [SerializeField, BoxGroup("Parameters/Employees")]
        [Tooltip("Required employees for the operation of the building, as well as their number")]
        private SerializableDictionary<TypeEmployee, byte> d_requiredEmployees = new();
        public SerializableDictionary<TypeEmployee, byte> requiredEmployees => d_requiredEmployees;


#if UNITY_EDITOR
        [Button("Add New"), BoxGroup("Raw Materials"), HorizontalGroup("Raw Materials/Hor")]
        private void AddNewRawMaterial(in TypeProductionResources.TypeResource typeResource,
                                       in float quantityRawMaterial = 1)
        {
            if (l_requiredRawMaterials.Contains(typeResource) == false && quantityRawMaterial > 0)
            {
                l_requiredRawMaterials.Add(typeResource);
                l_quantityRequiredRawMaterials.Add(quantityRawMaterial);
            }
        }

        [Button("Remove"), BoxGroup("Raw Materials"), HorizontalGroup("Raw Materials/Hor")]
        private void RemoveRawMaterial(in TypeProductionResources.TypeResource typeResource)
        {
            if (l_requiredRawMaterials.Contains(typeResource) == true)
            {
                int index = l_requiredRawMaterials.IndexOf(typeResource);
                l_requiredRawMaterials.RemoveAt(index);
                l_quantityRequiredRawMaterials.RemoveAt(index);
            }
        }

        [Button("Update Info Capacity")]
        private void UpdateInfoCapacity()
        {
            int count = Enum.GetNames(typeof(TypeProductionResources.TypeResource)).ToArray().Length;
            if (_localCapacityProduction.Length < count)
                _localCapacityProduction = new uint[count];
        }
#endif
    }
}
