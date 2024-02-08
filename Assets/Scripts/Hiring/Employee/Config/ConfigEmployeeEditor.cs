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

        [SerializeField, EnumPaging]
        private TypeEmployee _typeEmployee;
        public TypeEmployee typeEmployee => _typeEmployee;

        [SerializeField]
        private ushort _paymentPerDay;
        public ushort paymentPerDay => _paymentPerDay;

        [SerializeField]
        private byte _rating;
        public byte rating => _rating;

        [SerializeField]
        private SerializableDictionary<TypeResource, ushort> d_productionEfficiency = new();
        public SerializableDictionary<TypeResource, ushort> productionEfficiencyDictionary => d_productionEfficiency;
    }
}
