using UnityEngine;
using Route.Builder;
using Boot;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using static Boot.Bootstrap;
using static Building.BuildingEnumType;
using Config.Transport.Reception;
using static Resources.TypeProductionResources;

namespace Transport.Reception
{
    public sealed class TransportReception : MonoBehaviour, ITransportReception, IBoot
    {
        private IBuildingRequestForTransport _IbuildingRequest;
        IBuildingRequestForTransport ITransportReception.IbuildingRequest => _IbuildingRequest;

        [SerializeField, Required, HideLabel]
        private ConfigTransportReceptionEditor _configReception;

        private RouteBuilderControl _routeBuilderControl;

        TypeBuilding ITransportReception.typeCurrentBuilding => _configReception.typeCurrentBuilding;

        private readonly Dictionary<ITransportReception, GameObject> d_infoRouteConnect = new();

        private byte _freeConnectionCount;


        private TransportReception() { }

        [System.Obsolete]
        void IBoot.InitAwake()
        {
            _routeBuilderControl = FindObjectOfType<RouteBuilderControl>();
            _IbuildingRequest = GetComponent<IBuildingRequestForTransport>();
            _freeConnectionCount = _configReception.defaultConnectionCount;
        }

        void IBoot.InitStart() { }

        private void BuildRoute(in ITransportReception secondObject, in TypeTransport.Type typeRoute)
        {
            if (IsBuildingsWerePurchased(secondObject))
            {
                CreatorCurveRoadControl createdRoute = Instantiate(_routeBuilderControl.config.prefabRoute,
                Vector3.zero, Quaternion.identity);

                createdRoute.Generate(secondObject, this, typeRoute);
                AddConnectionToDictionary(secondObject, createdRoute.gameObject);
                secondObject.AddConnectionToDictionary(this, createdRoute.gameObject);
            }
        }

        private bool IsBuildingsWerePurchased(in ITransportReception secondObject)
        {
            var firstBuildingBuyed = _IbuildingRequest.IbuildingPurchased?.isBuyed;
            var secondBuildingBuyed = secondObject.IbuildingRequest.IbuildingPurchased?.isBuyed;

            if (firstBuildingBuyed is null or true & secondBuildingBuyed is null or true)
                return true;
            else
                return false;
        }

        float ITransportReception.GetRequestConnectionToLoadRes(in float capacity, in TypeResource typeResource)
            => _IbuildingRequest.RequestGetResource(capacity, typeResource);

        bool ITransportReception.IsRequestConnectionToUnloadRes(in float quantityForUnloading, in TypeResource typeResource)
            => _IbuildingRequest.RequestUnloadResource(quantityForUnloading, typeResource);

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.MediumImportant, TypeSingleOrLotsOf.LotsOf);

        public void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject)
            => d_infoRouteConnect.Add(fromObject, createdRouteObject);

        public void ConnectionRequest(in ITransportReception fromObject, in TypeTransport.Type typeRoute)
        {
            if (d_infoRouteConnect.Count <= _freeConnectionCount)
                if (_configReception.typeConnectBuildings.Contains(fromObject.GetTypeBuilding()))
                    if (fromObject.IsConfirmRequest(this) && IsConfirmRequest(fromObject))
                        BuildRoute(fromObject, typeRoute);
        }

        public bool IsConfirmRequest(in ITransportReception fromObject)
        {
            if (!d_infoRouteConnect.ContainsKey(fromObject) || !d_infoRouteConnect.ContainsKey(this))
            {
                _freeConnectionCount--;
                return true;
            }
            else return false;
        }

        public void DisconnectRequest(in ITransportReception fromObject)
        {
            if (d_infoRouteConnect.ContainsKey(fromObject) && fromObject.IsConfirmDisconnectRequest(this))
            {
                d_infoRouteConnect.Remove(fromObject);
                _freeConnectionCount++;
            }
        }

        public bool IsConfirmDisconnectRequest(in ITransportReception fromObject)
        {
            if (d_infoRouteConnect.ContainsKey(fromObject))
            {
                Destroy(d_infoRouteConnect[fromObject]);
                d_infoRouteConnect.Remove(fromObject);
                _freeConnectionCount++;
                return true;
            }
            else return false;
        }

        public Transform GetTransform() => transform;

        public TypeBuilding GetTypeBuilding() => _configReception.typeCurrentBuilding;
    }
}
