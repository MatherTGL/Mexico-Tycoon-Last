using Building.Additional;
using UnityEngine;

namespace Events.Buildings.PoliceRaid
{
    //TODO: https://yougile.com/team/bf00efa6ea26/#chat:181f48980a80
    public sealed class PoliceRaidEvent : MonoBehaviour, IBuildingEvent
    {
        private IProductPackaging _IproductPackaging;

        private ConfigEventPoliceRaid _config;


        void IBuildingEvent.Init(in ScriptableObject config)
        {
            _IproductPackaging = GetComponent<ProductPackagingService>();

            if (config != null || _IproductPackaging != null)
                _config = config as ConfigEventPoliceRaid;
            else
                throw new System.Exception("Config or Component is null");
        }

        void IBuildingEvent.CheckConditionsAreMet()
        {
            if (_IproductPackaging.IsActive())
                Debug.Log("ыыы можно хватать");
        }
    }
}
