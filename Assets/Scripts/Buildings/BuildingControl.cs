using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Config.Building;
using Building.Farm;
using Transport.Reception;
using Resources;
using Building.Additional;
using Building.Border;
using Building.Fabric;
using Building.City;
using Building.Aerodrome;
using Building.Stock;
using Building.SeaPort;
using Building.City.Business;
using Climate;
using Expense;
using static Boot.Bootstrap;
using Config.Expenses;
using System;

namespace Building
{
    [RequireComponent(typeof(TransportReception))]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class BuildingControl : MonoBehaviour, IBoot, IBuildingRequestForTransport
    {
        [ShowInInspector, ReadOnly]
        private IBuilding _Ibuilding;

        private ISpending _Ispending;

        private IEnergyConsumption _IenergyConsumption;

        private IBuildingJobStatus _IbuildingJobStatus;

        private IBuildingPurchased _IbuildingPurchased;

        private ICityBusiness _IcityBusiness;
        
        //TODO: remove to main config (variable: _configSO)
        [SerializeField, Required, BoxGroup("Parameters"), HideLabel]
        private ConfigExpensesManagementEditor _configExpenses; 

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel, PropertySpace(0, 5)]
        private ScriptableObject _configSO; //TODO: make general config

        private TimeDateControl _timeDateControl;

        private WaitForSeconds _coroutineTimeStep;

        public enum TypeBuilding : byte
        {
            City, Fabric, Farm, Aerodrome, SeaPort, Stock, Border
        }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeBuilding _typeBuilding;


        private BuildingControl() { }

        void IBoot.InitAwake()
        {
            Find();

            void Find()
            {
                _timeDateControl = FindObjectOfType<TimeDateControl>();
                Create();
            }

            void Create()
            {
                _coroutineTimeStep = new WaitForSeconds(_timeDateControl.GetCurrentTimeOneDay());

                if (_configSO != null)
                    CreateInstance();

                if (_Ibuilding is ISpending)
                    _Ispending = (ISpending)_Ibuilding;

                if (_Ibuilding is ICityBusiness)
                    _IcityBusiness = (ICityBusiness)_Ibuilding;

                Invoke();
            }

            void Invoke()
            {
                ConnectExpensesManagementControl();
                CreateDictionaryTypeDrugs();
                StartCoroutine(ConstantUpdating());
            }
        }

        private void ConnectExpensesManagementControl()
        {
            if (_Ibuilding is IUsesExpensesManagement)
            {
                IExpensesManagement IexpensesManagement = FindObjectOfType<ExpenseManagementControl>();
                IUsesExpensesManagement IusesExpensesManagement = (IUsesExpensesManagement)_Ibuilding;

                //TODO: remove ConfigExpenses to the shared config and pass from there
                if (IexpensesManagement != null && _configExpenses != null)
                    IusesExpensesManagement.LoadExpensesManagement(IexpensesManagement, _configExpenses);
                else
                    throw new NullReferenceException();
            }
        }

        private void CreateInstance()
        {
            if (_typeBuilding is TypeBuilding.City)
                _Ibuilding = new BuildingCity(_configSO);
            else if (_typeBuilding is TypeBuilding.Farm)
                _Ibuilding = new BuildingFarm(_configSO);
            else if (_typeBuilding is TypeBuilding.Fabric)
                _Ibuilding = new BuildingFabric(_configSO);
            else if (_typeBuilding is TypeBuilding.Stock)
                _Ibuilding = new BuildingStock(_configSO);
            else if (_typeBuilding is TypeBuilding.Border)
                _Ibuilding = new BuildingBorder(_configSO);
            else if (_typeBuilding is TypeBuilding.Aerodrome)
                _Ibuilding = new BuildingAerodrome(_configSO);
            else if (_typeBuilding is TypeBuilding.SeaPort)
                _Ibuilding = new BuildingSeaPort(_configSO);
        }

        private void CreateDictionaryTypeDrugs()
        {
            if (_Ibuilding is not BuildingBorder)
                _Ibuilding.InitDictionaries();
        }

