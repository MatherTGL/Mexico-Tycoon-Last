using UnityEngine;
using Route.Builder;
using Boot;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Building;
using Resources;
using static Boot.Bootstrap;

namespace Transport.Reception
{
    public sealed class TransportReception : MonoBehaviour, ITransportReception, IBoot
    {
        private IBuildingRequestForTransport _buildingRequest;

        private RouteBuilderControl _routeBuilderControl;

        [SerializeField]
        private BuildingControl.TypeBuilding _typeCurrentBuilding;
        BuildingControl.TypeBuilding ITransportReception.typeCurrentBuilding => _typeCurrentBuilding;

        [SerializeField]
        private BuildingControl.TypeBuilding[] _typeConnectBuildings;

        [ShowInInspector, ReadOnly]
        private Dictionary<ITransportReception, GameObject> d_infoRouteConnect = new();

        [SerializeField]
        private byte _freeConnectionCount;


        private TransportReception() { }

        void IBoot.InitAwake()
        {
            _routeBuilderControl = FindObjectOfType<RouteBuilderControl>();
            _buildingRequest = GetComponent<IBuildingRequestForTransport>();
        }

        private void BuildRoute(in ITransportReception secondObject)
        {
            CreatorCurveRoadControl createdRoute = Instantiate(_routeBuilderControl.prefabRoute,
                Vector3.zero, Quaternion.identity);

            createdRoute.SetPositionPoints(secondObject, this);
            AddConnectionToDictionary(secondObject, createdRoute.gameObject);
            secondObject.AddConnectionToDictionary(this, createdRoute.gameObject);
        }

        float ITransportReception.RequestConnectionToLoadRes(in float transportCapacity,
                                                             in TypeProductionResources.TypeResource typeResource)
        {
            return _buildingRequest.RequestGetResource(transportCapacity, typeResource);
        }

        bool ITransportReception.RequestConnectionToUnloadRes(in float quantityForUnloading,
                                                              in TypeProductionResources.TypeResource typeResource)
        {
            return _buildingRequest.RequestUnloadResource(quantityForUnloading, typeResource);
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.MediumImportant, TypeSingleOrLotsOf.LotsOf);
        }

        public void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject)
        {
            d_infoRouteConnect.Add(fromObject, createdRouteObject);
        }

        public void ConnectionRequest(in ITransportReception fromObject)
        {
            if (d_infoRouteConnect.Count <= _freeConnectionCount)
                if (_typeConnectBuildings.Contains(fromObject.GetTypeBuilding()))
                    if (fromObject.ConfirmRequest(this) && ConfirmRequest(fromObject))
                        BuildRoute(fromObject);
        }

        public bool ConfirmRequest(in ITransportReception fromObject)
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
            if (d_infoRouteConnect.ContainsKey(fromObject) && fromObject.ConfirmDisconnectRequest(this))
            {
                d_infoRouteConnect.Remove(fromObject);
                _freeConnectionCount++;
            }
        }

        public bool ConfirmDisconnectRequest(in ITransportReception fromObject)
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

        public Transform GetPosition() { return this.transform; }

        public BuildingControl.TypeBuilding GetTypeBuilding() { return _typeCurrentBuilding; }
    }
}
