namespace Building
{
    public interface IBuilding
    {
        void ConstantUpdatingInfo();
        void ChangeJobStatus(in bool isState);
        (bool confirm, float quantityAmount) GetResources(in float transportCapacity);
        bool SetResources(in float quantityResource);
    }
}
