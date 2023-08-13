using System.Threading;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Boot;
using TimeControl;
using Data;
using System.Collections.Generic;
using Config.FabricControl.View;
using Upgrade.Buildings.Fabric;
using Road;
using System.Threading.Tasks;
using DebugCustomSystem;


namespace Fabric
{
    [RequireComponent(typeof(Upgrade.UpgradeControl))]
    public sealed class FabricControl : MonoBehaviour, IBoot, IUpgradableFabric, IPluggableingRoad
    {
        #region Variables
        private static byte _maxConnectionObjects = 4;

        private IFabricProduction _IfabricProduction;

        private IFabricView _IfabricView;

        [BoxGroup("Parameters", centerLabel: true), TabGroup("Parameters/Tabs", "Links")]
        [SerializeField, Required, Title("Time Date Control", horizontalLine: false), HideLabel]
        [EnableIf("@_timeDateControl == null")]
        private TimeDateControl _timeDateControl;

        [SerializeField, BoxGroup("Parameters", centerLabel: true), Required, Title("Road Control"), HideLabel]
        [TabGroup("Parameters/Tabs", "Links"), EnableIf("@_roadControl == null")]
        private RoadControl _roadControl;

        private RoadBuilded _roadBuilded;

        [BoxGroup("Parameters", centerLabel: true), TabGroup("Parameters/Tabs", "Links")]
        [SerializeField, Required, Title("Config Fabric Control View", horizontalLine: false), HideLabel]
        [EnableIf("@_configFabricControlView == null")]
        private ConfigFabricControlView _configFabricControlView;

        private SpriteRenderer _spriteRendererObject;

        [TabGroup("Parameters/Tabs", "Toggles")]
        [SerializeField, ReadOnly, LabelText("Buyed"), ToggleLeft]
        private bool _isBuyed;

        [SerializeField, ReadOnly, TabGroup("Parameters/Tabs", "Toggles"), LabelText("Work"), ToggleLeft]
        private bool _isWork;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Product Quality Local Max", horizontalLine: false)]
        [MinValue("@_currentProductQuality"), MaxValue(95.0f), HideLabel]
        private float _productQualityLocalMax;
        public float productQualityLocalMax { get => _productQualityLocalMax; set => _productQualityLocalMax = value; }

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Product Quality", horizontalLine: false)]
        [MinValue(10.0f), MaxValue("@_productQualityLocalMax"), HideLabel]
        private float _currentProductQuality;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Product Quality Local Step Upgrade", horizontalLine: false)]
        [MinValue(0.01f), MaxValue(1.0f), HideLabel]
        private float _productQualityLocalStepUpgrade = 0.01f;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Productivity Production", horizontalLine: false), HideLabel]
        [MinValue(0.0f), SuffixLabel("kg/day")]
        private float _productivityKgPerDay;
        public float productivityKgPerDay { get => _productivityKgPerDay; set => _productivityKgPerDay = value; }

        //?
        // [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Current Free Production", horizontalLine: false), HideLabel]
        // [MinValue(0.0f), ReadOnly, SuffixLabel("kg/day")]
        //private float _currentFreeProductionKgPerDay;
        //public float currentFreeProductionKgPerDay { get => _currentFreeProductionKgPerDay; set => _currentFreeProductionKgPerDay = value; }

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Product in Stock", horizontalLine: false), HideLabel]
        [MinValue(0.0f), ReadOnly, SuffixLabel("kg")]
        private float _productInStock;
        public float productInStock => _productInStock;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Security Level", horizontalLine: false), HideLabel]
        [MinValue(0), MaxValue(10), SuffixLabel("Star (0-10)")]
        private byte _securityLevel;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Level Suspicion", horizontalLine: false), HideLabel]
        [MinValue(0.0f), MaxValue(100.0f), SuffixLabel("%")]
        private float _levelSuspicion;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Max Capacity Stock", horizontalLine: false), HideLabel]
        [MinValue(10.0f), SuffixLabel("kg")]
        private float _maxCapacityStock;
        public float maxCapacityStock { get => _maxCapacityStock; set => _maxCapacityStock = value; }

        public enum TypeProductionResource { Cocaine, Marijuana, Crack }

        [SerializeField, EnumPaging, BoxGroup("Parameters/Main Settings"), DisableIf("_isBuyed")]
        [Title("Type Production Resource", horizontalLine: false), HideLabel]
        private TypeProductionResource _typeProductionResource;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Buy", horizontalLine: false), HideLabel]
        [MinValue(10000), HorizontalGroup("Parameters/Main Settings/Fabric Cost"), SuffixLabel("$")]
        private double _fabricBuyCost = 10_000;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Sell", horizontalLine: false), HideLabel]
        [MinValue(5000), HorizontalGroup("Parameters/Main Settings/Fabric Cost"), SuffixLabel("$")]
        private double _fabricSellCost = 5_000;

