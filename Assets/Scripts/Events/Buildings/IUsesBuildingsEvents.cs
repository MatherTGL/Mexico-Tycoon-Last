using System.Collections.Generic;
using Config.Building.Events;
using Resources;

namespace Events.Buildings
{
    public interface IUsesBuildingsEvents
    {
        ConfigBuildingsEventsEditor configBuildingsEvents { get; }

        Dictionary<TypeProductionResources.TypeResource, double> amountResources { get; set; }
    }
}
