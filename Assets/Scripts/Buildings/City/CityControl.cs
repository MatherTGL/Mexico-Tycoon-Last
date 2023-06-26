using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Boot;
using Data;
using Config.CityControl.View;
using System.Collections;
using TimeControl;
using Road;
using Fabric;

namespace City
{
    internal sealed class CityControl : MonoBehaviour, IBoot
    {
        private const byte c_mathematicalDivisor = 100;

        private const byte _maxConnectionFabrics = 4;

        private ICityView _IcityView;

        private Dictionary<string, ushort> _dictionaryConnectionToObject = new Dictionary<string, ushort>();

        private CityReproduction _cityReproduction;

        private SpriteRenderer _spriteRendererObject;

        [SerializeField, BoxGroup("Parameters"), Required, Title("Time Date Control"), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, BoxGroup("Parameters"), Required, Title("Road Control"), HideLabel]
        private RoadControl _roadControl;

        [SerializeField, BoxGroup("Parameters")]
        [Title("Config City Control View"), HideLabel, Required, PropertySpace(0, 15)]
        private ConfigCityControlView _configCityControlView;

        [FoldoutGroup("Parameters/Population City"), Title("Population in City", horizontalLine: false)]
        [MinValue(0), MaxValue(double.MaxValue / 2), HideLabel, SerializeField]
        private uint _populationCity;

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

            if (_cityReproduction is null)
                _cityReproduction = new CityReproduction(c_mathematicalDivisor,
                                                         _populationChangeStepPercentMax,
                                                         _populationChangeStepPercentMin);

            StartCoroutine(Reproduction());
        }

        public void ConnectFabricToCity(float decliningDemand, Vector2 positionFabric, FabricControl gameObjectConnectionTo)
        {
            Debug.Log(gameObjectConnectionTo);
            if (_connectFabricsCount < _maxConnectionFabrics)
            {
                _connectFabricsCount++;
                _decliningDemand = decliningDemand;
                _roadControl.BuildRoad(transform.position, positionFabric);
                AddDecliningDemand(decliningDemand);

                _dictionaryConnectionToObject.Add(gameObjectConnectionTo.name, _roadControl.GetListAllBuildedRoadLastIndex());
                Debug.Log(_dictionaryConnectionToObject[$"{gameObjectConnectionTo.name}"]);
            }

            if (_connectFabricsCount! > 0) { _IcityView.ConnectFabric(ref _spriteRendererObject); }
        }

        public void DisconnectFabricToCity(FabricControl gameObjectDisconnectTo)
        {
            if (_connectFabricsCount >= 1)
            {
                _connectFabricsCount--;
                var indexDestroy = _dictionaryConnectionToObject[$"{gameObjectDisconnectTo.name}"];
                _roadControl.DestroyRoad(indexDestroy);
                Debug.Log(indexDestroy);

                if (_connectFabricsCount == 0)
                {
                    _IcityView.DisconnectFabric(ref _spriteRendererObject);
                }
            }
        }

        public void AddDecliningDemand(in float decliningDemand)
        {
            _decliningDemand += decliningDemand;
            _roadControl.AddDecliningDemand(decliningDemand);
        }

        public void ReduceDecliningDemand(in float decliningDemand)
        {
            _decliningDemand -= decliningDemand;
            _roadControl.ReduceDecliningDemand(decliningDemand);
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

        private void SellDrugs() //? вынести в отдельный класс для реализации
        {
            if (_currentCapacityStock > _weightToSellCocaine)
            {
                _currentCapacityStock -= _weightToSellCocaine;
                DataControl.IdataPlayer.AddPlayerMoney(_averageCostCocaine);
            }
        }

        private IEnumerator Reproduction()
        {
            while (true)
            {
                if (!_timeDateControl.GetStatePaused()) { _cityReproduction.ReproductionPopulation(ref _populationCity); }
                yield return new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));
            }
        }
    }
}
