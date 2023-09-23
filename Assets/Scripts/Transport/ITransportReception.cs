using Building;
using Resources;
using UnityEngine;


namespace Transport.Reception
{
    public interface ITransportReception
    {
        BuildingControl.TypeBuilding typeCurrentBuilding { get; }

        void ConnectionRequest(in ITransportReception fromObject);
        bool ConfirmRequest(in ITransportReception fromObject);

        void DisconnectRequest(in ITransportReception fromObject);
        bool ConfirmDisconnectRequest(in ITransportReception fromObject);

        float RequestConnectionToLoadRes(in float transportCapacity,
            in TypeProductionResources.TypeResource typeResource);
        bool RequestConnectionToUnloadRes(in float quantityForUnloading,
            in TypeProductionResources.TypeResource typeResource);

        Transform GetPosition();
        BuildingControl.TypeBuilding GetTypeBuilding();

        void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject);
    }
}
