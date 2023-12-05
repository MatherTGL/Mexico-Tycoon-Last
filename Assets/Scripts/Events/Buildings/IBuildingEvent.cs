namespace Events.Buildings
{
    public interface IBuildingEvent
    {
        void CheckConditionsAreMet(in IUsesBuildingsEvents buildingsEvents);
    }
}
