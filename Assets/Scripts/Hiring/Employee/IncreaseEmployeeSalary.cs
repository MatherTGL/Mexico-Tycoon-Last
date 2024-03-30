using Config.Employees;
using UnityEngine;

namespace Hire.Employee
{
    public sealed class IncreaseEmployeeSalary
    {
        private ConfigEmployeeEditor _config;

        private double _paymentCostPerDay;
        public double paymentCostPerDay => _paymentCostPerDay;

        private double _basePayment;

        private byte _currentLevel;

        private float _currentExperience;


        public void Init(in ConfigEmployeeEditor config, in double defaultPaymentCost)
        {
            _config = config;
            _paymentCostPerDay = defaultPaymentCost;
            Debug.Log($"_paymentCostPerDay in hired employee: {_paymentCostPerDay}");
        }

        //TODO completed
        public void Update()
        {
            if (_currentExperience < _config.experienceForLevelUp)
                _currentExperience++;
            else
            if (_currentLevel < _config.costForEachLevels.Count)
                LevelUp();
            Debug.Log($"_currentExperience: {_currentExperience}");
        }

        private void LevelUp()
        {
            _currentLevel++;
            _paymentCostPerDay = _basePayment * _config.costForEachLevels[_currentLevel];
            _currentExperience = 0;
            Debug.Log($"current level employee salary: {_currentLevel} / p: {_paymentCostPerDay}");
        }
    }
}