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
    [RequireComponent(typeof(DrugBuyersContractControl))]
    internal sealed class CityControl : MonoBehaviour, IBoot
    {
        private const byte c_mathematicalDivisor = 100;

        private const byte c_maxConnectionFabrics = 4;

        private ICityView _IcityView;

        private ICityDrugBuyers _IcityDrugBuyers;

        [ShowInInspector, FoldoutGroup("Parameters/Drugs"), ReadOnly]
        private Dictionary<string, float> d_amountDrugsInCity = new Dictionary<string, float>();

        [ShowInInspector, FoldoutGroup("Parameters/Drugs/WeightSell"), ReadOnly]
        private Dictionary<string, float> d_weightToSellDrugs = new Dictionary<string, float>();

        private CityReproduction _cityReproduction;

        private SpriteRenderer _spriteRendererObject;

        [SerializeField, FoldoutGroup("Parameters/Links"), Required, Title("Time Date Control"), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, FoldoutGroup("Parameters/Links"), Required, Title("Road Control"), HideLabel]
        private RoadControl _roadControl;

        [SerializeField, FoldoutGroup("Parameters/Links"), Required, Title("City Drugs Sell"), HideLabel, ReadOnly]
        private CityDrugsSell _cityDrugsSell;

        [SerializeField, FoldoutGroup("Parameters/Links")]
        [Title("Config City Control View"), HideLabel, Required, PropertySpace(0, 15)]
        private ConfigCityControlView _configCityControlView;

        [FoldoutGroup("Parameters/Population City"), Title("Population in City", horizontalLine: false)]
        [MinValue(0), MaxValue(double.MaxValue / 2), HideLabel, SerializeField]
        private uint _populationCity;

        [FoldoutGroup("Parameters/Population City/Population Change Step"), Title("Max", horizontalLine: false)]
        [MinValue(0.1f), MaxValue(1.0f), SerializeField, HideLabel, SuffixLabel("%/day")]
        [HorizontalGroup("Parameters/Population City/Population Change Step/Horizontal")]
        private float _populationChangeStepPercentMax;

        [FoldoutGroup("Parameters/Population City/Population Change Step"), Title("Min", horizontalLine: false)]
        [MinValue(-1.0f), MaxValue(-0.01f), SerializeField, HideLabel, SuffixLabel("%/day")]
        [HorizontalGroup("Parameters/Population City/Population Change Step/Horizontal")]
        private float _populationChangeStepPercentMin;

        [SerializeField, BoxGroup("Parameters"), Title("Police Level", horizontalLine: false), SuffixLabel("Star (0-10)")]
        [MinValue(0), MaxValue(10), Tooltip("Уровень полиции в данном городе"), PropertySpace(5), HideLabel]
        private byte _policeLevel;

        [BoxGroup("Parameters"), Title("Connect Fabrics", horizontalLine: false), SerializeField, ReadOnly]
        [MinValue(0), HideLabel]
        private byte _connectFabricsCount = 0;

        [SerializeField, BoxGroup("Parameters"), Title("Max Capacity Stock", horizontalLine: false)]
        [MinValue(0.0f), Tooltip("Максимальная вместимость хранилища города в кг"), HideLabel, SuffixLabel("kg")]
        private float _maxCapacityStock;

        [SerializeField, FoldoutGroup("Parameters/Drugs/WeightSell"), MinValue(0.0f)]
        private float[] _weightToSellArray;

        [FoldoutGroup("Parameters/Drugs"), Title("Increased Demand in kg/day", horizontalLine: false)]
        [MinValue(0.01f), HideLabel, SerializeField]
        private float _increasedDemand;

        [FoldoutGroup("Parameters/Drugs"), Title("Min Clamp Demand Buyers", horizontalLine: false), HorizontalGroup("Parameters/Drugs/ClampDemand")]
        [MinValue("@_buyersDrugsClampDemandMin"), HideLabel, SerializeField, MaxValue("@_buyersDrugsClampDemandMax - 1")]
        private float _buyersDrugsClampDemandMin = 3;

        [FoldoutGroup("Parameters/Drugs"), Title("Max", horizontalLine: false)]
        [MinValue("@_buyersDrugsClampDemandMin + 1"), HideLabel, SerializeField, HorizontalGroup("Parameters/Drugs/ClampDemand")]
        private float _buyersDrugsClampDemandMax = 4;


        public void InitAwake()
        {
            SearchAndCreateComponents();

            SetBuyersDrugs();
            SetWeightToSellDrugs();
        }

        private void SearchAndCreateComponents()
        {
            if (_configCityControlView is not null) { _IcityView = new CityControlView(_configCityControlView); }

            if (_spriteRendererObject is null) { _spriteRendererObject = GetComponent<SpriteRenderer>(); }

            if (_timeDateControl is null) { _timeDateControl = FindObjectOfType<TimeDateControl>(); }

            if (_cityReproduction is null)
                _cityReproduction = new CityReproduction(c_mathematicalDivisor,
                                                         _populationChangeStepPercentMax,
                                                         _populationChangeStepPercentMin);

            _cityDrugsSell = new CityDrugsSell();

            StartCoroutine(Reproduction());
        }

        public void ConnectFabricToCity(string typeFabricDrug, Vector2 positionFabric, string gameObjectConnectionTo)
        {
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
                    Debug.Log($"CityControl Add Resources {typeFabricDrug} Every Step{addResEveryStep}");
                }
                else { Debug.Log("Хранилище заполнено"); }
            }
            _roadControl.DecliningDemandUpdate(addResEveryStep, typeFabricDrug);

            // _IcityDrugBuyers.d_contractDrugsCityDemand[typeFabricDrug] = Mathf.Clamp(_IcityDrugBuyers.d_contractDrugsCityDemand[typeFabricDrug] + _increasedDemand,
            //                                                         _buyersDrugsClampDemandMin,
            //                                                         _buyersDrugsClampDemandMax);

            SellResources(in typeFabricDrug);
        }

        private void SellResources(in string typeFabricDrug)
        {
            foreach (var buyers in _IcityDrugBuyers.d_contractContactAndDrug.Keys)
            {
                if (_IcityDrugBuyers.d_contractContactAndDrug[buyers] is true)
                {
                    if (d_amountDrugsInCity[typeFabricDrug] >= d_weightToSellDrugs[typeFabricDrug] && _IcityDrugBuyers.d_contractDrugsCityDemand[buyers + typeFabricDrug] >= d_weightToSellDrugs[typeFabricDrug])
                    {
                        //? начисляется просто так, потому что проверку проходит d_weightToSellDrugs = 0, начисляя деньги просто так
                        d_amountDrugsInCity[typeFabricDrug] -= d_weightToSellDrugs[typeFabricDrug];
                        _IcityDrugBuyers.d_contractDrugsCityDemand[buyers + typeFabricDrug] -= d_weightToSellDrugs[typeFabricDrug];
                        //_cityDrugsSell.Sell(d_weightToSellDrugs[typeFabricDrug], _IcityDrugBuyers);

                        //! сделать контрактные поставки
                        Debug.Log($"Sell {typeFabricDrug} | Current Demand Contracts {_IcityDrugBuyers.d_contractDrugsCityDemand[buyers]}");
                    }
                }
            }
        }

        [Button("Set Users Parameters", 30), FoldoutGroup("Parameters/Drugs/Percentage Users")]
        private void SetBuyersDrugs()
        {
            _IcityDrugBuyers = GetComponent<DrugBuyersContractControl>();

            var countsDrugBuyers = Enum.GetNames(typeof(DrugBuyers.AllBuyers));

            for (int i = 0; i < countsDrugBuyers.Length / 2; i++)
            {
                var addRandomBuyer = UnityEngine.Random.Range(0, countsDrugBuyers.Length);

                if (_IcityDrugBuyers.d_contractContactAndDrug.ContainsKey(countsDrugBuyers[addRandomBuyer]) is false)
                    _IcityDrugBuyers.d_contractContactAndDrug.Add(countsDrugBuyers[addRandomBuyer], false);
            }
        }

        [Button("Set Weight Parameters", 30), FoldoutGroup("Parameters/Drugs/WeightSell")]
        private void SetWeightToSellDrugs()
        {
            var countsDrugBuyers = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

            if (_weightToSellArray.Length < countsDrugBuyers.Length)
                _weightToSellArray = new float[countsDrugBuyers.Length];

            for (int i = 0; i < countsDrugBuyers.Length; i++)
            {
                if (d_weightToSellDrugs.ContainsKey(countsDrugBuyers[i]) is false)
                    d_weightToSellDrugs.Add(countsDrugBuyers[i], _weightToSellArray[i]);
                else
                    d_weightToSellDrugs[countsDrugBuyers[i]] = _weightToSellArray[i];
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
