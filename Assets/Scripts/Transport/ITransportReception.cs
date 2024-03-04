using UnityEngine;
using static Building.BuildingEnumType;
using static Resources.TypeProductionResources;

namespace Transport.Reception
{
    public interface ITransportReception
    {
        IBuildingRequestForTransport IbuildingRequest { get; }

        TypeBuilding typeCurrentBuilding { get; }


        void ConnectionRequest(in ITransportReception fromObject, in TypeTransport.Type routeType);

        bool IsConfirmRequest(in ITransportReception fromObject);

        void DisconnectRequest(in ITransportReception fromObject);

        bool IsConfirmDisconnectRequest(in ITransportReception fromObject);

        float GetRequestConnectionToLoadRes(in float transportCapacity, in TypeResource typeResource);

        bool IsRequestConnectionToUnloadRes(in float quantityForUnloading, in TypeResource typeResource);

        Transform GetTransform();

        TypeBuilding GetTypeBuilding();

        void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject);
    }
}
