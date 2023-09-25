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
        ConfigClimateZoneEditor IClimateZone.configClimateZone { get => _configClimateZone; 
                                                                 set => _configClimateZone = value; }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<BuildingControl>())
                other.GetComponent<BuildingControl>().IclimateZoneControl = this;
        }
    }
}
