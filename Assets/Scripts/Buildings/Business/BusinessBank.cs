using Config.City.Business;
using Data;
using UnityEngine;


namespace City.Business
{
    public sealed class BusinessBank : IBuisinessBuilding
    {
        private double _income;
        double IBuisinessBuilding.income { get => _income; }

        private double _consumption;
        double IBuisinessBuilding.consumption { get => _consumption; }

        private byte _occupiedNumberSlots = 3;
        byte IBuisinessBuilding.occupiedNumberSlots { get => _occupiedNumberSlots; }

        private double _amountMoneyLaunderedPerYear;
        double IBuisinessBuilding.amountMoneyLaunderedPerYear { get => _amountMoneyLaunderedPerYear; }

        private double _maxAmountMoneyLaundered;
        double IBuisinessBuilding.maxAmountMoneyLaundered { get => _maxAmountMoneyLaundered; }

        private float _percentageMoneyLaundered;
        float IBuisinessBuilding.percentageMoneyLaundered { get => _percentageMoneyLaundered; }

        private ushort _numberVisitors;
        ushort IBuisinessBuilding.numberVisitors { get => _numberVisitors; }

        private ushort _maxNumberVisitors;
        ushort IBuisinessBuilding.maxNumberVisitors => _maxNumberVisitors;

        private ushort _averageCheckVisitor;
        ushort IBuisinessBuilding.averageCheckVisitor => _averageCheckVisitor;

        private ushort _averageSpendPerVisitor;
        ushort IBuisinessBuilding.averageSpendPerVisitor => _averageSpendPerVisitor;

        private float _institutionPopularity;
        float IBuisinessBuilding.institutionPopularity => _institutionPopularity;

        private bool _isWork;
        bool IBuisinessBuilding.isWork { get => _isWork; set => _isWork = value; }

        private CityTreasury _cityTreasury;
        public CityTreasury cityTreasury => _cityTreasury;

        private CityControl _cityControl;
        public CityControl cityControl => _cityControl;

        private BusinessDataSO _businessDataSO;
        public BusinessDataSO businessDataSO => _businessDataSO;


        public BusinessBank(in CityTreasury cityTreasury, in CityControl cityControl, in BusinessDataSO businessDataSO)
        {
            _cityTreasury = cityTreasury;
            _cityControl = cityControl;
            _businessDataSO = businessDataSO;

            _averageCheckVisitor = _businessDataSO.averageCheckVisitor;
            _averageSpendPerVisitor = _businessDataSO.averageSpendPerVisitor;
            _maxNumberVisitors = _businessDataSO.maxNumberVisitors;
            _maxAmountMoneyLaundered = 20_000; //!
        }

        public void WorkBusiness()
        {
            if (_isWork)
            {
                CalculationNumberVisitors();
                _income = _numberVisitors * _averageCheckVisitor;
                _consumption = _numberVisitors * _averageSpendPerVisitor;
                var netProfit = _income + _cityTreasury.LaunderMoney(_percentageMoneyLaundered, _maxAmountMoneyLaundered) - _consumption;
                Debug.Log($"Bank is work | Net Profit {netProfit} I: {_income}  C: {_consumption}");
                DataControl.IdataPlayer.AddPlayerMoney(netProfit);

                void CalculationNumberVisitors()
                {
                    _numberVisitors = (ushort)Mathf.Clamp(_cityControl.populationCity *
                                                          _institutionPopularity * Random.Range(_businessDataSO.minPercentageVisitors,
                                                                                          _businessDataSO.maxPercentageVisitors),
                                                                                          0, _maxNumberVisitors);
                    Debug.Log($"Number visitors bank: {_numberVisitors}");
                }
            }
        }

        public byte GetOccupiedNumberSlots() { return _occupiedNumberSlots; }

        public void ChangePercentageMoneyLaundered(float setPercentage)
        {
            _percentageMoneyLaundered = setPercentage;
        }

        public float GetPercentageMoneyLaundered() { return _percentageMoneyLaundered; }

        public void SetInstitutionPopularity(float percentPopularity)
        {
            _institutionPopularity = percentPopularity;
        }

        public void UpgradeMaxNumberVisitors()
        {
            _maxNumberVisitors *= 2; //!
        }
    }
}
