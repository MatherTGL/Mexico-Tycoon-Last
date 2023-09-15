using System.Collections.Generic;
using Resources;


namespace Building
{
    public interface IBuilding
    {
        Dictionary<TypeProductionResources.TypeResource, float> d_amountResources { get; set; }


        void ConstantUpdatingInfo();

        float GetResources(in float transportCapacity, in TypeProductionResources.TypeResource typeResource);

        bool SetResources(in float quantityResource, in TypeProductionResources.TypeResource typeResource);
    }
}
