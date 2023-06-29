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

        private const float c_weightToSell = 1;

        private ICityView _IcityView;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        private Dictionary<string, float> d_amountDrugsInCity = new Dictionary<string, float>();

        //[ShowInInspector, BoxGroup("Parameters"), ReadOnly]
        //private Dictionary<string, float> d_addingResourceEveryStep = new Dictionary<string, float>();

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

        [BoxGroup("Parameters"), Title("Connect Fabrics", horizontalLine: false), SerializeField, ReadOnly]
        [MinValue(0), HideLabel]
        private byte _connectFabricsCount = 0;

        [SerializeField, BoxGroup("Parameters"), Title("Max Capacity Stock in kg", horizontalLine: false)]
        [MinValue(0.0f), Tooltip("Максимальная вместимость хранилища города в кг"), HideLabel]
        private float _maxCapacityStock;

        [FoldoutGroup("Parameters/Drugs"), ShowInInspector, HideLabel, ReadOnly]
        [FoldoutGroup("Parameters/Drugs/Percentage Users"), Title("Buyers Drugs name/kg", horizontalLine: false)]
        private Dictionary<string, float> d_buyersDrugsDemand = new Dictionary<string, float>();

#if UNITY_EDITOR
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cost Parameters"), HideLabel, ReadOnly, HorizontalGroup("Parameters/Drugs/Cost Parameters/Average Cost")]
        [Title("Name Drug", horizontalLine: false)]
        private string[] _nameDrugEditorAverageCost;

        [SerializeField, FoldoutGroup("Parameters/Drugs/Cost Parameters"), HideLabel, MinValue("@_minValueEditorAverageCost")]
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
            }
        }

        [Button("Set Users Parameters", 30), FoldoutGroup("Parameters/Drugs/Percentage Users")]
        private void SetBuyersDrugs()
        {
            var countsDrugBuyers = Enum.GetNames(typeof(DrugBuyers.AllBuyers));

            for (int i = 0; i < countsDrugBuyers.Length; i++)
            {
                d_buyersDrugsDemand.Add(countsDrugBuyers[i], UnityEngine.Random.Range(1, 10)); //!1/10
            }
        }
#endif

        [Title("Cost $/kg", horizontalLine: false), MinValue("@_minValueEditorAverageCost")]
        [SerializeField, FoldoutGroup("Parameters/Drugs/Cost Parameters"), HideLabel, HorizontalGroup("Parameters/Drugs/Cost Parameters/Average Cost")]
        private uint[] _averageCostDrugs;

        [FoldoutGroup("Parameters/Drugs"), Title("Increased Demand in kg/day", horizontalLine: false)]
        [MinValue(0.01f), HideLabel, SerializeField]
        private float _increasedDemand;

        [FoldoutGroup("Parameters/Drugs"), Title("Min Clamp Demand Buyers", horizontalLine: false), HorizontalGroup("Parameters/Drugs/ClampDemand")]
        [MinValue("@_buyersDrugsClampDemandMin"), HideLabel, SerializeField, MaxValue("@_buyersDrugsClampDemandMax - 1")]
        private float _buyersDrugsClampDemandMin = 3;

        [FoldoutGroup("Parameters/Drugs"), Title("Max", horizontalLine: false)]
        [MinValue("@_buyersDrugsClampDemandMin + 1"), HideLabel, SerializeField, HorizontalGroup("Parameters/Drugs/ClampDemand")]
        private float _buyersDrugsClampDemandMax = 4;


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

            SetBuyersDrugs();

            StartCoroutine(Reproduction());
        }

        public void ConnectFabricToCity(string typeFabricDrug,
                                        Vector2 positionFabric,
                                        string gameObjectConnectionTo)
        {
            Debug.Log(gameObjectConnectionTo);
            if (_connectFabricsCount < c_maxConnectionFabrics)
            {
                _connectFabricsCount++;

                CheckDemandDictionary(typeFabricDrug);

                _roadControl.BuildRoad(transform.position, positionFabric, gameObjectConnectionTo);
            }

            if (_connectFabricsCount! > 0) { _IcityView.ConnectFabric(ref _spriteRendererObject); }
        }

        public void DisconnectFabricToCity(string gameObjectDisconnectTo)
        {
            if (_connectFabricsCount >= 1)
            {
                _roadControl.DestroyRoad(gameObjectDisconnectTo);
                _connectFabricsCount--;
            }
            else { _IcityView.DisconnectFabric(ref _spriteRendererObject); }
        }

        private void CheckDemandDictionary(string typeFabricDrug)
        {
            if (d_amountDrugsInCity.ContainsKey(typeFabricDrug) is false)
                d_amountDrugsInCity.Add(typeFabricDrug, 0);
        }

        public void IngestResources(string typeFabricDrug, in bool isWork, in float addResEveryStep)
        {
            if (isWork)
            {
                if (d_amountDrugsInCity[typeFabricDrug] < _maxCapacityStock)
                {
                    d_amountDrugsInCity[typeFabricDrug] += addResEveryStep;
                    Debug.Log(addResEveryStep);
                }
                else { Debug.Log("Хранилище заполнено"); }
            }
            _roadControl.DecliningDemandUpdate(addResEveryStep, typeFabricDrug);

            d_buyersDrugsDemand["FirstClan"] = Mathf.Clamp(d_buyersDrugsDemand["FirstClan"] + _increasedDemand,
                                                                   _buyersDrugsClampDemandMin,
                                                                   _buyersDrugsClampDemandMax);

            //SellResources(ref typeFabricDrug);
        }

        private void SellResources(ref string typeFabricDrug)
        {
            if (d_amountDrugsInCity[typeFabricDrug] >= c_weightToSell && d_buyersDrugsDemand["FirstClan"] >= c_weightToSell) //todo добавить возможность менять weightToSell для каждого типа
            {
                _cityDrugsSell.Sell(typeFabricDrug);
                d_amountDrugsInCity[typeFabricDrug] -= c_weightToSell;
                d_buyersDrugsDemand["FirstClan"] -= c_weightToSell;
                Debug.Log($"Sell {d_buyersDrugsDemand["FirstClan"]}");
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
