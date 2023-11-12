using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Country.Climate
{
    [CreateAssetMenu(fileName = "ClimateZoneConfig", menuName = "Config/Country/Climate/Zone/Create New", order = 50)]
    public sealed class ConfigClimateZoneEditor : ScriptableObject
    {
        public enum TypeClimate : byte
        {
            Polar, Subpolar, Temperate, Subtropical, Tropical, Equatorials
        }

        public enum TypeSeasons : byte { Summer, Autumn, Winter, Spring }

        [SerializeField, EnumPaging]
        private TypeClimate _typeClimate;
        public TypeClimate typeClimate => _typeClimate;

        [SerializeField, MinValue(0.01f), MaxValue(0.4f)]
        private float _percentageImpactCostMaintenance;
        public float percentageImpactCostMaintenance => _percentageImpactCostMaintenance;

        [SerializeField]
        private byte _seasonLength = 90;
        public byte seasonLength => _seasonLength;

        [SerializeField, MinValue(-70), MaxValue(70)]
        private float _maxTemperature;
        public float maxTemperature => _maxTemperature;

        [SerializeField, MinValue(-70), MaxValue(70)]
        private float _minTemperature;
        public float minTemperature => _minTemperature;

        [SerializeField, MinValue(0.1f)]
        private float _humidityLevel;
        public float humidityLevel => _humidityLevel;

        [SerializeField, MinValue(50), MaxValue(4000)]
        private ushort _amountOfPrecipitation;
        public ushort amountOfPrecipitation => _amountOfPrecipitation;
    }
}
