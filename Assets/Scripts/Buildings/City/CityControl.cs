using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using Data;
using Config.CityControl.View;
using System.Collections;
using TimeControl;


namespace City
{
    public sealed class CityControl : MonoBehaviour, IBoot
    {
        private ICityView _IcityView;

        private SpriteRenderer _spriteRendererObject;

        [SerializeField, BoxGroup("Parameters"), Required, Title("Time Date Control"), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, BoxGroup("Parameters"), Title("Config City Control View"), HideLabel, Required, PropertySpace(0, 15)]
        private ConfigCityControlView _configCityControlView;

        [FoldoutGroup("Parameters/Population City"), Title("Population in City", horizontalLine: false)]
        [MinValue(0), MaxValue(double.MaxValue / 2), HideLabel, SerializeField]
        private uint _populationCity;

        [FoldoutGroup("Parameters/Population City"), Title("Population Percent Change in %/day", horizontalLine: false)]
        [MinValue(-0.3f), MaxValue(0.3f), SerializeField, HideLabel]
        private float _populationChangeStepPercent;

        [SerializeField, BoxGroup("Parameters"), Title("Police Level in star (0-10)"), HideLabel]
        [MinValue(0), MaxValue(10), Tooltip("Уровень полиции в данном городе"), PropertySpace(5)]
        private byte _policeLevel;

        [SerializeField, BoxGroup("Parameters"), Title("Max Capacity Stock in kg"), HideLabel]
        [MinValue(0.0f), Tooltip("Максимальная вместимость хранилища города в кг")]
        private float _maxCapacityStock;

        [SerializeField, BoxGroup("Parameters"), Title("Current Capacity Stock in kg)"), HideLabel]
        [MinValue(0.0f), Tooltip("Текущая вместимость хранилища города в кг"), ReadOnly]
        private float _currentCapacityStock;

        [FoldoutGroup("Parameters/Drugs")]
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Percentage of Users Cocaine in %"), HideLabel]
        [MinValue(1.0f), MaxValue(80.0f), Tooltip("Процент наркоманов употребляющих кокаин в данном городе")]
        private float _percentageUsersCocaine;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Average Cost Cocaine in $/kg"), HideLabel]
        private uint _averageCostCocaine;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Weight to Sell Cocaine in kg"), HideLabel]
        [MinValue(1)]
        private float _weightToSellCocaine; //? сменить на float для игрока

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Current Drug Demand Cocaine in kg/day"), HideLabel]
        [MinValue(0.0f)]
        private float _currentDrugDemandCocaine;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Increased Demand Cocaine in kg/day"), HideLabel]
        [MinValue(0.01f)]
        private float _increasedDemandCocaine;

        private float _decliningDemand;

        private byte _connectFabricsCount;


        public void InitAwake()
        {
            SearchAndCreateComponents();

            Debug.Log("Город успешно инициализирован");
        }

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
            Debug.Log($"Connect Fabric | City DD {decliningDemand}");
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
            //_currentDrugDemandCocaine

            if (_currentDrugDemandCocaine > _decliningDemand)
            {
                _currentDrugDemandCocaine += _increasedDemandCocaine - _decliningDemand;
                _currentCapacityStock += _decliningDemand;
                SellDrugs();
            }
            else
                _currentDrugDemandCocaine += _increasedDemandCocaine;
            Debug.Log($"Ingest Resources {_decliningDemand}");
        }

        private void SellDrugs()
        {
            if (_currentCapacityStock > _weightToSellCocaine)
            {
                _currentCapacityStock -= _weightToSellCocaine;
                DataControl.IdataPlayer.AddPlayerMoney(_averageCostCocaine);
                Debug.Log("Sell Drugs");
            }
        }

        private void ReproductionPopulation()
        {
            uint addCountPeople = (uint)(_populationCity * _populationChangeStepPercent / 100);
            _populationCity += addCountPeople;
        }

        private IEnumerator Reproduction()
        {
            while (true)
            {
                ReproductionPopulation();
                yield return new WaitForSecondsRealtime(_timeDateControl.GetCurrentTimeOneDay());
            }
        }
    }
}
