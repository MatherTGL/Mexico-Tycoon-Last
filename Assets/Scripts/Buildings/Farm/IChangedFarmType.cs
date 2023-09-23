using Config.Building;


namespace Building.Farm
{
    public interface IChangedFarmType
    {
        void ChangeType(in ConfigBuildingFarmEditor.TypeFarm typeFarm);
    }
}
