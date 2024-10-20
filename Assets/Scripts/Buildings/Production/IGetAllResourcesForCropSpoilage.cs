using System.Collections.Generic;
using static Resources.TypeProductionResources;

namespace Building.Additional.Crop
{
    public interface IGetAllResourcesForCropSpoilage
    {
        Dictionary<TypeResource, double> amountResources { get; set; }
    }
}
