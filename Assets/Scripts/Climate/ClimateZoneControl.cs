using UnityEngine;
using Config.Climate;
using Building;
using Sirenix.OdinInspector;
using Boot;
using System.Linq;
using static Boot.Bootstrap;
using System.Collections;

namespace Climate
{
    [RequireComponent(typeof(MeshCollider))]
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone, IBoot
    {
        [SerializeField, Required]
        private ConfigClimateZoneEditor _configClimateZone;
        ConfigClimateZoneEditor IClimateZone.configClimateZone => _configClimateZone;

        [SerializeField, ReadOnly]
        private ConfigClimateZoneEditor.TypeSeasons _currentSeason;

        private WaitForSeconds _seasonLength;


        private ClimateZoneControl() { }

        void IBoot.InitAwake()
        {
            _seasonLength = new WaitForSeconds(_configClimateZone.seasonLength);
            FindObjectsInArea();
            StartCoroutine(SeasonChanger());
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.MediumImportant, TypeSingleOrLotsOf.LotsOf);
        }

        private void FindObjectsInArea()
        {
            var collidersBuildings = Physics.OverlapBox(
                transform.position, GetComponent<MeshCollider>().bounds.size, Quaternion.identity)
                .Where(item => item.GetComponent<BuildingControl>()).ToArray();

            for (ushort i = 0; i < collidersBuildings.Length; i++)
                collidersBuildings[i].GetComponent<BuildingControl>().SetClimateZone(this);
        }

        private void ChangeSeason()
        {
            if (++_currentSeason > ConfigClimateZoneEditor.TypeSeasons.Spring)
                _currentSeason = 0;
        }

        private IEnumerator SeasonChanger()
        {
            while (true)
            {
                yield return _seasonLength;
                ChangeSeason();
            }
        }

        ConfigClimateZoneEditor.TypeSeasons IClimateZone.GetCurrentSeason() => _currentSeason;
    }
}
