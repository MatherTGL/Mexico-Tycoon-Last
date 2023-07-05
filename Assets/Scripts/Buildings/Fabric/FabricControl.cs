using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Boot;
using TimeControl;
using Data;
using City;
using System.Collections.Generic;
using Config.FabricControl.View;
using Upgrade.Buildings.Fabric;


namespace Fabric
{
    [RequireComponent(typeof(Upgrade.UpgradeControl))]
    public sealed class FabricControl : MonoBehaviour, IBoot, IUpgradableFabric
    {
        #region Variables

        private IFabricProduction _IfabricProduction;

        private IFabricView _IfabricView;

        [BoxGroup("Parameters", centerLabel: true), TabGroup("Parameters/Tabs", "Links")]
        [SerializeField, Required, Title("Time Date Control", horizontalLine: false), HideLabel]
        [EnableIf("@_timeDateControl == null")]
        private TimeDateControl _timeDateControl;

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
        private float _productivityKgPerDay; //! под каждый наркотик
        public float productivityKgPerDay { get => _productivityKgPerDay; set => _productivityKgPerDay = value; }

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Current Free Production", horizontalLine: false), HideLabel]
        [MinValue(0.0f), ReadOnly, SuffixLabel("kg/day")]
        private float _currentFreeProductionKgPerDay;
        public float currentFreeProductionKgPerDay { get => _currentFreeProductionKgPerDay; set => _currentFreeProductionKgPerDay = value; }

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

        [SerializeField, FoldoutGroup("Parameters/Control"), PropertySpace(10, 10)]
        [Tooltip("Города, в которые фабрика будет поставлять ресурс"), ReadOnly]
        private List<CityControl> l_citiesClients = new List<CityControl>();

        [ShowInInspector, FoldoutGroup("Parameters/Control"), ReadOnly]
        private Dictionary<string, float> d_allInfoCitiesClientsTransition = new Dictionary<string, float>();

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), EnableIf("_isBuyed")]
        [HideLabel, Title("City New Transport Way Link", horizontalLine: false)]
        private CityControl _cityNewTransportWay;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), ShowIf("@_cityNewTransportWay != null || l_citiesClients.Count != 0")]
        [MinValue(0.0f), EnableIf("_isBuyed"), Title("Upload Resource", horizontalLine: false), HideLabel, SuffixLabel("kg")]
        private float _uploadResourceAddWay;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), ShowIf("@l_citiesClients.Count != 0")]
        [MinValue(0), EnableIf("_isBuyed"), Title("Index Change City Declining Demand", horizontalLine: false), HideLabel]
        private ushort _indexChangeCityDecliningDemand;

        #endregion


        #region Editor Extension

