using UnityEngine;
using Sirenix.OdinInspector;
using Boot;
using TimeControl;
using System.Collections;
using Building.City;
using Config.Building;
using Building.Farm;
using Transport.Reception;
using Building.Fabric;
using DebugCustomSystem;


namespace Building
{
    [RequireComponent(typeof(Transport.Reception.TransportReception))]
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class BuildingControl : MonoBehaviour, IBoot, IBuildingRequestForTransport
    {
        private IBuilding _Ibuilding;

        [SerializeField, Required, BoxGroup("Parameters"), HideLabel, PropertySpace(0, 5)]
        private ScriptableObject _configSO;
        public ScriptableObject configSO => _configSO;

        private TimeDateControl _timeDateControl;

        private WaitForSeconds _coroutineTimeStep;

        public enum TypeBuilding : byte
        {
            City, Fabric, Farm, Aerodrome, SeaPort, Stock
        }

        [SerializeField, BoxGroup("Parameters"), EnumToggleButtons, HideLabel]
        private TypeBuilding _typeBuilding;

        [SerializeField, BoxGroup("Parameters"), ToggleLeft, ReadOnly]
        private bool _isBuyed;


        private BuildingControl() { }

        public void InitAwake()
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

                if (_configSO is not null)
                {
                    if (_typeBuilding is TypeBuilding.City)
                        _Ibuilding = new BuildingCity((ConfigBuildingCityEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Farm)
                        _Ibuilding = new BuildingFarm((ConfigBuildingFarmEditor)_configSO);
                    else if (_typeBuilding is TypeBuilding.Fabric)
                        _Ibuilding = new BuildingFabric((ConfigBuildingFabricEditor)_configSO);
                }
            }
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SuperImportant, false);
        }

        (bool inStock, float quantity) IBuildingRequestForTransport.RequestGetResource(in float transportCapacity)
        {
            Debug.Log($"пункт 3 {transportCapacity} / {_Ibuilding}");
            if (_Ibuilding.GetResources(transportCapacity).confirm)
                return (true, _Ibuilding.GetResources(transportCapacity).quantityAmount);
            else
                return (false, 0);
        }

        bool IBuildingRequestForTransport.RequestUnloadResource(in float quantityResource)
        {
            DebugSystem.Log(this, DebugSystem.SelectedColor.Green, "Отправлен запрос на склад", "Transport");
            if (_Ibuilding.SetResources(quantityResource))
                return true;
            else
                return false;
        }

        private void ChangeOwnerState(in bool isBuy)
        {
            if (isBuy == true && _isBuyed == false || isBuy == false && _isBuyed == true) _isBuyed = isBuy;

            if (_isBuyed) StartCoroutine(ConstantUpdating());
            else StopAllCoroutines();
        }

        private IEnumerator ConstantUpdating()
        {
            while (true)
            {
                _Ibuilding.ConstantUpdatingInfo();
                yield return _coroutineTimeStep;
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
        private void SetActivateBuilding() => _Ibuilding.ChangeJobStatus(true);

        [Button("Deactivate"), BoxGroup("Editor Control"), HorizontalGroup("Editor Control/Hor2")]
        [DisableInEditorMode]
        private void SetDeactivateBuilding() => _Ibuilding.ChangeJobStatus(false);
#endif
    }
}
