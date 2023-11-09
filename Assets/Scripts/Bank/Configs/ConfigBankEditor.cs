using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Bank
{
    [CreateAssetMenu(fileName = "DefaultBankConfig", menuName = "Config/Bank/Create New", order = 50)]
    public sealed class ConfigBankEditor : ScriptableObject
    {
        [SerializeField, MinValue(0), MaxValue(1_000_000_000_000)]
        private double _averageAffordableCredit = 100_000;
        public double averageAffordableCredit => _averageAffordableCredit;
    }
}
