using System.Threading.Tasks;
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
using City.Business;
using Upgrade;
using System.Threading;
using System.Linq;

namespace City
{
    [RequireComponent(typeof(DrugBuyersContractControl))]
    [RequireComponent(typeof(CityBusinessControl))]
    [RequireComponent(typeof(UpgradeControl))]
    public sealed class CityControl : MonoBehaviour, IBoot, ICityControlSell, IPluggableingRoad
    {
        private const byte c_mathematicalDivisor = 100;

        private const byte c_maxConnectionObjects = 4;

        private ICityView _IcityView;

        private ICityDrugBuyers _IcityDrugBuyers;
        ICityDrugBuyers ICityControlSell._IcityDrugBuyers => _IcityDrugBuyers;

        private ICityControlSell _IcityControlSell;

        private ICityDrugSell _IcityDrugSell;

        private IPluggableingRoad _pluggableObject;

        [ShowInInspector, BoxGroup("Parameters"), HideLabel, Title("New Transport Way Link", horizontalLine: false), PropertyOrder(1)]
        private IPluggableingRoad _connectingObject;

        private RoadBuilded _roadBuilded;

        private RoadControl _roadControl;

        private CityReproduction _cityReproduction;

        private TimeDateControl _timeDateControl;

        private SpriteRenderer _spriteRendererObject;

        [SerializeField, BoxGroup("Parameters"), Title("Config City Control View", horizontalLine: false)]
        [PropertyOrder(0), HideLabel, Required]
        private ConfigCityControlView _configCityControlView;

        [SerializeField, BoxGroup("Parameters"), EnumPaging, HideLabel, Title("Type Drug"), PropertyOrder(3)]
        private FabricControl.TypeProductionResource _typeProductionResource;

        private WaitForSeconds _coroutineTimeStep;

        private CancellationTokenSource _cancellationTokenSource;

        private CityTreasury _cityTreasury;
        public CityTreasury cityTreasury => _cityTreasury;

        private Dictionary<string, float> d_amountDrugsInCity = new Dictionary<string, float>();
        Dictionary<string, float> ICityControlSell.d_amountDrugsInCity => d_amountDrugsInCity;

        private Dictionary<string, float> d_weightToSellDrugs = new Dictionary<string, float>();
        Dictionary<string, float> ICityControlSell.d_weightToSellDrugs => d_weightToSellDrugs;

        [ShowInInspector, BoxGroup("Parameters"), ReadOnly, PropertyOrder(4)]
        private Dictionary<string, InfoDrugClientsTransition> d_allInfoObjectClientsTransition = new Dictionary<string, InfoDrugClientsTransition>();

        [SerializeField, BoxGroup("Parameters"), MinValue(0.0f), PropertyOrder(5)]
        private float[] _weightToSellArray;

        [SerializeField, BoxGroup("Parameters"), TitleGroup("Parameters/Population"), HideLabel, PropertyOrder(6)]
        [HorizontalGroup("Parameters/Population/Hor"), MinValue(0.1f), MaxValue(1.0f), Title("Change % Max", horizontalLine: false)]
        private float _populationChangeStepPercentMax;

        [SerializeField, BoxGroup("Parameters"), TitleGroup("Parameters/Population"), HideLabel, MinValue(-1.0f), MaxValue(-0.01f)]
        [HorizontalGroup("Parameters/Population/Hor"), Title("Change % Min", horizontalLine: false), PropertyOrder(7)]
        private float _populationChangeStepPercentMin;

        [BoxGroup("Parameters"), Title("Max Capacity City Stock")]
        [MinValue(0.0f), HideLabel, PropertyOrder(8), SerializeField]
        private float _maxCapacityStock;

        [Title("Min Clamp Demand Buyers", horizontalLine: false), HorizontalGroup("Parameters/Drugs/ClampDemand"), PropertyOrder(9)]
        [MinValue(1), HideLabel, SerializeField, MaxValue("@_buyersDrugsClampDemandMax - 1"), TitleGroup("Parameters/Drugs")]
        private float _buyersDrugsClampDemandMin = 3;
        float ICityControlSell.buyersDrugsClampDemandMin => _buyersDrugsClampDemandMin;

        [TitleGroup("Parameters/Drugs"), Title("Max", horizontalLine: false), SerializeField, PropertyOrder(10)]
        [MinValue("@_buyersDrugsClampDemandMin + 1"), HideLabel, HorizontalGroup("Parameters/Drugs/ClampDemand")]
        private float _buyersDrugsClampDemandMax = 4;
        float ICityControlSell.buyersDrugsClampDemandMax => _buyersDrugsClampDemandMax;

        [SerializeField, Title("Upload Resource", horizontalLine: false), HideLabel, PropertyOrder(11)]
        [ShowIf("@_connectingObject != null"), BoxGroup("Parameters"), MinValue(0.0f)]
        private float _uploadResourceAddWay;

