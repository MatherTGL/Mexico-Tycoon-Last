using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Boot;
using TimeControl;
using Data;


namespace Fabric
{
    public sealed class FabricControl : MonoBehaviour, IBoot
    {
        #region Variables

        private IFabricProduction _IFabricProduction;

        [BoxGroup("Parameters")]
        [SerializeField, Required, BoxGroup("Parameters/Links"), Title("Time Date Control", horizontalLine:false), HideLabel]
        private TimeDateControl _timeDateControl;

        [SerializeField, ReadOnly, BoxGroup("Parameters/Toggles"), LabelText("Buyed")]
        private bool _isBuyed;

        [SerializeField, ReadOnly, BoxGroup("Parameters/Toggles"), LabelText("Work")]
        private bool _isWork;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Product Quality in %", horizontalLine:false)]
        [MinValue(10.0f), MaxValue(100.0f), HideLabel]
        private float _productQuality;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Productivity Production in kg/day"), HideLabel]
        [MinValue(0.0f)]
        private float _productivityKgPerDay;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Product in Stock in kg"), HideLabel]
        [MinValue(0.0f)]
        private float _productInStock;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Security Level in Star (0-10)"), HideLabel]
        [MinValue(0), MaxValue(10)]
        private byte _securityLevel;

        [SerializeField, BoxGroup("Parameters/Main", false), Title("Level Suspicion in %"), HideLabel]
        [MinValue(0.0f), MaxValue(100.0f)]
        private float _levelSuspicion;

        [SerializeField, FoldoutGroup("Parameters/Main/Additional"), Title("Max Capacity Stock in kg"), HideLabel]
        [MinValue(10.0f)]
        private float _maxCapacityStock;

        [SerializeField, FoldoutGroup("Parameters/Main/Additional"), Title("Buy Fabric Cost in $"), HideLabel]
        [MinValue(10000)]
        private double _fabricBuyCost = 10000;

        [SerializeField, FoldoutGroup("Parameters/Main/Additional"), Title("Sell Fabric Cost in $"), HideLabel]
        [MinValue(5000)]
        private double _fabricSellCost = 5000;

        // [SerializeField, BoxGroup("Parameters"), PropertySpace(10, 10)]
        // [Tooltip("Города, в которые фабрика будет поставлять ресурс")]
        //private CityControl[] _citiesClients;

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
            // if (_playerData.CheckAndSpendingPlayerMoney(_fabricBuyCost, true))
            // {
            //     _isBuyed = true;
            //     gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            // }
        }

        [Button("Sell Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control")]
        private void SellFabricEditor()
        {
            _isBuyed = false;
            _isWork = false;

            // gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        [Button("Work Fabric", 30), ShowIf("_isBuyed"), FoldoutGroup("Parameters/Control")]
        private void WorkFabricEditor()
        {
            _isWork = !_isWork;
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
            _IFabricProduction = new FabricProduction(_productQuality,
                                                      _productivityKgPerDay,
                                                      _productInStock,
                                                      _maxCapacityStock);
        }

        private void GetFabricProductionParameters()
        {
            _productInStock = _IFabricProduction.GetProductInStock();
        }

        private IEnumerator DrugProduction()
        {
            while(true)
            {
                if (_IFabricProduction is not null)
                {
                    if (_isWork)
                    {
                        _IFabricProduction.ProductionProduct();
                        GetFabricProductionParameters();
                    }
                }
                else { SetParametersFabricProduction(); }
                yield return new WaitForSecondsRealtime(_timeDateControl.GetCurrentTimeOneDay()); //TODO докинуть время игры
            }
        }
    }
}