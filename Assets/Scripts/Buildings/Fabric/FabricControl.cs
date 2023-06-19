using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Boot;
using TimeControl;
using Data;
using City;
using System.Collections.Generic;


namespace Fabric
{
    public sealed class FabricControl : MonoBehaviour, IBoot
    {
        #region Variables

        private IFabricProduction _IFabricProduction;

        [BoxGroup("Parameters", centerLabel: true), TabGroup("Parameters/Tabs", "Links")]
        [SerializeField, Required, Title("Time Date Control", horizontalLine: false), HideLabel]
        [EnableIf("@_timeDateControl == null")]
        private TimeDateControl _timeDateControl;

        [TabGroup("Parameters/Tabs", "Toggles")]
        [SerializeField, ReadOnly, LabelText("Buyed"), ToggleLeft]
        private bool _isBuyed;

        [SerializeField, ReadOnly, TabGroup("Parameters/Tabs", "Toggles"), LabelText("Work"), ToggleLeft]
        private bool _isWork;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Drugs"), Title("Product Quality 10%-95%", horizontalLine: false)]
        [MinValue(10.0f), MaxValue(95.0f), HideLabel]
        private float _productQuality;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Drugs"), Title("Productivity Production in kg/day", horizontalLine: false), HideLabel]
        [MinValue(0.0f)]
        private float _productivityKgPerDay;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Drugs"), Title("Current Free Production in kg/day", horizontalLine: false), HideLabel]
        [MinValue(0.0f), ReadOnly]
        private float _currentFreeProductionKgPerDay;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Drugs"), Title("Product in Stock in kg", horizontalLine: false), HideLabel]
        [MinValue(0.0f), PropertySpace(0, 15)]
        private float _productInStock;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Main"), Title("Security Level in Star (0-10)", horizontalLine: false), HideLabel]
        [MinValue(0), MaxValue(10)]
        private byte _securityLevel;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Main"), Title("Level Suspicion in %", horizontalLine: false), HideLabel]
        [MinValue(0.0f), MaxValue(100.0f)]
        private float _levelSuspicion;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Main"), Title("Max Capacity Stock in kg", horizontalLine: false), HideLabel]
        [MinValue(10.0f)]
        private float _maxCapacityStock;

        private enum TypeProductionResource
        {
            Cocaine, Marijuana
        }

        [SerializeField, EnumPaging, TabGroup("Parameters/TabsTwo", "Main")]
        [Title("Type Production Resource", horizontalLine: false), HideLabel, PropertySpace(0, 15)]
        private TypeProductionResource _typeProductionResource;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Main"), Title("Buy Fabirc Cost $", horizontalLine: false), HideLabel]
        [MinValue(10000)]
        private double _fabricBuyCost = 10_000;

        [SerializeField, TabGroup("Parameters/TabsTwo", "Main"), Title("Sell Fabric Cost $", horizontalLine: false), HideLabel]
        [MinValue(5000)]
        private double _fabricSellCost = 5_000;

        [SerializeField, FoldoutGroup("Parameters/Control"), PropertySpace(10, 10)]
        [Tooltip("Города, в которые фабрика будет поставлять ресурс"), ReadOnly]

        private List<CityControl> _citiesClients = new List<CityControl>();

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), Required, EnableIf("_isBuyed")]
        private CityControl _cityNewTransportWay;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting")]
        [MinValue(0.0f), EnableIf("_isBuyed"), Title("Upload Resource", horizontalLine: false), HideLabel]
        private float _uploadResource;

        #endregion


        //#if UNITY_EDITOR //!Аналоги реализовать в скрипте, ответственном за UI для фабрик
        #region Editor Extension

        [Button("Buy Fabric", 30), HideIf("_isBuyed"), FoldoutGroup("Parameters/Control"), GUIColor("#15e90f")]
        [PropertySpace(15)]
        private void BuyFabricEditor()
        {
            if (DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(_fabricBuyCost, true))
            {
                _isBuyed = true;
                Debug.Log("Фабрика куплена");
            }
        }

        [Button("Sell Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control"), GUIColor("#e90f1f")]
        private void SellFabricEditor()
        {
            _isBuyed = false;
            _isWork = false;
            DataControl.IdataPlayer.AddPlayerMoney(_fabricSellCost);
            Debug.Log("Фабрика продана");
        }

        [Button("Work Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control"), PropertySpace(15)]
        private void WorkFabricEditor()
        {
            _isWork = !_isWork;
        }

        [Button("Add Transport Way"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting"), PropertySpace(15)]
        private void AddNewTransportWay()
        {
            _citiesClients.Add(_cityNewTransportWay);
            _cityNewTransportWay = null;

            if (_uploadResource < _currentFreeProductionKgPerDay)
                _currentFreeProductionKgPerDay -= _uploadResource;
            else
            {
                _uploadResource = _currentFreeProductionKgPerDay;
                _currentFreeProductionKgPerDay -= _uploadResource;
            }

            _uploadResource = 0;
        }

        [Button("Clear Cities Clients"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        private void RemoveAllCitiesClients()
        {
            _citiesClients.Clear();
        }
        //#endif
        #endregion


        void IBoot.InitAwake()
        {
            if (_timeDateControl is null)
            {
                Debug.LogWarning("TimeDateControl is null in FabricControl.cs");
                _timeDateControl = FindObjectOfType<TimeDateControl>();
            }

            StartCoroutine(DrugProduction());
        }

        private void SetParametersFabricProduction()
        {
            _IFabricProduction = new FabricProduction();
            _currentFreeProductionKgPerDay = _productivityKgPerDay;
        }

        private void TransportingResourcesProduction()
        {
            if (_citiesClients.Count != 0)
            {
                for (int i = 0; i < _citiesClients.Count; i++)
                {
                    if (_citiesClients[i].CheckCurrentCapacityStock())
                    {
                        _citiesClients[i].IngestResources(_uploadResource);
                    }
                    else { Debug.Log("Хранилище города полное"); }
                }
            }
        }

        private IEnumerator DrugProduction()
        {
            while (true)
            {
                if (_IFabricProduction is not null)
                {
                    if (_isWork)
                    {
                        _IFabricProduction.ProductionProduct(_currentFreeProductionKgPerDay, _maxCapacityStock, ref _productInStock);
                        TransportingResourcesProduction();
                    }
                }
                else { SetParametersFabricProduction(); }
                yield return new WaitForSecondsRealtime(_timeDateControl.GetCurrentTimeOneDay());
            }
        }
    }
}