        [ShowInInspector, FoldoutGroup("Parameters/Control/Transporting"), EnableIf("_isBuyed")]
        [HideLabel, Title("City New Transport Way Link", horizontalLine: false)]
        private IPluggableingRoad _connectingObject;
        public IPluggableingRoad connectingObject => _connectingObject;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), ShowIf("@_connectingObject != null")]
        [MinValue(0.0f), EnableIf("_isBuyed"), Title("Upload Resource", horizontalLine: false), HideLabel, SuffixLabel("kg")]
        private float _uploadResourceAddWay;
        public float uploadResourceAddWay => _uploadResourceAddWay;

        [BoxGroup("Parameters"), Title("Connect Objects", horizontalLine: false), SerializeField, ReadOnly]
        [MinValue(0), HideLabel]
        private byte _connectObjectsCount = 0;
        public byte connectObjectsCount => _connectObjectsCount;

        [ShowInInspector, FoldoutGroup("Parameters/Control"), ReadOnly]
        private Dictionary<string, InfoDrugClientsTransition> d_allInfoObjectClientsTransition = new Dictionary<string, InfoDrugClientsTransition>();
        Dictionary<string, InfoDrugClientsTransition> IPluggableingRoad.d_allInfoObjectClientsTransition => d_allInfoObjectClientsTransition;

        private WaitForSeconds _coroutineTimeStep;

        private CancellationTokenSource _cancellationTokenSource;

        #endregion


        void IBoot.InitAwake()
        {
            if (_timeDateControl is null) { _timeDateControl = FindObjectOfType<TimeDateControl>(); }
            if (_spriteRendererObject is null) { _spriteRendererObject = GetComponent<SpriteRenderer>(); }
            _cancellationTokenSource = new CancellationTokenSource();

            SetFabricProduction();
        }

        private void OnApplicationQuit()
        {
            _cancellationTokenSource.Cancel();
        }

        private void SetFabricProduction()
        {
            _IfabricView = new FabricControlView(_configFabricControlView);
            _IfabricProduction = new FabricProduction();
            _coroutineTimeStep = new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay(true));

            StartCoroutine(FabricWork());
        }

        private IEnumerator FabricWork()
        {
            while (true)
            {
                if (!_timeDateControl.GetStatePaused())
                {
                    if (_isWork)
                        _IfabricProduction.ProductionProduct(_productivityKgPerDay,
                                                             _maxCapacityStock, ref _productInStock);

                    TransportingResourcesProductionAsync();
                }
                yield return _coroutineTimeStep;
            }
        }

        private async ValueTask TransportingResourcesProductionAsync()
        {
            await Task.Run(() =>
            {
                LocalUpgradeProductQualityAsync();

                if (d_allInfoObjectClientsTransition.Count > 0)
                {
                    foreach (string allInfoDictionaryItems in d_allInfoObjectClientsTransition.Keys)
                    {
                        if (d_allInfoObjectClientsTransition[allInfoDictionaryItems].d_allClientObjects.Count != 0)
                        {
                            for (int i = 0; i < d_allInfoObjectClientsTransition[allInfoDictionaryItems].d_allClientObjects.Count; i++)
                            {
                                if (_productInStock >= d_allInfoObjectClientsTransition[allInfoDictionaryItems].d_typeDrugAndAmountTransition[_typeProductionResource.ToString()])
                                {
                                    d_allInfoObjectClientsTransition[allInfoDictionaryItems].d_allClientObjects[this][i].IngestResources(_typeProductionResource.ToString(),
                                        _isWork,
                                        d_allInfoObjectClientsTransition[allInfoDictionaryItems].d_typeDrugAndAmountTransition[_typeProductionResource.ToString()]);

                                    _productInStock -= d_allInfoObjectClientsTransition[allInfoDictionaryItems].d_typeDrugAndAmountTransition[_typeProductionResource.ToString()];
                                }
                            }
                        }
                    }
                }

                async ValueTask LocalUpgradeProductQualityAsync()
                {
                    await Task.Run(() =>
                    {
                        if (_isWork && _isBuyed)
                            if (_currentProductQuality < _productQualityLocalMax)
                                _currentProductQuality += _productQualityLocalStepUpgrade;
                    });
                }
            }, _cancellationTokenSource.Token);

            if (_cancellationTokenSource.IsCancellationRequested)
                return;
        }

        public void ConnectObjectToObject(string typeFabricDrug, string gameObjectConnectionTo, IPluggableingRoad FirstObject, IPluggableingRoad SecondObject)
        {
            if (_connectObjectsCount < _maxConnectionObjects)
            {
                _connectObjectsCount++;
                _roadControl.BuildRoad(transform.position, transform.position, gameObjectConnectionTo);
            }
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (typeLoad: Bootstrap.TypeLoadObject.MediumImportant, isSingle: false);
        }

        public void DisconnectObjectToObject(string gameObjectDisconnectTo)
        {
            throw new System.NotImplementedException();
        }

        public void IngestResources(string typeFabricDrug, in bool isWork, in float addResEveryStep)
        {
            Debug.Log("bbbb");
        }

        public Vector2 GetPositionVector2() { return transform.position; }


        #region Editor Extension

