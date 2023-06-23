using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Data;
using Config.CityControl.View;
using System.Collections;
using TimeControl;


namespace City
{
    internal sealed class CityControl : MonoBehaviour, IBoot
    {
        private const byte c_mathematicalDivisor = 100;

        private ICityView _IcityView;

        private SpriteRenderer _spriteRendererObject;

        [SerializeField, BoxGroup("Parameters"), Required, Title("Time Date Control"), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, BoxGroup("Parameters")]
        [Title("Config City Control View"), HideLabel, Required, PropertySpace(0, 15)]
        private ConfigCityControlView _configCityControlView;

        [FoldoutGroup("Parameters/Population City"), Title("Population in City", horizontalLine: false)]
        [MinValue(0), MaxValue(double.MaxValue / 2), HideLabel, SerializeField]
        private uint _populationCity;

        [SerializeField, Title("Population Percent Change in %/day", horizontalLine: false)]
        [HideLabel, ReadOnly, FoldoutGroup("Parameters/Population City/Population Change Step")]
        private float _populationChangeStepPercent;

        [FoldoutGroup("Parameters/Population City/Population Change Step"), Title("Max %/day", horizontalLine: false)]
        [MinValue(0.1f), MaxValue(1.0f), SerializeField, HideLabel]
        [HorizontalGroup("Parameters/Population City/Population Change Step/Horizontal")]
        private float _populationChangeStepPercentMax;

        [FoldoutGroup("Parameters/Population City/Population Change Step"), Title("Min %/day", horizontalLine: false)]
        [MinValue(-1.0f), MaxValue(-0.01f), SerializeField, HideLabel]
        [HorizontalGroup("Parameters/Population City/Population Change Step/Horizontal")]
        private float _populationChangeStepPercentMin;

        [SerializeField, BoxGroup("Parameters"), Title("Police Level in star (0-10)", horizontalLine: false)]
        [MinValue(0), MaxValue(10), Tooltip("Уровень полиции в данном городе"), PropertySpace(5), HideLabel]
        private byte _policeLevel;

        [SerializeField, BoxGroup("Parameters"), Title("Max Capacity Stock in kg", horizontalLine: false)]
        [MinValue(0.0f), Tooltip("Максимальная вместимость хранилища города в кг"), HideLabel]
        private float _maxCapacityStock;

        [SerializeField, BoxGroup("Parameters"), Title("Current Capacity Stock in kg", horizontalLine: false), HideLabel]
        [MinValue(0.0f), Tooltip("Текущая вместимость хранилища города в кг"), ReadOnly]
        private float _currentCapacityStock;

        [FoldoutGroup("Parameters/Drugs"), SerializeField, HideLabel]
        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Percentage of Users Cocaine in %", horizontalLine: false)]
        [MinValue(1.0f), MaxValue(80.0f), Tooltip("Процент наркоманов употребляющих кокаин в данном городе")]
        private float _percentageUsersCocaine;

        [Title("Average Cost Cocaine in $/kg", horizontalLine: false)]
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), HideLabel]
        private uint _averageCostCocaine;

        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Weight to Sell Cocaine in kg", horizontalLine: false)]
        [MinValue(1), SerializeField, HideLabel]
        private float _weightToSellCocaine;

        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Current Drug Demand Cocaine in kg/day")]
        [MinValue(0.0f), HideLabel, SerializeField]
        private float _currentDrugDemandCocaine;

        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Increased Demand Cocaine in kg/day")]
        [MinValue(0.01f), HideLabel, SerializeField]
        private float _increasedDemandCocaine;

        private float _decliningDemand;

        private byte _connectFabricsCount;


        public void InitAwake() => SearchAndCreateComponents();

        private void SearchAndCreateComponents()
        {
            if (_configCityControlView is not null) { _IcityView = new CityControlView(_configCityControlView); }
            else { Debug.LogError("Config City Control View missing in CityControl.cs"); }

            if (_spriteRendererObject is null) { _spriteRendererObject = GetComponent<SpriteRenderer>(); }

            if (_timeDateControl is null) { _timeDateControl = FindObjectOfType<TimeDateControl>(); }

            StartCoroutine(Reproduction());
        }

        public void ConnectFabricToCity(float decliningDemand)
        {
            _connectFabricsCount++;
            _decliningDemand = decliningDemand;

            if (_connectFabricsCount! > 0) { _IcityView.ConnectFabric(ref _spriteRendererObject); }
        }

        public void DisconnectFabricToCity()
        {
            if (_connectFabricsCount >= 1)
            {
                _connectFabricsCount--;

                if (_connectFabricsCount == 0) { _IcityView.DisconnectFabric(ref _spriteRendererObject); }
            }
        }

        public float GetDecliningDemand() => _decliningDemand;

        public bool CheckCurrentCapacityStock()
        {
            if (_currentCapacityStock < _maxCapacityStock) return true;
            else return false;
        }

        public void IngestResources() //todo докинуть тип наркотика
        {
            if (_currentDrugDemandCocaine > _decliningDemand)
            {
                _currentDrugDemandCocaine += _increasedDemandCocaine - _decliningDemand;
                _currentCapacityStock += _decliningDemand;
                SellDrugs();
            }
            else
                _currentDrugDemandCocaine += _increasedDemandCocaine;
        }

        private void SellDrugs()
        {
            if (_currentCapacityStock > _weightToSellCocaine)
            {
                _currentCapacityStock -= _weightToSellCocaine;
                DataControl.IdataPlayer.AddPlayerMoney(_averageCostCocaine);
            }
        }

        private void ReproductionPopulation()
        {
            _populationChangeStepPercent = Random.Range(_populationChangeStepPercentMin, _populationChangeStepPercentMax);
            uint addCountPeople = (uint)(_populationCity * _populationChangeStepPercent / c_mathematicalDivisor);
            _populationCity += addCountPeople;
        }

        private IEnumerator Reproduction()
        {
            while (true)
            {
                if (!_timeDateControl.GetStatePaused()) { ReproductionPopulation(); }
                yield return new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));
            }
        }
    }
}
