using Building.Additional;
using Resources;
using static Resources.TypeProductionResources;

namespace Transport.Reception
{
    public interface IBuildingRequestForTransport
    {
        IBuildingPurchased IbuildingPurchased { get; }
        IBuildingJobStatus IbuildingJobStatus { get; }


        float RequestGetResource(in float transportCapacity, in TypeResource typeResource);

        bool RequestUnloadResource(in float quantityResource, in TypeResource typeResource);
    }
}