#if UNITY_EDITOR
        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), ShowIf("@_connectingObject != null")]
        [MinValue(0), EnableIf("_isBuyed"), Title("Index Change City Declining Demand", horizontalLine: false), HideLabel]
        private ushort _indexChangeCityDecliningDemand;

        [Button("Buy Fabric", 30), HideIf("_isBuyed"), FoldoutGroup("Parameters/Control"), GUIColor("#15e90f")]
        [PropertySpace(15)]
        private void BuyFabricEditor()
        {
            if (DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(_fabricBuyCost, true))
            {
                _isBuyed = true;
                _IfabricView.BuyFabricView(ref _spriteRendererObject);
            }
        }

        [Button("Work Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control"), PropertySpace(15)]
        private void WorkFabricEditor()
        {
            _isWork = !_isWork;
            _IfabricView.ChangeWorkStateFabricView(ref _spriteRendererObject, _isWork);
        }

        [Button("Sell Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control"), GUIColor("#e90f1f")]
        private void SellFabricEditor()
        {
            _isBuyed = false;
            _isWork = false;
            DataControl.IdataPlayer.AddPlayerMoney(_fabricSellCost);
            _IfabricView.SellFabricView(ref _spriteRendererObject);
        }

        [Button("New Way"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting"), PropertySpace(10)]
        [ShowIf("@_connectingObject != null"), HorizontalGroup("Parameters/Control/Transporting/Way")]
        private void AddNewTransportWay()
        {
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

#if UNITY_EDITOR
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"Created Way from {this} to {_connectingObject} | Upload Res: {_uploadResourceAddWay}", "Fabric", "Way");
#endif

            _connectingObject = null;
            _uploadResourceAddWay = 0;
        }

        [Button("Remove Way"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting"), PropertySpace(10)]
        [ShowIf("@_connectingObject != null"), HorizontalGroup("Parameters/Control/Transporting/Way")]
        private void RemoveTransportWay()
        {
            if (_connectingObject is null)
            {
                _connectingObject.DisconnectObjectToObject(gameObject.name + _connectingObject.ToString());
                d_allInfoObjectClientsTransition.Remove(_connectingObject.ToString() + _typeProductionResource.ToString());
                _roadBuilded.roadResourcesManagement.DestroyRoute(this, _connectingObject);

#if UNITY_EDITOR
                DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"Remove Way from {this} to {_connectingObject}", "Fabric", "Way");
#endif
            }
        }

        [Button("Load Res"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_uploadResourceAddWay != 0 && _connectingObject != null"), PropertySpace(5, 10)]
        private void AddUploadResourceWay()
        {
            string nameClient = _connectingObject.ToString() + _typeProductionResource.ToString();
            d_allInfoObjectClientsTransition[nameClient].d_typeDrugAndAmountTransition[_typeProductionResource.ToString()] += _uploadResourceAddWay;

#if UNITY_EDITOR
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"Connecting object ({nameClient}) upload resource {_uploadResourceAddWay}", "Fabric", "Resources");
#endif
        }

        [Button("Unload Res"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting"), PropertySpace(5, 10)]
        [ShowIf("@_uploadResourceAddWay != 0 && _connectingObject != null"), HorizontalGroup("Parameters/Control/Transporting/Upload")]
        private void ReduceUploadResourceWay()
        {
            string nameClient = _connectingObject.ToString() + _typeProductionResource.ToString();
            d_allInfoObjectClientsTransition[nameClient].d_typeDrugAndAmountTransition[_typeProductionResource.ToString()] -= _uploadResourceAddWay;

#if UNITY_EDITOR
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, $"Connecting object ({nameClient}) reduce resource {_uploadResourceAddWay}", "Fabric", "Resources");
#endif
        }
#endif
        #endregion
    }
}
