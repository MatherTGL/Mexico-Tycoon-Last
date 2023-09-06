using Resources;


namespace Transport.Reception
{
    public interface IBuildingRequestForTransport
    {
        float RequestGetResource(in float transportCapacity, in TypeProductionResources.TypeResource typeResource);
        bool RequestUnloadResource(in float quantityResource, in TypeProductionResources.TypeResource typeResource);
    }
}