        private void ChangeOwnerState(in bool isBuy)
        {
            if (_Ibuilding is IBuildingPurchased)
            {
                _IbuildingPurchased = (IBuildingPurchased)_Ibuilding;

                if (isBuy) _IbuildingPurchased.Buy();
                else _IbuildingPurchased.Sell();
            }
        }

        private void ChangeJobStatusBuilding(in bool isState)
        {
            if (_Ibuilding is IBuildingJobStatus)
            {
                _IbuildingJobStatus = (IBuildingJobStatus)_Ibuilding;
                _IbuildingJobStatus.ChangeJobStatus(isState);
            }
        }

        private void ChangeFarmType(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            if (_Ibuilding is IChangedFarmType)
            {
                IChangedFarmType IchangedType = (IChangedFarmType)_Ibuilding;
                IchangedType.ChangeType(typeFarm);
            }
        }

        private void UpdateSpendingBuildings()
        {
            if (_IbuildingJobStatus != null && _IbuildingJobStatus.isWorked)
                _Ispending?.Spending();
        }

        private void MonitorEnergy()
        {
            if (_Ibuilding is IEnergyConsumption)
            {
                _IenergyConsumption ??= (IEnergyConsumption)_Ibuilding;
                _IbuildingJobStatus ??= (IBuildingJobStatus)_Ibuilding;

                if (_IbuildingJobStatus.isWorked)
                    _IenergyConsumption.MonitorEnergy(_IenergyConsumption);
            }
        }

        private IEnumerator ConstantUpdating()
        {
            while (true)
            {
                _Ibuilding.ConstantUpdatingInfo();
                UpdateSpendingBuildings();
                MonitorEnergy();
                yield return _coroutineTimeStep;
            }
        }

        float IBuildingRequestForTransport.RequestGetResource(in float transportCapacity,
            in TypeProductionResources.TypeResource typeResource)
        {
            return _Ibuilding.GetResources(transportCapacity, typeResource);
        }

        bool IBuildingRequestForTransport.RequestUnloadResource(in float quantityResource,
            in TypeProductionResources.TypeResource typeResource)
        {
            return _Ibuilding.SetResources(quantityResource, typeResource);
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.LotsOf);
        }

        public void SetClimateZone(in IClimateZone IclimateZone)
        {
            if (_Ibuilding is IUsesClimateInfo)
            {
                IUsesClimateInfo IusesClimateInfo = (IUsesClimateInfo)_Ibuilding;
                IusesClimateInfo.SetClimateZone(IclimateZone);
            }
        }


#if UNITY_EDITOR
        [Button("Buy Building"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor")]
        [DisableInEditorMode]
        private void BuyBuilding() => ChangeOwnerState(true);

        [Button("Sell Building"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor")]
        [DisableInEditorMode]
        private void SellBuilding() => ChangeOwnerState(false);

        [Button("Activate"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor2")]
        [DisableInEditorMode]
        private void SetActivateBuilding() => ChangeJobStatusBuilding(true);

        [Button("Deactivate"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor2")]
        [DisableInEditorMode]
        private void SetDeactivateBuilding() => ChangeJobStatusBuilding(false);

        #region Farm

        [Button("Change Farm Type"), BoxGroup("Editor Control | Farm"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == TypeBuilding.Farm")]
        private void ChangeFarmTypeEditor(in ConfigBuildingFarmEditor.TypeFarm typeFarm)
        {
            ChangeFarmType(typeFarm);
        }

        #endregion

        #region City-Business

        [Button("Buy"), BoxGroup("Editor Control | City Business"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == TypeBuilding.City")]
        private void BuyBusiness(in CityBusiness.TypeBusiness typeBusiness)
        {
            _IcityBusiness.BuyBusiness(typeBusiness);
        }

        [Button("Sell"), BoxGroup("Editor Control | City Business"), DisableInEditorMode]
        [ShowIf("@_typeBuilding == TypeBuilding.City")]
        private void SellBusiness(in ushort indexBusiness)
        {
            _IcityBusiness.SellBusiness(indexBusiness);
        }

        #endregion
#endif
    }
}
