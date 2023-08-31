using UnityEngine;
using Transport.Reception;


namespace Route.Builder
{
    public interface ICreatorCurveRoad
    {
        Vector3[] GetRoutePoints();
        Vector3 GetRouteMainPoint();
        ITransportReception[] GetPointsConnectionRoute();
    }
}
