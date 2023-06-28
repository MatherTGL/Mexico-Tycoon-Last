using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Boot;
using Config.CityControl.View;
using System.Collections;
using TimeControl;
using Road;
using Fabric;
using System;

namespace City
{
    internal sealed class CityControl : MonoBehaviour, IBoot
    {
        private const byte c_mathematicalDivisor = 100;

        private const byte c_maxConnectionFabrics = 4;

        private ICityView _IcityView;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, float> d_amountDrugsInCity = new Dictionary<string, float>();

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, float> d_addingResourceEveryStep = new Dictionary<string, float>();

        private CityReproduction _cityReproduction;

        private SpriteRenderer _spriteRendererObject;

        [SerializeField, BoxGroup("Parameters"), Required, Title("Time Date Control"), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, BoxGroup("Parameters"), Required, Title("Road Control"), HideLabel]
        private RoadControl _roadControl;

        private CityDrugsSell _cityDrugsSell;

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

        [FoldoutGroup("Parameters/Drugs/Sell"), Title("Weight to Sell in kg", horizontalLine: false)]
        [MinValue(1), SerializeField, HideLabel, HorizontalGroup("Parameters/Drugs/Sell/hor")]
        private float _weightToSell = 1;

        [FoldoutGroup("Parameters/Drugs"), SerializeField, HideLabel]
        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Percentage of Users Cocaine in %", horizontalLine: false)]
        [MinValue(1.0f), MaxValue(80.0f), Tooltip("Процент наркоманов употребляющих кокаин в данном городе")]
        private float _percentageUsersCocaine;

#if UNITY_EDITOR
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cost Parameters"), HideLabel, ReadOnly, HorizontalGroup("Parameters/Drugs/Cost Parameters/Average Cost")]
        [Title("Name Drug", horizontalLine: false)]
        private string[] _nameDrugEditorAverageCost;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cost Parameters"), HideLabel, MinValue(5_000)]
        [Title("Min Value Average Cost", horizontalLine: false), ShowIf("_isShowAdditionalParameters")]
        private uint _minValueEditorAverageCost = 5_000;

        private bool _isShowAdditionalParameters;

        [Button("Additional", 30), FoldoutGroup("Parameters/Drugs/Cost Parameters"), HorizontalGroup("Parameters/Drugs/Cost Parameters/BtnHor")]
        private void ShowAdditionalParametersEditor()
        {
            _isShowAdditionalParameters = !_isShowAdditionalParameters;
        }

        [Button("Update", 30), FoldoutGroup("Parameters/Drugs/Cost Parameters"), HorizontalGroup("Parameters/Drugs/Cost Parameters/BtnHor")]
        private void InitCosts()
        {
            var countsTypesDrugs = Enum.GetNames(typeof(FabricControl.TypeProductionResource));
            _averageCostDrugs = new uint[countsTypesDrugs.Length];
            _nameDrugEditorAverageCost = new string[countsTypesDrugs.Length];

            for (int i = 0; i < countsTypesDrugs.Length; i++)
            {
                _averageCostDrugs[i] = _minValueEditorAverageCost;
                _nameDrugEditorAverageCost[i] = countsTypesDrugs[i];
                //? пофиксить цену, которая всегда значится 0 после нажатия кнопки
            }
        }
#endif

        [Title("Cost $/kg", horizontalLine: false), MinValue(5_000)]
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cost Parameters"), HideLabel, HorizontalGroup("Parameters/Drugs/Cost Parameters/Average Cost")]
        private uint[] _averageCostDrugs;

        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Current Drug Demand in kg/day", horizontalLine: false)]
        [MinValue(0.0f), HideLabel, SerializeField]
        private float _currentDrugDemand;

        [FoldoutGroup("Parameters/Drugs/Cocaine"), Title("Increased Demand in kg/day", horizontalLine: false)]
        [MinValue(0.01f), HideLabel, SerializeField]
        private float _increasedDemand;

