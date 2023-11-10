using UnityEngine;
using Sirenix.OdinInspector;
using Country.Inflation;
using Boot;
using System.Collections;
using TimeControl;
using System;
using Climate;
using Config.Climate;
using Country.Climate;
using System.Linq;
using Building;

namespace Country
{
    public sealed class CountryControl : MonoBehaviour, IBoot, ICountryBuildings, ICountryClimate
    {
        private ICountryInflation _IcountryInflation;
        ICountryInflation ICountryBuildings.IcountryInflation => _IcountryInflation;

        private IClimateZone _IclimateZone;
        IClimateZone ICountryBuildings.IclimateZone => _IclimateZone;

        private WaitForSeconds _timeTick;

        public event Action timeUpdated;

        [SerializeField, Required]
        private ConfigClimateZoneEditor _configClimateZone;
        ConfigClimateZoneEditor ICountryClimate.configClimate => _configClimateZone;
        ConfigClimateZoneEditor ICountryBuildings.configClimate => _configClimateZone;


        void IBoot.InitAwake()
        {
            _IcountryInflation = new CountryInflation();
            _IcountryInflation.Init(this);

            float time = FindObjectOfType<TimeDateControl>().GetCurrentTimeOneDay();
            _timeTick = new WaitForSeconds(time);
            FindObjectsInArea();
        }

        void IBoot.InitStart() => StartCoroutine(UpdateTick());

        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.MediumImportant, Bootstrap.TypeSingleOrLotsOf.LotsOf);
        }

        private IEnumerator UpdateTick()
        {
            while (true)
            {
                timeUpdated();
                yield return _timeTick;
            }
        }

        private void FindObjectsInArea()
        {
            var collidersBuildings = Physics.OverlapBox(
                transform.position, GetComponent<MeshCollider>().bounds.size, Quaternion.identity)
                .Where(item => item.GetComponent<BuildingControl>()).ToArray();

            for (ushort i = 0; i < collidersBuildings.Length; i++)
                collidersBuildings[i].GetComponent<BuildingControl>().SetCountry(this);
        }
    }
}