        [SerializeField, BoxGroup("Parameters"), TitleGroup("Parameters/Population"), HideLabel, PropertyOrder(12)]
        [MinValue(0), MaxValue(double.MaxValue / 2), Title("Population", horizontalLine: false)]
        private uint _populationCity;
        public uint populationCity => _populationCity;

        [SerializeField, BoxGroup("Parameters"), Title("Police Level (0-10)", horizontalLine: false), PropertyOrder(13)]
        [MinValue(0), MaxValue(10), Tooltip("Уровень полиции в данном городе"), PropertySpace(5), HideLabel]
        private byte _policeLevel;

        private byte _connectObjectsCount = 0;

        private string _gameObjectConnectionIndex;


        public void InitAwake()
        {
            SearchComponents();

            void SearchComponents()
            {
                _spriteRendererObject = GetComponent<SpriteRenderer>();
                _timeDateControl = FindObjectOfType<TimeDateControl>();

                _IcityControlSell = this;
                _IcityDrugBuyers = GetComponent<DrugBuyersContractControl>();
                _roadControl = FindObjectOfType<RoadControl>();

                CreateComponents();
            }

            void CreateComponents()
            {
                if (_configCityControlView is not null) _IcityView = new CityControlView(_configCityControlView);

                _cityReproduction = new CityReproduction(c_mathematicalDivisor, _populationChangeStepPercentMax, _populationChangeStepPercentMin);

                _cityTreasury = new();
                _cancellationTokenSource = new();
                _IcityDrugSell = new CityDrugsSell(_cityTreasury);
                _coroutineTimeStep = new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));

                InvokeComponents();
            }