        private byte _connectFabricsCount = 0;


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
            _cityDrugsSell = new CityDrugsSell(_averageCostDrugs);

            StartCoroutine(Reproduction());
        }

        public void ConnectFabricToCity(float decliningDemand,
                                        string typeFabricDrug,
                                        Vector2 positionFabric,
                                        string gameObjectConnectionTo)
        {
            Debug.Log(gameObjectConnectionTo);
            if (_connectFabricsCount < c_maxConnectionFabrics)
            {
                _connectFabricsCount++;

                CheckDemandDictionary(decliningDemand, typeFabricDrug, true);

                d_addingResourceEveryStep.Add(typeFabricDrug, decliningDemand);
                _roadControl.BuildRoad(transform.position, positionFabric, gameObjectConnectionTo);
                _roadControl.DecliningDemandUpdate(decliningDemand, typeFabricDrug, true);
            }

            if (_connectFabricsCount! > 0) { _IcityView.ConnectFabric(ref _spriteRendererObject); }
        }

        public void DisconnectFabricToCity(string gameObjectDisconnectTo)
        {
            Debug.Log(gameObjectDisconnectTo);
            if (_connectFabricsCount >= 1)
            {
                _roadControl.DestroyRoad(gameObjectDisconnectTo);
                _connectFabricsCount--;
            }
            else { _IcityView.DisconnectFabric(ref _spriteRendererObject); }
        }

        public void AddDecliningDemand(in float decliningDemand, in string typeFabricDrug)
        {
            CheckDemandDictionary(decliningDemand, typeFabricDrug, true);
            d_addingResourceEveryStep[typeFabricDrug] += decliningDemand;
            _roadControl.DecliningDemandUpdate(decliningDemand, typeFabricDrug, true);
        }

        public void ReduceDecliningDemand(in float decliningDemand, in string typeFabricDrug)
        {
            if (d_amountDrugsInCity[typeFabricDrug] >= decliningDemand)
            {
                CheckDemandDictionary(decliningDemand, typeFabricDrug, false);

                d_addingResourceEveryStep[typeFabricDrug] -= decliningDemand;
                _roadControl.DecliningDemandUpdate(decliningDemand, typeFabricDrug, false);
            }
        }

        private void CheckDemandDictionary(float decliningDemand, string typeFabricDrug, bool isAddDrugs)
        {
            if (d_amountDrugsInCity.ContainsKey(typeFabricDrug))
            {
                if (isAddDrugs)
                    d_amountDrugsInCity[typeFabricDrug] += decliningDemand;
                else
                    d_amountDrugsInCity[typeFabricDrug] -= decliningDemand;
            }
            else { d_amountDrugsInCity.Add(typeFabricDrug, decliningDemand); }
        }

        public bool CheckCurrentCapacityStock()
        {
            if (_currentCapacityStock < _maxCapacityStock) return true;
            else return false;
        }

        public void IngestResources(string typeFabricDrug)
        {
            if (d_amountDrugsInCity[typeFabricDrug] < _maxCapacityStock)
            {
                if (_currentDrugDemand > d_addingResourceEveryStep[typeFabricDrug])
                {
                    _currentDrugDemand += _increasedDemand - d_addingResourceEveryStep[typeFabricDrug];
                    _currentCapacityStock += d_addingResourceEveryStep[typeFabricDrug];

                    d_amountDrugsInCity[typeFabricDrug] += d_addingResourceEveryStep[typeFabricDrug];
                    SellResources(ref typeFabricDrug);
                }
                else { _currentDrugDemand += _increasedDemand; }
            }
            else { Debug.Log("Хранилище заполнено"); }
        }

        private void SellResources(ref string typeFabricDrug)
        {
            _cityDrugsSell.Sell(typeFabricDrug, ref _weightToSell, ref _currentCapacityStock);
            d_amountDrugsInCity[typeFabricDrug] -= _weightToSell;
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
