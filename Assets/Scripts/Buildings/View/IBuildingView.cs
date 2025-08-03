namespace Building.View
{
    public interface IBuildingView
    {
        void FirstLoad(IGetBuildingViewFunctions IBuildingViewFunctions);

        void Reload();

        void End();
    }
}