#if UNITY_EDITOR
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

        [Button("New Way"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_cityNewTransportWay != null"), HorizontalGroup("Parameters/Control/Transporting/Way")]
        [PropertySpace(10)]
        private void AddNewTransportWay()
        {
            if (l_citiesClients.Contains(_cityNewTransportWay) is false && d_allInfoCitiesClientsTransition.ContainsKey(_cityNewTransportWay.name) is false)
            {
                l_citiesClients.Add(_cityNewTransportWay);
                SpendFreeProduction();

                d_allInfoCitiesClientsTransition.Add(_cityNewTransportWay.name, _uploadResourceAddWay);

                _cityNewTransportWay.ConnectFabricToCity(_typeProductionResource.ToString(),
                                                         transform.position, gameObject.name + _cityNewTransportWay.name);

                _uploadResourceAddWay = 0;
                _cityNewTransportWay = null;
            }
        }

        [Button("Remove Way"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_cityNewTransportWay != null"), HorizontalGroup("Parameters/Control/Transporting/Way")]
        [PropertySpace(10)]
        private void RemoveTransportWay()
        {
            if (_cityNewTransportWay != null)
            {
                l_citiesClients.Remove(_cityNewTransportWay);
                _cityNewTransportWay.DisconnectFabricToCity(gameObject.name + _cityNewTransportWay.name);
                d_allInfoCitiesClientsTransition.Remove(_cityNewTransportWay.name);
            }
        }

        [Button("Clear Cities Clients"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@l_citiesClients.Count != 0"), PropertySpace(5, 5)]
        private void RemoveAllCitiesClients()
        {
            for (int i = 0; i < l_citiesClients.Count; i++)
                l_citiesClients[i].DisconnectFabricToCity(gameObject.name + _cityNewTransportWay.name);

            l_citiesClients.Clear();
            d_allInfoCitiesClientsTransition.Clear();
        }

        [Button("Load Res"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_uploadResourceAddWay != 0 && _cityNewTransportWay != null && l_citiesClients.Count != 0"), HorizontalGroup("Parameters/Control/Transporting/Upload")]
        [PropertySpace(5, 10)]
        private void AddUploadResourceWay()
        {
            SpendFreeProduction();
            d_allInfoCitiesClientsTransition[l_citiesClients[_indexChangeCityDecliningDemand].name] += _uploadResourceAddWay;
        }

        [Button("Unload Res"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_uploadResourceAddWay != 0 && _cityNewTransportWay != null && l_citiesClients.Count != 0"), HorizontalGroup("Parameters/Control/Transporting/Upload")]
        [PropertySpace(5, 10)]
        private void ReduceUploadResourcecWay()
        {
            ReturnFreeProduction();
            d_allInfoCitiesClientsTransition[l_citiesClients[_indexChangeCityDecliningDemand].name] -= _uploadResourceAddWay;
        }
#endif
        #endregion


        void IBoot.InitAwake()
        {
            if (_timeDateControl is null) { _timeDateControl = FindObjectOfType<TimeDateControl>(); }
            if (_spriteRendererObject is null) { _spriteRendererObject = GetComponent<SpriteRenderer>(); }

            _IfabricView = new FabricControlView(_configFabricControlView);

            SetFabricProduction();
        }

        private void SetFabricProduction()
        {
            _IfabricProduction = new FabricProduction();
            _currentFreeProductionKgPerDay = _productivityKgPerDay;

            StartCoroutine(FabricWork());
        }

        private void TransportingResourcesProduction()
        {
            if (l_citiesClients.Count != 0)
                for (int i = 0; i < l_citiesClients.Count; i++)
                    l_citiesClients[i].IngestResources(_typeProductionResource.ToString(),
                                                      _isWork, d_allInfoCitiesClientsTransition[l_citiesClients[i].name]);
        }

        private IEnumerator FabricWork()
        {
            while (true)
            {
                if (_timeDateControl.GetStatePaused() is false)
                {
                    if (_isWork is true)
                        _IfabricProduction.ProductionProduct(_currentFreeProductionKgPerDay,
                                                         _maxCapacityStock, ref _productInStock);

                    TransportingResourcesProduction();
                    LocalUpgradeProductQuality();
                }
                yield return new WaitForSecondsRealtime(_timeDateControl.GetCurrentTimeOneDay(true));
            }
        }

        private void LocalUpgradeProductQuality()
        {
            if (_isWork is true && _isBuyed is true)
            {
                if (_currentProductQuality < _productQualityLocalMax)
                    _currentProductQuality += _productQualityLocalStepUpgrade;
            }
        }

        private void SpendFreeProduction()
        {
            if (_uploadResourceAddWay < _currentFreeProductionKgPerDay)
                _currentFreeProductionKgPerDay -= _uploadResourceAddWay;
            else
            {
                _uploadResourceAddWay = _currentFreeProductionKgPerDay;
                _currentFreeProductionKgPerDay -= _uploadResourceAddWay;
            }
        }

        private void ReturnFreeProduction()
        {
            if (_currentFreeProductionKgPerDay < _productivityKgPerDay)
            {
                if (_currentFreeProductionKgPerDay >= _productivityKgPerDay)
                    _currentFreeProductionKgPerDay = _productivityKgPerDay;
                else
                    _currentFreeProductionKgPerDay += _uploadResourceAddWay;
            }
        }
    }
}