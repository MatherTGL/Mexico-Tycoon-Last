using UnityEngine;

namespace Events.Buildings
{
    public interface IBuildingEvent
    {
        void Init(in ScriptableObject config);

        void CheckConditionsAreMet();
    }
}
