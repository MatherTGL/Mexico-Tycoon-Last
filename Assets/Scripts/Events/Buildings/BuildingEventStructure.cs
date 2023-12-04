using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Events.Buildings
{
    [Serializable]
    public sealed class BuildingEventStructure
    {
        [SerializeField, EnumPaging]
        private BuildingEventTypes _typeEvent;
        public BuildingEventTypes typeEvent => _typeEvent;

        [SerializeField, Required]
        private ScriptableObject _config;
        public ScriptableObject config => _config;

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
