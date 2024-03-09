using System;
using Building.Additional;
using Transport.Reception;
using UnityEngine;

namespace Transport
{
    public interface ITransportInteractRoute
    {
        event Action fixedUpdate;

        event Action updatedTimeStep;

        ITransportReception[] GetPointsReception();

        Vector3[] routePoints { get; }

        float impactOfObstaclesOnSpeed { get; }
    }
}
