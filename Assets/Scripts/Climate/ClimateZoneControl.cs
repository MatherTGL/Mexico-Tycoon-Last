using UnityEngine;
using Config.Climate;
using Building;
using Sirenix.OdinInspector;

namespace Climate
{
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone
    {
        [SerializeField, Required]
        private ConfigClimateZoneEditor _configClimateZone;
        ConfigClimateZoneEditor IClimateZone.configClimateZone => _configClimateZone;


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BuildingControl>())
            {
                Debug.Log($"{this}");
                other.GetComponent<BuildingControl>().IclimateZoneControl = this;
                Debug.Log(other.GetComponent<BuildingControl>().IclimateZoneControl);
            }
        }
    }
}
