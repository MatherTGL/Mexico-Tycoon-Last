using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Climate
{
    [CreateAssetMenu(fileName = "ClimateZoneConfig", menuName = "Config/Climate/Zone/Create New", order = 50)]
    public sealed class ConfigClimateZoneEditor : ScriptableObject
    {
        [SerializeField, MinValue(0.01f), MaxValue(0.4f)]
        private float _percentageImpactCostMaintenance;
        public float percentageImpactCostMaintenance => _percentageImpactCostMaintenance;
    }
}
