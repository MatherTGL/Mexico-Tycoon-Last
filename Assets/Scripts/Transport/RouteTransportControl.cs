using UnityEngine;
using Sirenix.OdinInspector;
using Route.Builder;
using System.Collections.Generic;
using System;
using DebugCustomSystem;
using Transport.Reception;
using Resources;

namespace Transport
{
    public sealed class RouteTransportControl : MonoBehaviour, ITransportInteractRoute
    {
        [ShowInInspector]
        private ICreatorCurveRoad _IcreatorCurveRoad;

        [SerializeField]
        private TypeTransport[] _allTypesTransport;

        [SerializeField, ReadOnly]
        private List<GameObject> l_purchasedTransport = new List<GameObject>();

        [SerializeField, ReadOnly]
        private List<SelfTransport> l_purchasedTransportController = new List<SelfTransport>();

        [SerializeField, ReadOnly]
        private Vector3[] _routePoints;
        public Vector3[] routePoints => _routePoints;

        private event Action _onLateUpdateAction;
        event Action ITransportInteractRoute.onLateUpdateAction
        {
            add { _onLateUpdateAction += value; }
            remove { _onLateUpdateAction -= value; }
        }


        private RouteTransportControl() { }

        private void OnEnable() => _IcreatorCurveRoad = GetComponent<ICreatorCurveRoad>();

        private void LateUpdate() => _onLateUpdateAction?.Invoke();

        ITransportReception[] ITransportInteractRoute.GetPointsReception()
        {
            return _IcreatorCurveRoad.GetPointsConnectionRoute();
        }

#if UNITY_EDITOR
        [SerializeField]
        private TypeProductionResources.TypeResource _typeTransportingResource;

        [SerializeField]
        private byte _indexTypeTransport, _indexTransportInList;


        [Button("Buy Transport"), HorizontalGroup("Hor")]
        private void BuyTransport()
        {
            if (_routePoints[0] == Vector3.zero)
                _routePoints = _IcreatorCurveRoad.GetRoutePoints();

            GameObject newTransport = Instantiate(_allTypesTransport[_indexTypeTransport].prefab,
                                                  _IcreatorCurveRoad.GetRouteMainPoint(),
                                                  Quaternion.identity);
            SelfTransport selfTransportObject = new(_allTypesTransport[_indexTypeTransport], this, newTransport);

            l_purchasedTransport.Add(newTransport);
            l_purchasedTransportController.Add(selfTransportObject);
        }

        [Button("Sell Transport"), HorizontalGroup("Hor")]
        private void SellTransport()
        {
            try
            {
                l_purchasedTransportController[_indexTransportInList].Dispose();
                l_purchasedTransportController.RemoveAt(_indexTransportInList);
                Destroy(l_purchasedTransport[_indexTransportInList]);
                l_purchasedTransport.RemoveAt(_indexTransportInList);
            }
            catch (Exception exception)
            {
                DebugSystem.Log(exception, DebugSystem.SelectedColor.Red, "Exception", "Произошла ошибка: ");
            }
        }

        [Button("Load|Unload States")]
        private void ChangeLoadUnloadStates(in byte indexReception, in byte indexTypeState, in bool isState)
        {
            l_purchasedTransportController[_indexTransportInList].ChangeLoadUnloadStates(indexReception, indexTypeState, isState);
        }

        [Button("Load|Unload States")]
        private void SetCarWaitingMode(in bool isState)
        {
            l_purchasedTransportController[_indexTransportInList].ChangeStateWaiting(isState);
        }

        [Button("Set New Type"), Tooltip("Set new type transporting resource on car")]
        private void UpdateTypeTransportingResource()
        {
            l_purchasedTransportController[_indexTransportInList]
                .SetTypeTransportingResource(_typeTransportingResource);
        }
#endif
    }
}
