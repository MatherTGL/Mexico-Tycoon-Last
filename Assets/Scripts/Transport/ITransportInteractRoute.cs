using System;
using Transport.Reception;
using UnityEngine;


namespace Transport
{
    public interface ITransportInteractRoute
    {
        event Action onLateUpdateAction;
        ITransportReception[] GetPointsReception();
        Vector3[] routePoints { get; }
    }
}
