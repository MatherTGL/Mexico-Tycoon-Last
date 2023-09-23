using UnityEngine;
using Transport.Reception;
using Transport;


namespace Route.Builder
{
    public interface ICreatorCurveRoad
    {
        TypeTransport.Type typeRoute { get; }


        Vector3[] GetRoutePoints();

        Vector3 GetRouteMainPoint();

        ITransportReception[] GetPointsConnectionRoute();
    }
}
