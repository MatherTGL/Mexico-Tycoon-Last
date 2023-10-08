using Resources;
using UnityEngine;
using static Building.BuildingEnumType;

namespace Transport.Reception
{
    public interface ITransportReception
    {
        TypeBuilding typeCurrentBuilding { get; }

        void ConnectionRequest(in ITransportReception fromObject);
        bool ConfirmRequest(in ITransportReception fromObject);

        void DisconnectRequest(in ITransportReception fromObject);
        bool ConfirmDisconnectRequest(in ITransportReception fromObject);

        float RequestConnectionToLoadRes(in float transportCapacity,
            in TypeProductionResources.TypeResource typeResource);
        bool RequestConnectionToUnloadRes(in float quantityForUnloading,
            in TypeProductionResources.TypeResource typeResource);

        Transform GetPosition();
        TypeBuilding GetTypeBuilding();

        void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject);
    }
}
