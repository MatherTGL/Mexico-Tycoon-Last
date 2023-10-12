using Building.Additional;
using Resources;

namespace Transport.Reception
{
    public interface IBuildingRequestForTransport
    {
        IBuildingPurchased IbuildingPurchased { get; }
        IBuildingJobStatus IbuildingJobStatus { get; }


        float RequestGetResource(in float transportCapacity, in TypeProductionResources.TypeResource typeResource);
        bool RequestUnloadResource(in float quantityResource, in TypeProductionResources.TypeResource typeResource);
    }
}
