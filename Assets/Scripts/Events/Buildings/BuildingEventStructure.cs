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
    }
}
