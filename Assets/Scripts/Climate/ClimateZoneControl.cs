using UnityEngine;
using Config.Climate;
using Building;
using Sirenix.OdinInspector;
using Boot;
using System.Linq;

namespace Climate
{
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone, IBoot
    {
        [SerializeField, Required]
        private ConfigClimateZoneEditor _configClimateZone;
        ConfigClimateZoneEditor IClimateZone.configClimateZone => _configClimateZone;


        void IBoot.InitAwake() => TriggerEnterObjects();

        (Bootstrap.TypeLoadObject typeLoad, bool isSingle) IBoot.GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SimpleImportant, false);
        }

        private void TriggerEnterObjects()
        {
            var collidersBuildings = Physics.OverlapBox(
                transform.position, GetComponent<BoxCollider>().size, Quaternion.identity)
                .Where(item => item.GetComponent<BuildingControl>()).ToArray();

            for (int i = 0; i < collidersBuildings.Length; i++)
                collidersBuildings[i].GetComponent<BuildingControl>().SetClimateZone(this);
        }
    }
}
