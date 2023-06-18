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

        [BoxGroup("Parameters")]
        [SerializeField, Required, BoxGroup("Parameters/Links"), Title("Time Date Control", horizontalLine: false), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, ReadOnly, BoxGroup("Parameters/Toggles"), LabelText("Buyed")]
        private bool _isBuyed;

        [SerializeField, ReadOnly, BoxGroup("Parameters/Toggles"), LabelText("Work")]
        private bool _isWork;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Product Quality in %", horizontalLine: false)]
        [MinValue(10.0f), MaxValue(100.0f), HideLabel]
        private float _productQualityCocaine;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Productivity Production Cocaine in kg/day", horizontalLine: false), HideLabel]
        [MinValue(0.0f)]
        private float _productivityKgPerDayCocaine;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Current Free Production Cocaine in kg/day", horizontalLine: false), HideLabel]
        [MinValue(0.0f), ReadOnly]
        private float _currentFreeProductionKgPerDayCocaine;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Product in Stock Cocaine in kg", horizontalLine: false), HideLabel]
        [MinValue(0.0f)]
        private float _productInStockCocaine;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Security Level in Star (0-10)", horizontalLine: false), HideLabel]
        [MinValue(0), MaxValue(10)]
        private byte _securityLevel;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Level Suspicion in %", horizontalLine: false), HideLabel]
        [MinValue(0.0f), MaxValue(100.0f)]
        private float _levelSuspicion;

        [SerializeField, FoldoutGroup("Parameters/Main/Additional"), Title("Max Capacity Stock in kg", horizontalLine: false), HideLabel]
        [MinValue(10.0f)]
        private float _maxCapacityStock;

        [SerializeField, FoldoutGroup("Parameters/Main/Additional"), Title("Buy Fabric Cost in $", horizontalLine: false), HideLabel]
        [MinValue(10000)]
        private double _fabricBuyCost = 10000;

        [SerializeField, FoldoutGroup("Parameters/Main/Additional"), Title("Sell Fabric Cost in $", horizontalLine: false), HideLabel]
        [MinValue(5000)]
        private double _fabricSellCost = 5000;

        [SerializeField, BoxGroup("Parameters"), PropertySpace(10, 10)]
        [Tooltip("Города, в которые фабрика будет поставлять ресурс"), ReadOnly]
        private List<CityControl> _citiesClients = new List<CityControl>();

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting"), Required]
        [EnableIf("_isBuyed")]
        private CityControl _cityNewTransportWay;

        private enum TypeUploadResource
        {
            Cocaine
        }
        [SerializeField, EnumPaging, FoldoutGroup("Parameters/Control/Transporting")]
        [Title("Type Upload Resource", horizontalLine: false), HideLabel, EnableIf("_isBuyed")]
        private TypeUploadResource _typeUploadResource;

        [SerializeField, FoldoutGroup("Parameters/Control/Transporting/Cocaine"), EnableIf("_typeUploadResource", TypeUploadResource.Cocaine)]
        [MinValue(0.0f), EnableIf("_isBuyed"), Title("Upload Resource", horizontalLine: false), HideLabel]
        private float _uploadResourceCocaine;

        #endregion


        //#if UNITY_EDITOR //!Аналоги реализовать в скрипте, ответственном за UI для фабрик
        #region Editor Extension

        [Button("Buy Fabric", 30), HideIf("_isBuyed"), FoldoutGroup("Parameters/Control")]
        private void BuyFabricEditor()
        {
            if (DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(_fabricBuyCost, true))
            {
                _isBuyed = true;
                Debug.Log("Фабрика куплена");
            }
        }

        [Button("Sell Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control")]
        private void SellFabricEditor()
        {
            _isBuyed = false;
            _isWork = false;
        }

        [Button("Work Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control")]
        private void WorkFabricEditor()
        {
            _isWork = !_isWork;
        }

        [Button("Add Transport Way"), EnableIf("_isBuyed"), FoldoutGroup("Parameters/Control/Transporting")]
        private void AddNewTransportWay()
        {
            _citiesClients.Add(_cityNewTransportWay);
            _cityNewTransportWay = null;

            if (_uploadResourceCocaine < _currentFreeProductionKgPerDayCocaine)
                _currentFreeProductionKgPerDayCocaine -= _uploadResourceCocaine;
            else
            {
                _uploadResourceCocaine = _currentFreeProductionKgPerDayCocaine;
                _currentFreeProductionKgPerDayCocaine -= _uploadResourceCocaine;
            }
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

        private void OnMouseDown()
        {
            Debug.Log(gameObject.name);
        }

        private void SetParametersFabricProduction()
        {
            _IFabricProduction = new FabricProduction();
            _currentFreeProductionKgPerDayCocaine = _productivityKgPerDayCocaine;
        }

        private void TransportingResourcesProduction()
        {
            if (_citiesClients.Count != 0)
            {
                for (int i = 0; i < _citiesClients.Count; i++)
                {
                    if (_citiesClients[i].CheckCurrentCapacityStock())
                    {
                        _citiesClients[i].IngestResources(_uploadResourceCocaine);
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
                        _IFabricProduction.ProductionProduct(_productivityKgPerDayCocaine, _maxCapacityStock, ref _productInStockCocaine);
                        TransportingResourcesProduction();
                    }
                }
                else { SetParametersFabricProduction(); }
                yield return new WaitForSecondsRealtime(_timeDateControl.GetCurrentTimeOneDay());
            }
        }
    }
}