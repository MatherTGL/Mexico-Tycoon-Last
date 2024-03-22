using Config.Building;

namespace Building.Farm
{
    public interface IChangedFarmType
    {
        void ChangeType(ConfigBuildingFarmEditor.TypeFarm typeFarm);
    }
}
