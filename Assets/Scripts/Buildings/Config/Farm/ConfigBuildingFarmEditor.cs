using UnityEngine;
using Sirenix.OdinInspector;
using Resources;
using System.Collections.Generic;
using System;
using System.Linq;
using Config.Country.Climate;
using Events.Buildings;

namespace Config.Building
{
    [CreateAssetMenu(fileName = "BuildingFarmConfig", menuName = "Config/Buildings/Farm/Create New", order = 50)]
    public sealed class ConfigBuildingFarmEditor : ScriptableObject
    {
        [SerializeField, BoxGroup("Raw Materials"), ReadOnly, HideLabel]
        private List<TypeProductionResources.TypeResource> l_requiredRawMaterials = new();
        public List<TypeProductionResources.TypeResource> requiredRawMaterials => l_requiredRawMaterials;

        [SerializeField, BoxGroup("Raw Materials"), ReadOnly, HideLabel]
        private List<float> l_quantityRequiredRawMaterials = new();
        public List<float> quantityRequiredRawMaterials => l_quantityRequiredRawMaterials;

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons]
        private TypeProductionResources.TypeResource _typeProductionResource;
        public TypeProductionResources.TypeResource typeProductionResource => _typeProductionResource;

        public enum TypeFarm { Terrestrial, Underground }

        [SerializeField, BoxGroup("Parameters"), EnumPaging]
        public TypeFarm typeFarm;

        [SerializeField, BoxGroup("Parameters"), HideLabel]
        private List<ConfigClimateZoneEditor.TypeSeasons> l_growingSeasons = new();
        public List<ConfigClimateZoneEditor.TypeSeasons> growingSeasons => l_growingSeasons;

        [SerializeField, BoxGroup("Parameters"), HideLabel]
        private List<BuildingEventStructure> l_activePossibleEvents = new();
        public List<BuildingEventStructure> activePossibleEvents => l_activePossibleEvents;

        [SerializeField, BoxGroup("Parameters"), MinValue(1)]
        private ushort _productionPerformanceStart = 1;
        public ushort productionStartPerformance => _productionPerformanceStart;

        [SerializeField, BoxGroup("Parameters"), MinValue(10)]
        private uint[] _localCapacityProduction;
        public uint[] localCapacityProduction => _localCapacityProduction;

        [SerializeField, BoxGroup("Parameters"), MinValue(1)]
        private float _harvestRipeningTime;
        public float harvestRipeningTime => _harvestRipeningTime;

        [SerializeField, BoxGroup("Parameters"), MinValue(0)]
        private double _costPurchase = 50_000;
        public double costPurchase => _costPurchase;


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
