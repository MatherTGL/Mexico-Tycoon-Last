using UnityEngine;
using Sirenix.OdinInspector;
using Country.Inflation;
using Boot;
using System.Collections;
using TimeControl;
using System;
using Climate;
using Country.Climate;
using System.Linq;
using Config.Country.Climate;
using Config.Country.Inflation;
using Country.Climate.Weather;

namespace Country
{
    [RequireComponent(typeof(WeatherControl), typeof(ClimateZoneControl))]
    public sealed class CountryControl : MonoBehaviour, IBoot, ICountryBuildings, ICountryClimate, ICountryInflation
    {
        private IInflation _IInflation;
        IInflation ICountryBuildings.IcountryInflation => _IInflation;

        private IClimateZone _IclimateZone;
        IClimateZone ICountryBuildings.IclimateZone => _IclimateZone;

        private WaitForSeconds _timeTick;

        public event Action timeUpdated;

        [SerializeField, Required]
        private ConfigClimateZoneEditor _configClimateZone;
        ConfigClimateZoneEditor ICountryClimate.configClimate => _configClimateZone;
        ConfigClimateZoneEditor ICountryBuildings.configClimate => _configClimateZone;

        [SerializeField, Required]
        private ConfigInflationEditor _configInflation;
        ConfigInflationEditor ICountryInflation.configInflation => _configInflation;


        private CountryControl() { }

        void IBoot.InitAwake()
        {
            _IclimateZone = GetComponent<ClimateZoneControl>();
            _IclimateZone.Init(this);

            _IInflation = new CountryInflation();
            _IInflation.Init(this);

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
            var collidersInteractObjects = Physics.OverlapBox(
                transform.position, GetComponent<MeshCollider>().bounds.size, Quaternion.identity)
                .Where(item => item.GetComponent(typeof(ICountryAreaFindSceneObjects))).ToArray();

            for (ushort i = 0; i < collidersInteractObjects.Length; i++)
                collidersInteractObjects[i].GetComponent<ICountryAreaFindSceneObjects>().SetCountry(this);
        }
    }
}
