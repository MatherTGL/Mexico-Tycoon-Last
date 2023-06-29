using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Boot;
using TimeControl;
using Data;
using City;
using System.Collections.Generic;
using Config.FabricControl.View;


namespace Fabric
{
    public sealed class FabricControl : MonoBehaviour, IBoot
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

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Product Quality 10%-95%", horizontalLine: false)]
        [MinValue(10.0f), MaxValue(95.0f), HideLabel]
        private float _productQuality;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Productivity Production in kg/day", horizontalLine: false), HideLabel]
        [MinValue(0.0f)]
        private float _productivityKgPerDay;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Current Free Production in kg/day", horizontalLine: false), HideLabel]
        [MinValue(0.0f), ReadOnly]
        private float _currentFreeProductionKgPerDay;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Product in Stock in kg", horizontalLine: false), HideLabel]
        [MinValue(0.0f), ReadOnly]
        private float _productInStock;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Security Level in Star (0-10)", horizontalLine: false), HideLabel]
        [MinValue(0), MaxValue(10)]
        private byte _securityLevel;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Level Suspicion in %", horizontalLine: false), HideLabel]
        [MinValue(0.0f), MaxValue(100.0f)]
        private float _levelSuspicion;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Max Capacity Stock in kg", horizontalLine: false), HideLabel]
        [MinValue(10.0f)]
        private float _maxCapacityStock;

        public enum TypeProductionResource
        {
            Cocaine, Marijuana, Crack
        }

        [SerializeField, EnumPaging, BoxGroup("Parameters/Main Settings"), DisableIf("_isBuyed")]
        [Title("Type Production Resource", horizontalLine: false), HideLabel]
        private TypeProductionResource _typeProductionResource;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Buy $", horizontalLine: false), HideLabel]
        [MinValue(10000), HorizontalGroup("Parameters/Main Settings/Fabric Cost")]
        private double _fabricBuyCost = 10_000;

        [SerializeField, BoxGroup("Parameters/Main Settings"), Title("Sell $", horizontalLine: false), HideLabel]
        [MinValue(5000), HorizontalGroup("Parameters/Main Settings/Fabric Cost")]
        private double _fabricSellCost = 5_000;

        [SerializeField, FoldoutGroup("Parameters/Control"), PropertySpace(10, 10)]
        [Tooltip("Города, в которые фабрика будет поставлять ресурс"), ReadOnly]
        private List<CityControl> _citiesClients = new List<CityControl>();

        [ShowInInspector, FoldoutGroup("Parameters/Control")]
        private Dictionary<string, float> d_allInfoCitiesClientsTransition = new Dictionary<string, float>();

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), EnableIf("_isBuyed")]
        [HideLabel, Title("City New Transport Way Link", horizontalLine: false)]
        private CityControl _cityNewTransportWay;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), ShowIf("@_cityNewTransportWay != null || _citiesClients.Count != 0")]
        [MinValue(0.0f), EnableIf("_isBuyed"), Title("Upload Resource", horizontalLine: false), HideLabel]
        private float _uploadResourceAddWay;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), ShowIf("@_citiesClients.Count != 0")]
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
            if (_citiesClients.Contains(_cityNewTransportWay) is false && d_allInfoCitiesClientsTransition.ContainsKey(_cityNewTransportWay.name) is false)
            {
                _citiesClients.Add(_cityNewTransportWay);
                d_allInfoCitiesClientsTransition.Add(_cityNewTransportWay.name, _uploadResourceAddWay);
                SpendFreeProduction();

                _cityNewTransportWay.ConnectFabricToCity(_typeProductionResource.ToString(),
                                                         transform.position,
                                                         gameObject.name + _cityNewTransportWay.name);
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
                _citiesClients.Remove(_cityNewTransportWay);
                _cityNewTransportWay.DisconnectFabricToCity(gameObject.name + _cityNewTransportWay.name);
                d_allInfoCitiesClientsTransition.Remove(_cityNewTransportWay.name);
            }
        }

        [Button("Clear Cities Clients"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_citiesClients.Count != 0"), PropertySpace(5, 5)]
        private void RemoveAllCitiesClients()
        {
            for (int i = 0; i < _citiesClients.Count; i++)
            {
                _citiesClients[i].DisconnectFabricToCity(gameObject.name + _cityNewTransportWay.name);
            }
            _citiesClients.Clear();
            d_allInfoCitiesClientsTransition.Clear();
        }

        [Button("Load Res"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_uploadResourceAddWay != 0 && _cityNewTransportWay != null && _citiesClients.Count != 0"), HorizontalGroup("Parameters/Control/Transporting/Upload")]
        [PropertySpace(5, 10)]
        private void AddUploadResourceWay()
        {
            SpendFreeProduction();
            d_allInfoCitiesClientsTransition[_citiesClients[_indexChangeCityDecliningDemand].name] += _uploadResourceAddWay;
        }

        [Button("Unload Res"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        [ShowIf("@_uploadResourceAddWay != 0 && _cityNewTransportWay != null && _citiesClients.Count != 0"), HorizontalGroup("Parameters/Control/Transporting/Upload")]
        [PropertySpace(5, 10)]
        private void ReduceUploadResourcecWay()
        {
            ReturnFreeProduction();
            d_allInfoCitiesClientsTransition[_citiesClients[_indexChangeCityDecliningDemand].name] -= _uploadResourceAddWay;
        }
#endif
        #endregion


        void IBoot.InitAwake()
        {
            if (_timeDateControl is null)
            {
                Debug.LogWarning("TimeDateControl is null in FabricControl.cs");
                _timeDateControl = FindObjectOfType<TimeDateControl>();
            }

            SetFabricProduction();
        }

        private void SetFabricProduction()
        {
            _IfabricProduction = new FabricProduction();
            _currentFreeProductionKgPerDay = _productivityKgPerDay;

            SetFabricControlViewParameters();
            StartCoroutine(DrugProduction());
        }

        private void SetFabricControlViewParameters()
        {
            if (_spriteRendererObject is null) { _spriteRendererObject = GetComponent<SpriteRenderer>(); }
            _IfabricView = new FabricControlView(_configFabricControlView);
        }

        private void TransportingResourcesProduction()
        {
            if (_citiesClients.Count != 0)
            {
                for (int i = 0; i < _citiesClients.Count; i++)
                {
                    _citiesClients[i].IngestResources(_typeProductionResource.ToString(),
                                                      _isWork,
                                                      d_allInfoCitiesClientsTransition[_citiesClients[i].name]);
                }
            }
        }

        private IEnumerator DrugProduction()
        {
            while (true)
            {
                if (_timeDateControl.GetStatePaused() is false)
                {
                    if (_isWork)
                    {
                        _IfabricProduction.ProductionProduct(_currentFreeProductionKgPerDay,
                                                         _maxCapacityStock,
                                                         ref _productInStock);
                    }
                    TransportingResourcesProduction();
                }
                yield return new WaitForSecondsRealtime(_timeDateControl.GetCurrentTimeOneDay(true));
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