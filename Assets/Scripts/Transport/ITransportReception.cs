using Resources;
using UnityEngine;
using static Building.BuildingEnumType;

namespace Transport.Reception
{
    public interface ITransportReception
    {
        IBuildingRequestForTransport IbuildingRequest { get; }

        TypeBuilding typeCurrentBuilding { get; }


        void ConnectionRequest(in ITransportReception fromObject);

        bool IsConfirmRequest(in ITransportReception fromObject);

        void DisconnectRequest(in ITransportReception fromObject);

        bool IsConfirmDisconnectRequest(in ITransportReception fromObject);

        float GetRequestConnectionToLoadRes(in float transportCapacity,
            in TypeProductionResources.TypeResource typeResource);

        bool IsRequestConnectionToUnloadRes(in float quantityForUnloading,
            in TypeProductionResources.TypeResource typeResource);

        Transform GetPosition();

        TypeBuilding GetTypeBuilding();

        void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject);
    }
}
