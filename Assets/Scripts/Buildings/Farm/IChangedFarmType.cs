using System.Threading.Tasks;
using static Config.Building.ConfigBuildingFarmEditor;

namespace Building.Farm
{
    public interface IChangedFarmType
    {
        ValueTask ChangeType(TypeFarm typeFarm);
    }
}
