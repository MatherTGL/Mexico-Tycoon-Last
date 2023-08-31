namespace Transport.Reception
{
    public interface IBuildingRequestForTransport
    {
        (bool inStock, float quantity) RequestGetResource(in float transportCapacity);
        bool RequestUnloadResource(in float quantityResource);
    }
}
