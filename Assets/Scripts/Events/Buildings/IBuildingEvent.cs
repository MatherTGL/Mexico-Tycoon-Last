using UnityEngine;

namespace Events.Buildings
{
    public interface IUserEvent
    {
        void Init(in ScriptableObject config);

        void CheckConditionsAreMet();
    }
}
