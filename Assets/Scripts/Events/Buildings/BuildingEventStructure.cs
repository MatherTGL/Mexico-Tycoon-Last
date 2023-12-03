using UnityEngine;
using Sirenix.OdinInspector;
using System;
using static Resources.TypeProductionResources;
using System.Collections.Generic;

namespace Events.Buildings
{
    [Serializable]
    public sealed class BuildingEventStructure
    {
        [ShowInInspector, ShowIf("@_isUseAccessResourcesAmount")]
        private Dictionary<TypeResource, double> d_typeResources = new();
        public Dictionary<TypeResource, double> typeResources => d_typeResources;

        [SerializeField, MinValue(0.0f)]
        private float _percentageImpact;
        public float percentageImpact => _percentageImpact;

        [SerializeField, MinValue(0.0f), DisableIf("@_isTemporary == false")]
        private float _duration;
        public float duration => _duration;

        [SerializeField, ToggleLeft]
        private bool _isTemporary;
        public bool isTemporary => _isTemporary;

        [SerializeField, ToggleLeft]
        private bool _isUseAccessResourcesAmount;
        public bool isUseAccessResourcesAmount => _isUseAccessResourcesAmount;
    }
}