            void InvokeComponents()
            {
                GetComponent<CityBusinessControl>().InitAwake();

                SetBuyersDrugsAsync();
                SubmittingResourcesAsync();
                StartCoroutine(Reproduction());
            }
        }

        public void ConnectObjectToObject(string typeFabricDrug, string gameObjectConnectionIndex,
                                          IPluggableingRoad FirstObject, IPluggableingRoad SecondObject)
        {
            if (_connectObjectsCount < c_maxConnectionObjects)
            {
                _gameObjectConnectionIndex = gameObjectConnectionIndex;
                _connectObjectsCount++;

                if (!d_amountDrugsInCity.ContainsKey(typeFabricDrug)) { d_amountDrugsInCity.Add(typeFabricDrug, 0); }
                _roadControl.BuildRoad(transform.position, SecondObject.GetPositionVector2(), gameObjectConnectionIndex);
            }

            if (_connectObjectsCount! > 0) _IcityView.Connect(ref _spriteRendererObject);
        }

        public void DisconnectObjectToObject(string gameObjectDisconnectTo)
        {
            if (_connectObjectsCount > 0)
            {
                _roadControl.DestroyRoad(gameObjectDisconnectTo);
                _connectObjectsCount--;

                if (_connectObjectsCount == 0) _IcityView.Disconnect(ref _spriteRendererObject);
            }
        }

        public Vector2 GetPositionVector2() => transform.position;

        public void IngestResources(string typeFabricDrug, in bool isWork, in float addResEveryStep)
        {
            if (isWork && d_amountDrugsInCity[typeFabricDrug] < _maxCapacityStock)
                d_amountDrugsInCity[typeFabricDrug] += addResEveryStep;

            _roadControl.DecliningDemandUpdate(addResEveryStep, typeFabricDrug, _gameObjectConnectionIndex);
            SellResources(in typeFabricDrug);

            void SellResources(in string typeFabricDrug)
            {
                var allTypeDrugs = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

                foreach (var contractBuyers in _IcityDrugBuyers.d_contractBuyers.Keys)
                    _IcityDrugSell.Sell(d_weightToSellDrugs[typeFabricDrug], contractBuyers, typeFabricDrug, _IcityControlSell);
            }
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.MediumImportant, isSingle: false);
        }

        private void OnApplicationQuit() { _cancellationTokenSource.Cancel(); }

        private async Task SetBuyersDrugsAsync()
        {
            await Task.Run(() =>
            {
                SetWeightToSellDrugsAsync();
                var countsDrugBuyers = Enum.GetNames(typeof(DrugBuyers.AllBuyers));

                System.Random systemRandom = new System.Random();

                for (int i = 0; i < countsDrugBuyers.Length / 2; i++) //!хардкод
                {
                    var addRandomBuyer = systemRandom.Next(0, countsDrugBuyers.Length);

                    if (!_IcityDrugBuyers.d_contractBuyers.ContainsKey(countsDrugBuyers[addRandomBuyer]))
                        _IcityDrugBuyers.d_contractBuyers.Add(countsDrugBuyers[addRandomBuyer], new ContractBuyerInfo());
                }
                systemRandom = null;
            });

            async Task SetWeightToSellDrugsAsync()
            {
                await Task.Run(() =>
                {
                    var countsDrugBuyers = Enum.GetNames(typeof(FabricControl.TypeProductionResource));

                    if (_weightToSellArray.Length < countsDrugBuyers.Length)
                        _weightToSellArray = new float[countsDrugBuyers.Length];

                    for (int i = 0; i < countsDrugBuyers.Length; i++)
                    {
                        if (!d_weightToSellDrugs.ContainsKey(countsDrugBuyers[i]))
                            d_weightToSellDrugs.Add(countsDrugBuyers[i], _weightToSellArray[i]);
                        else
                            d_weightToSellDrugs[countsDrugBuyers[i]] = _weightToSellArray[i];
                    }
                });
            }
        }

        private IEnumerator Reproduction()
        {
            while (true)
            {
                if (!_timeDateControl.GetStatePaused()) { _cityReproduction.ReproductionPopulation(ref _populationCity); }
                yield return _coroutineTimeStep;
            }
        }

        private async ValueTask SubmittingResourcesAsync()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    foreach (var item in d_allInfoObjectClientsTransition.Keys)
                    {
                        foreach (var typeDrug in d_allInfoObjectClientsTransition[item].d_typeDrugAndAmountTransition.Keys)
                        {
                            if (d_amountDrugsInCity[typeDrug] >= d_allInfoObjectClientsTransition[item].d_typeDrugAndAmountTransition[typeDrug])
                            {
                                foreach (var allClients in d_allInfoObjectClientsTransition[item].d_allClientObjects.Keys)
                                {
                                    for (int i = 0; i < d_allInfoObjectClientsTransition[item].d_allClientObjects[allClients].Length; i++)
                                    {
                                        if (d_allInfoObjectClientsTransition[item].d_allClientObjects[allClients][i] is not null)
                                        {
                                            _pluggableObject = d_allInfoObjectClientsTransition[item].d_allClientObjects[allClients][i];
                                            _pluggableObject.IngestResources(typeDrug, true, d_allInfoObjectClientsTransition[item].d_typeDrugAndAmountTransition[typeDrug]);
                                            d_amountDrugsInCity[typeDrug] -= d_allInfoObjectClientsTransition[item].d_typeDrugAndAmountTransition[typeDrug];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }, _cancellationTokenSource.Token);

                await Task.Delay((int)(_timeDateControl.GetCurrentTimeOneDay(true) * 1000));

                if (_cancellationTokenSource.IsCancellationRequested)
                    return;
            }
        }


#if UNITY_EDITOR
        [Button("New Way"), BoxGroup("Parameters"), TitleGroup("Parameters/Control")]
        [ShowIf("@_connectingObject != null"), PropertySpace(10), PropertyOrder(14)]
        private void AddNewTransportWay()
        {
            if (_connectObjectsCount < c_maxConnectionObjects)
            {
                _connectObjectsCount++;
                _roadBuilded = new RoadBuilded(transform.position, _connectingObject.GetPositionVector2());
                string nameInfoClients = _connectingObject.ToString() + _typeProductionResource.ToString();

                d_allInfoObjectClientsTransition.Add(nameInfoClients, new InfoDrugClientsTransition());

                if (!d_allInfoObjectClientsTransition[nameInfoClients].d_allClientObjects.ContainsKey(this))
                {
                    _roadBuilded.roadResourcesManagement.CreateNewRoute(this, _connectingObject);

                    d_allInfoObjectClientsTransition[nameInfoClients].d_allClientObjects.Add(
                        this, _roadBuilded.roadResourcesManagement.CheckAllConnectionObjectsRoad(this));

                    d_allInfoObjectClientsTransition[nameInfoClients].d_typeDrugAndAmountTransition.Add(
                        _typeProductionResource.ToString(), _uploadResourceAddWay);

                    _connectingObject.ConnectObjectToObject(_typeProductionResource.ToString(), gameObject.name
                        + _connectingObject.ToString(), _connectingObject, this);
                }

                _connectingObject = null;
                _uploadResourceAddWay = 0;
            }
        }

        [Button("Remove Way"), BoxGroup("Parameters"), TitleGroup("Parameters/Control"), PropertySpace(10)]
        [ShowIf("@_connectingObject != null && d_allInfoObjectClientsTransition.Count > 0"), PropertyOrder(14)]
        private void RemoveTransportWay()
        {
            if (_connectObjectsCount > 0)
            {
                _connectObjectsCount--;
                string nameInfoClients = _connectingObject.ToString() + _typeProductionResource.ToString();

                if (d_allInfoObjectClientsTransition.ContainsKey(nameInfoClients))
                {
                    _roadBuilded.roadResourcesManagement.DestroyRoute(this, _connectingObject);
                    d_allInfoObjectClientsTransition[nameInfoClients].d_allClientObjects.Remove(this);
                    d_allInfoObjectClientsTransition[nameInfoClients].d_typeDrugAndAmountTransition.Remove(_typeProductionResource.ToString());
                    _connectingObject.DisconnectObjectToObject(gameObject.name + _connectingObject.ToString());
                    d_allInfoObjectClientsTransition.Remove(nameInfoClients);
                    _connectingObject = null;
                }
            }
        }
#endif
    }
}
