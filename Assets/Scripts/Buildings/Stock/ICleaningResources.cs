using Resources;

namespace Building.Farm
{
    public interface ICleaningResources
    {
        void Clear(in TypeProductionResources.TypeResource typeResource, in double amount);
    }
}
