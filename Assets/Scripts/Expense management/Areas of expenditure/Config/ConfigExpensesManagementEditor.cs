using UnityEngine;

namespace Config.Expenses
{
    [CreateAssetMenu(fileName = "ConfigExpensesManagement", menuName = "Config/Expenses/Create New", order = 50)]
    public sealed class ConfigExpensesManagementEditor : ScriptableObject
    {
        [SerializeField]
        private double _expensesOnProduction;
        public double expensesOnProduction => _expensesOnProduction;

        [SerializeField]
        private double _expensesOnWater;
        public double expensesOnWater => _expensesOnWater;

        [SerializeField]
        private double _expensesOnSecurity;
        public double expensesOnSecurity => _expensesOnSecurity;
    }
}
