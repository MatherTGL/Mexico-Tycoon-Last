using UnityEngine;
using Sirenix.OdinInspector;
using SerializableDictionary.Scripts;

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

        [SerializeField, EnumPaging, BoxGroup("Parameters")]
        private TypeClimate _typeClimate;
        public TypeClimate typeClimate => _typeClimate;

        [SerializeField, BoxGroup("Parameters/Seasons"), Space(5)]
        private SerializableDictionary<TypeSeasons, float> d_seasonsImpactExpenses = new();
        public SerializableDictionary<TypeSeasons, float> seasonsImpactExpenses => d_seasonsImpactExpenses;

        [SerializeField, BoxGroup("Parameters"), MinValue(0), Space(5)]
        [InfoBox("Value in seconds!", InfoMessageType.Warning)]
        private byte _seasonLength = 90;
        public byte seasonLength => _seasonLength;

        [SerializeField, MinValue(-70), MaxValue(70), BoxGroup("Parameters")]
        private float _maxTemperature;
        public float maxTemperature => _maxTemperature;

        [SerializeField, MinValue(-70), MaxValue(70), BoxGroup("Parameters")]
        private float _minTemperature;
        public float minTemperature => _minTemperature;

        [SerializeField, MinValue(0.1f), BoxGroup("Parameters")]
        private float _humidityLevel;
        public float humidityLevel => _humidityLevel;

        [SerializeField, MinValue(50), MaxValue(4000), BoxGroup("Parameters")]
        private ushort _amountOfPrecipitation;
        public ushort amountOfPrecipitation => _amountOfPrecipitation;
    }
}
