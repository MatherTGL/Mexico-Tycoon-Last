using System.Linq;
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
    internal sealed class CityControl : MonoBehaviour, IBoot, ICityControlSell, IPluggableingRoad
    {
        private const byte c_mathematicalDivisor = 100;

        private static byte c_maxConnectionObjects = 4;

        private ICityView _IcityView;

        private ICityDrugBuyers _IcityDrugBuyers;
        ICityDrugBuyers ICityControlSell._IcityDrugBuyers => _IcityDrugBuyers;

        [ShowInInspector, FoldoutGroup("Parameters/Links"), ReadOnly]
        private ICityControlSell _IcityControlSell;

        [ShowInInspector, FoldoutGroup("Parameters/Links"), ReadOnly]
        private ICityDrugSell _IcityDrugSell;

        private RoadBuilded _roadBuilded;

        [ShowInInspector, FoldoutGroup("Parameters/Drugs"), ReadOnly]
        private Dictionary<string, float> d_amountDrugsInCity = new Dictionary<string, float>();
        Dictionary<string, float> ICityControlSell.d_amountDrugsInCity => d_amountDrugsInCity;

        [ShowInInspector, FoldoutGroup("Parameters/Drugs/WeightSell"), ReadOnly]
        private Dictionary<string, float> d_weightToSellDrugs = new Dictionary<string, float>();
        Dictionary<string, float> ICityControlSell.d_weightToSellDrugs => d_weightToSellDrugs;

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

        [BoxGroup("Parameters"), Title("Connect Objects", horizontalLine: false), SerializeField, ReadOnly]
        [MinValue(0), HideLabel]
        private byte _connectObjectsCount = 0;
        public byte connectObjectsCount => _connectObjectsCount;

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
        float ICityControlSell.buyersDrugsClampDemandMin => _buyersDrugsClampDemandMin;

        [FoldoutGroup("Parameters/Drugs"), Title("Max", horizontalLine: false)]
        [MinValue("@_buyersDrugsClampDemandMin + 1"), HideLabel, SerializeField, HorizontalGroup("Parameters/Drugs/ClampDemand")]
        private float _buyersDrugsClampDemandMax = 4;
        float ICityControlSell.buyersDrugsClampDemandMax => _buyersDrugsClampDemandMax;

        private string _gameObjectConnectionIndex;

        [ShowInInspector, FoldoutGroup("Parameters/Control"), ReadOnly]
        private Dictionary<string, InfoDrugClientsTransition> d_allInfoObjectClientsTransition = new Dictionary<string, InfoDrugClientsTransition>();
        Dictionary<string, InfoDrugClientsTransition> IPluggableingRoad.d_allInfoObjectClientsTransition => d_allInfoObjectClientsTransition;

        [ShowInInspector, FoldoutGroup("Parameters/Control")]
        [HideLabel, Title("New Transport Way Link", horizontalLine: false)]
        private IPluggableingRoad _connectingObject;
        IPluggableingRoad IPluggableingRoad.connectingObject => _connectingObject;

        [SerializeField, FoldoutGroup("Parameters/Control"), MinValue(0.0f), HideLabel, Title("Upload Resource", horizontalLine: false)]
        [SuffixLabel("kg")]
        private float _uploadResourceAddWay;
        public float uploadResourceAddWay => _uploadResourceAddWay;

        [SerializeField, FoldoutGroup("Parameters/Control"), EnumPaging, HideLabel, Title("Type Drug")]
        private FabricControl.TypeProductionResource _typeProductionResource;


#if UNITY_EDITOR
        [Button("New Way"), FoldoutGroup("Parameters/Control"), PropertySpace(10)]
        [ShowIf("@_connectingObject != null"), HorizontalGroup("Parameters/Control")]
        private void AddNewTransportWay() //! сделать общим, т.к код одинаковый
        {
            _roadBuilded = new RoadBuilded();
            d_allInfoObjectClientsTransition.Add(_connectingObject.ToString() + _typeProductionResource.ToString(), new InfoDrugClientsTransition());

            if (d_allInfoObjectClientsTransition[_connectingObject.ToString() + _typeProductionResource.ToString()].d_allClientObjects.ContainsKey(this) is false)
            {
                _roadBuilded.roadResourcesManagement.CreateNewRoute(this, _connectingObject);

                d_allInfoObjectClientsTransition[_connectingObject.ToString() + _typeProductionResource.ToString()].d_allClientObjects.Add(this, _roadBuilded.roadResourcesManagement.CheckAllConnectionObjectsRoad(this));
                d_allInfoObjectClientsTransition[_connectingObject.ToString() + _typeProductionResource.ToString()].d_typeDrugAndAmountTransition.Add(_typeProductionResource.ToString(), _uploadResourceAddWay);

                _connectingObject.ConnectObjectToObject(_typeProductionResource.ToString(), gameObject.name + _connectingObject.ToString(), _connectingObject, this);
            }

            _connectingObject = null;
            _uploadResourceAddWay = 0;
        }
#endif


        public void InitAwake() => SearchAndCreateComponents();

        private void SearchAndCreateComponents()
        {
            if (_configCityControlView is not null) { _IcityView = new CityControlView(_configCityControlView); }

            if (_spriteRendererObject is null) { _spriteRendererObject = GetComponent<SpriteRenderer>(); }

            if (_timeDateControl is null) { _timeDateControl = FindObjectOfType<TimeDateControl>(); }

            if (_cityReproduction is null)
                _cityReproduction = new CityReproduction(c_mathematicalDivisor, _populationChangeStepPercentMax, _populationChangeStepPercentMin);

            _IcityDrugSell = new CityDrugsSell();
            //_roadBuilded = new RoadBuilded();
            _IcityControlSell = this;

            SetBuyersDrugs();
            SetWeightToSellDrugs();

            StartCoroutine(Reproduction());
            StartCoroutine(SubmittingResources());
        }

        public void ConnectObjectToObject(string typeFabricDrug, string gameObjectConnectionIndex, IPluggableingRoad FirstObject, IPluggableingRoad SecondObject)
        {
            if (_connectObjectsCount < c_maxConnectionObjects)
            {
                _gameObjectConnectionIndex = gameObjectConnectionIndex;
                _connectObjectsCount++;

                CheckDemandDictionary(typeFabricDrug);
                _roadControl.BuildRoad(transform.position, SecondObject.GetPositionVector2(), gameObjectConnectionIndex);
            }

            if (_connectObjectsCount! > 0) { _IcityView.ConnectFabric(ref _spriteRendererObject); }
        }

        public void DisconnectObjectToObject(string gameObjectDisconnectTo)
        {
            if (_connectObjectsCount >= 1)
            {
                _roadControl.DestroyRoad(gameObjectDisconnectTo);
                _connectObjectsCount--;
            }
            else { _IcityView.DisconnectFabric(ref _spriteRendererObject); }
        }

        public Vector2 GetPositionVector2() => transform.position;

        private void CheckDemandDictionary(string typeFabricDrug)
        {
            if (d_amountDrugsInCity.ContainsKey(typeFabricDrug) is false) { d_amountDrugsInCity.Add(typeFabricDrug, 0); }
        }

        public void IngestResources(string typeFabricDrug, in bool isWork, in float addResEveryStep)
        {
            if (isWork)
                if (d_amountDrugsInCity[typeFabricDrug] < _maxCapacityStock)
                    d_amountDrugsInCity[typeFabricDrug] += addResEveryStep;

            _roadControl.DecliningDemandUpdate(addResEveryStep, typeFabricDrug, _gameObjectConnectionIndex);
            SellResources(in typeFabricDrug);
        }

        private void SellResources(in string typeFabricDrug)
        {
            var allTypeDrugs = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

            foreach (var contractBuyers in _IcityDrugBuyers.d_contractBuyers.Keys)
                _IcityDrugSell.Sell(d_weightToSellDrugs[typeFabricDrug], contractBuyers, typeFabricDrug, _IcityControlSell);
        }

        [Button("Set Users Parameters", 30), FoldoutGroup("Parameters/Drugs/Percentage Users")]
        private void SetBuyersDrugs()
        {
            _IcityDrugBuyers = GetComponent<DrugBuyersContractControl>();

            var countsDrugBuyers = Enum.GetNames(typeof(DrugBuyers.AllBuyers));

            for (int i = 0; i < countsDrugBuyers.Length / 2; i++)
            {
                var addRandomBuyer = UnityEngine.Random.Range(0, countsDrugBuyers.Length);

                if (_IcityDrugBuyers.d_contractBuyers.ContainsKey(countsDrugBuyers[addRandomBuyer]) is false)
                    _IcityDrugBuyers.d_contractBuyers.Add(countsDrugBuyers[addRandomBuyer], new ContractBuyerInfo());
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

        private IEnumerator SubmittingResources()
        {
            while (true)
            {
                foreach (string item in d_allInfoObjectClientsTransition.Keys)
                    if (d_allInfoObjectClientsTransition[item].d_allClientObjects.Count > 0)
                        for (int i = 0; i < d_allInfoObjectClientsTransition[item].d_allClientObjects.Count; i++)
                            Debug.Log("HSHHHA");

                yield return new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));
            }
        }
    }
}
