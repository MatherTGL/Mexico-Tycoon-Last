using Sirenix.OdinInspector;
using UnityEngine;

namespace Config.Employees
{
    [CreateAssetMenu(fileName = "ConfigPossibleEmployeesShop", menuName = "Config/Employee/Shop/Create New", order = 50)]
    public sealed class ConfigPossibleEmployeesInShopEditor : ScriptableObject
    {
        public const byte numberEmployeesOffered = 10;

        [SerializeField, MinValue(1), MaxValue(20)]
        private byte _timeUpdateOffers = 20;
        public byte timeUpdateOffers => _timeUpdateOffers;
    }
}