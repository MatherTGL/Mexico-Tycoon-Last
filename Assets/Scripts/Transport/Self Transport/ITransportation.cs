using Transport.Breakdowns;
using Transport.Fuel;

namespace Transport
{
    public interface ITransportation
    {
        ITransportInteractRoute ItransportInteractRoute { get; }

        TransportationFuel transportationFuel { get; }

        TransportationBreakdowns transportationBreakdowns { get; }

        TypeTransport typeTransport { get; }
    }
}
