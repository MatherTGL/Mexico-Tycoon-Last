using UnityEngine;
using Route.Builder;
using Boot;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Building;


namespace Transport.Reception
{
    public sealed class TransportReception : MonoBehaviour, ITransportReception, IBoot
    {
        private IBuildingRequestForTransport _buildingRequest;

        private RouteBuilderControl _routeBuilderControl;

        [SerializeField]
        private BuildingControl.TypeBuilding _typeCurrentBuilding;

        [SerializeField]
        private BuildingControl.TypeBuilding[] _typeConnectionBuildings;

        [ShowInInspector, ReadOnly]
        private Dictionary<ITransportReception, GameObject> d_infoRouteConnect
            = new Dictionary<ITransportReception, GameObject>();

        [SerializeField]
        private sbyte _freeConnectionCount;


        private TransportReception() { }

        public void InitAwake()
        {
            _routeBuilderControl = FindObjectOfType<RouteBuilderControl>();
            _buildingRequest = GetComponent<IBuildingRequestForTransport>();
        }

        public (Bootstrap.TypeLoadObject typeLoad, bool isSingle) GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.MediumImportant, false);
        }

        public void ConnectionRequest(in ITransportReception fromObject)
        {
            if (d_infoRouteConnect.Count < _freeConnectionCount)
                if (_typeConnectionBuildings.Contains(fromObject.GetTypeBuilding()))
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

        public (bool confirmRequest, float quantityPerLoad) RequestConnectionToLoadRes(in float transportCapacity)
        {
            Debug.Log("пункт 2");
            if (_buildingRequest.RequestGetResource(transportCapacity).quantity >= transportCapacity)
                return (true, _buildingRequest.RequestGetResource(transportCapacity).quantity);
            else
                return (false, 0);
        }

        public bool RequestConnectionToUnloadRes(in float quantityForUnloading)
        {
            Debug.Log("пункт 2");
            if (_buildingRequest.RequestUnloadResource(quantityForUnloading))
                return true;
            else
                return false;
        }

        public void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject)
        {
            d_infoRouteConnect.Add(fromObject, createdRouteObject);
        }

        private void BuildRoute(in ITransportReception secondObject)
        {
            CreatorCurveRoad createdRoute = Instantiate(_routeBuilderControl.prefabRoute, Vector3.zero, Quaternion.identity);
            createdRoute.SetPositionPoints(secondObject, this);

            AddConnectionToDictionary(secondObject, createdRoute.gameObject);
            secondObject.AddConnectionToDictionary(this, createdRoute.gameObject);
        }

        public Transform GetPosition() { return this.transform; }

        public BuildingControl.TypeBuilding GetTypeBuilding() { return _typeCurrentBuilding; }
    }
}
