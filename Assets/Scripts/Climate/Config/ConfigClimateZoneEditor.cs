using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Climate
{
    [CreateAssetMenu(fileName = "ClimateZoneConfig", menuName = "Config/Climate/Zone/Create New", order = 50)]
    public sealed class ConfigClimateZoneEditor : ScriptableObject
    {
        public enum TypeClimate : byte
        {
            Polar, Subpolar, Temperate, Subtropical, Tropical, Equatorials
        }

        [SerializeField, EnumPaging]
        private TypeClimate _typeClimate;
        public TypeClimate typeClimate => _typeClimate;

        [SerializeField, MinValue(0.01f), MaxValue(0.4f)]
        private float _percentageImpactCostMaintenance;
        public float percentageImpactCostMaintenance => _percentageImpactCostMaintenance;
    }
}
