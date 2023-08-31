using Building;
using UnityEngine;


namespace Transport.Reception
{
    public interface ITransportReception
    {
        void ConnectionRequest(in ITransportReception fromObject);
        bool ConfirmRequest(in ITransportReception fromObject);

        void DisconnectRequest(in ITransportReception fromObject);
        bool ConfirmDisconnectRequest(in ITransportReception fromObject);

        (bool confirmRequest, float quantityPerLoad) RequestConnectionToLoadRes(in float transportCapacity);
        bool RequestConnectionToUnloadRes(in float quantityForUnloading);

        Transform GetPosition();
        BuildingControl.TypeBuilding GetTypeBuilding();

        void AddConnectionToDictionary(in ITransportReception fromObject, in GameObject createdRouteObject);
    }
}
