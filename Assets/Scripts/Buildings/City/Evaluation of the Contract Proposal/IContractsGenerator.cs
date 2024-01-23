using Config.Building.Deliveries;

namespace Building.City.Deliveries
{
    public interface IContractsGenerator
    {
        void Init(in ConfigContractsEditor configContracts);
    }
}