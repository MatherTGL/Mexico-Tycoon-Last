using UnityEngine;
using Config.Climate;
using Building;
using Sirenix.OdinInspector;
using Boot;
using System.Linq;
using static Boot.Bootstrap;

namespace Climate
{
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone, IBoot
    {
        [SerializeField, Required]
        private ConfigClimateZoneEditor _configClimateZone;
        ConfigClimateZoneEditor IClimateZone.configClimateZone => _configClimateZone;


        void IBoot.InitAwake() => FindObjectsInArea();

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.MediumImportant, TypeSingleOrLotsOf.LotsOf);
        }

        private void FindObjectsInArea()
        {
            var collidersBuildings = Physics.OverlapBox(
                transform.position, GetComponent<BoxCollider>().size, Quaternion.identity)
                .Where(item => item.GetComponent<BuildingControl>()).ToArray();

            for (ushort i = 0; i < collidersBuildings.Length; i++)
                collidersBuildings[i].GetComponent<BuildingControl>().SetClimateZone(this);
        }
    }
}
