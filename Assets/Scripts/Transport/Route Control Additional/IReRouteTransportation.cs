namespace Transport
{
    public interface IReRouteTransportation
    {
        ushort[] SendTransportTransferRequest(in TransportationDataStorage allTransportation);
    }
}
