using UnityEngine;
using System.Collections;
using System.Linq;

namespace Country.Climate.Weather
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class WeatherZoneControl : MonoBehaviour, IWeatherZone
    {
        private WaitForSecondsRealtime _weatherLifetime;

        private Collider[] _collidersInteractObjects;

        private float _scaleWeatherZone;

        private float _lifetimeWeatherZone;

        private float _impactWeatherZone;
        float IWeatherZone.impactWeatherZone => _impactWeatherZone;


        void IWeatherZone.Init(in ICountryClimate countryClimate)
        {
            int randomIndexWeather = UnityEngine.Random.Range(0, countryClimate.configClimate.availableWeatherInCountry.Length);
            var config = countryClimate.configClimate.availableWeatherInCountry[randomIndexWeather];

            _scaleWeatherZone = UnityEngine.Random.Range(config.minZoneScale, config.maxZoneScale);
            _lifetimeWeatherZone = UnityEngine.Random.Range(config.minLifetime, config.maxLifetime);
            _impactWeatherZone = UnityEngine.Random.Range(config.minPercentageImpact, config.maxPercentageImpact);

            GetComponent<BoxCollider>().size = new Vector3(_scaleWeatherZone, _scaleWeatherZone, _scaleWeatherZone);
            _weatherLifetime = new WaitForSecondsRealtime(_lifetimeWeatherZone);

            gameObject.SetActive(true);
            StartCoroutine(WeatherLifetime());
            return;
        }

        private IEnumerator WeatherLifetime()
        {
            FindObjectsInArea();
            ActivateWeatherEvent();

            yield return _weatherLifetime;

            DeactivateWeatherEvent();
            gameObject.SetActive(false); //? move to pool or destroy
        }

        private void FindObjectsInArea()
        {
            _collidersInteractObjects = Physics.OverlapBox(
                transform.position, GetComponent<BoxCollider>().bounds.size)
                .Where(item => item.GetComponent(typeof(ICountryAreaFindSceneObjects))).ToArray();
        }

        private void ActivateWeatherEvent()
        {
            for (ushort i = 0; i < _collidersInteractObjects.Length; i++)
                _collidersInteractObjects[i].GetComponent<ICountryAreaFindSceneObjects>().ActivateWeatherEvent(this);
        }

        private void DeactivateWeatherEvent()
        {
            for (ushort i = 0; i < _collidersInteractObjects.Length; i++)
                _collidersInteractObjects[i].GetComponent<ICountryAreaFindSceneObjects>().DeactiveWeatherEvent(this);
        }
    }
}
