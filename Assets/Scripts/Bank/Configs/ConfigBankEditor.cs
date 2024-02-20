using UnityEngine;
using Sirenix.OdinInspector;

namespace Config.Bank
{
    [CreateAssetMenu(fileName = "DefaultBankConfig", menuName = "Config/Bank/Create New", order = 50)]
    public sealed class ConfigBankEditor : ScriptableObject
    {
        [SerializeField, MinValue(0), MaxValue(1_000_000_000_000)]
        private double _affordableCredit = 100_000;
        public double affordableCredit => _affordableCredit;

        [SerializeField, MinValue(1), MaxValue(40)]
        private float _loanInterest = 4;
        public float loanInterest => _loanInterest;

        [SerializeField, MinValue(0.01f), MaxValue(0.50f)]
        [InfoBox("Add every time step", InfoMessageType.Warning)]
        private float _interestOnTheDeposit = 0.02f;
        public float interestOnTheDeposit => _interestOnTheDeposit;

        [SerializeField, MinValue(10_000), MaxValue(double.MaxValue)]
        private double _maxDepositSum = 500_000_000;
        public double maxDepositSum => _maxDepositSum;
    }
}
