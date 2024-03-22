using UnityEngine;
using Sirenix.OdinInspector;
using static Resources.TypeProductionResources;
using SerializableDictionary.Scripts;
using System.Collections.Generic;

namespace Config.Employees
{
    [CreateAssetMenu(fileName = "ConfigEmployeeDefault", menuName = "Config/Employee/Create New", order = 50)]
    public sealed class ConfigEmployeeEditor : ScriptableObject
    {
        public enum TypeEmployee : byte
        {
            Scientist, Guard, Grover
        }

        [SerializeField, EnumPaging, BoxGroup("Parameters")]
        private TypeEmployee _typeEmployee;
        public TypeEmployee typeEmployee => _typeEmployee;

        [SerializeField, BoxGroup("Parameters")]
        private SerializableDictionary<byte, float> d_costForEachLevels = new();
        public Dictionary<byte, float> costForEachLevels => d_costForEachLevels.Dictionary;

        [SerializeField, BoxGroup("Parameters")]
        private byte _experienceForLevelUp = 100;
        public byte experienceForLevelUp => _experienceForLevelUp;

        [SerializeField, BoxGroup("Parameters/Payment")]
        private ushort _minPaymentPerDay;
        public ushort minPaymentPerDay => _minPaymentPerDay;

        [SerializeField, BoxGroup("Parameters/Payment"), MinValue(5), MaxValue(50)]
        private ushort _maxDeviationFromBasePay;
        public ushort maxDeviationFromBasePay => _maxDeviationFromBasePay;

        [SerializeField, BoxGroup("Parameters/Rating"), MaxValue("@_maxRating")]
        private byte _minRating = 1;
        public byte minRating => _minRating;

        [SerializeField, BoxGroup("Parameters/Rating"), MinValue("@_minRating")]
        private byte _maxRating = 10;
        public byte maxRating => _maxRating;

        [SerializeField, BoxGroup("Parameters")]
        private SerializableDictionary<TypeResource, int> d_productionEfficiency = new();
        public SerializableDictionary<TypeResource, int> productionEfficiencyDictionary => d_productionEfficiency;

        [SerializeField, BoxGroup("Parameters"), MinValue(10), MaxValue("@_maxEfficiency")]
        private byte _minEfficiency = 10;
        public byte minEfficiency => _minEfficiency;

        [SerializeField, BoxGroup("Parameters"), MinValue("@_minEfficiency"), MaxValue(100)]
        private byte _maxEfficiency = 100;
        public byte maxEfficiency => _maxEfficiency;

        [SerializeField, BoxGroup("Parameters"), MinValue(1), MaxValue(10)]
        private byte _rateDeclineEfficiency = 4;
        public byte rateDeclineEfficiency => _rateDeclineEfficiency;
    }
}
