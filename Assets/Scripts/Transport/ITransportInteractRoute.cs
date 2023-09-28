using System;
using Transport.Reception;
using UnityEngine;


namespace Transport
{
    public interface ITransportInteractRoute
    {
        event Action lateUpdated;

        event Action updatedTimeStep;

        ITransportReception[] GetPointsReception();

        Vector3[] routePoints { get; }

        float impactOfObstaclesOnSpeed { get; }
    }
}
