using Resources;


namespace Building
{
    public interface IBuilding
    {
        void ConstantUpdatingInfo();

        float GetResources(in float transportCapacity, in TypeProductionResources.TypeResource typeResource);

        bool SetResources(in float quantityResource, in TypeProductionResources.TypeResource typeResource);
    }
}
