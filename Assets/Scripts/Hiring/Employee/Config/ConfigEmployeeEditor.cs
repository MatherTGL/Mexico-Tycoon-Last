using UnityEngine;
using Sirenix.OdinInspector;
using static Resources.TypeProductionResources;
using SerializableDictionary.Scripts;

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
        private SerializableDictionary<TypeResource, ushort> d_productionEfficiency = new();
        public SerializableDictionary<TypeResource, ushort> productionEfficiencyDictionary => d_productionEfficiency;
    }
}
