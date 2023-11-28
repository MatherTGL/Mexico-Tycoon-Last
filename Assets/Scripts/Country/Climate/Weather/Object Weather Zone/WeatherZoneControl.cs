using UnityEngine;
using static Config.Country.Climate.ConfigClimateZoneEditor;
using System.Collections;
using System.Linq;

namespace Country.Climate.Weather
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class WeatherZoneControl : MonoBehaviour, IWeatherZone
    {
        private WaitForSecondsRealtime _weatherLifetime;


        void IWeatherZone.Init(in ICountryClimate countryClimate)
        {
            float scaleWeatherZone = 0;
            float lifetimeWeatherZone = 0;

            for (int i = 0; i < countryClimate.configClimate.availableWeatherInCountry.Length; i++)
            {
                if (countryClimate.configClimate.availableWeatherInCountry[i].weatherType is WeatherType.Rain)
                {
                    var config = countryClimate.configClimate.availableWeatherInCountry[i];

                    scaleWeatherZone = Random.Range(config.minZoneScale, config.maxZoneScale);
                    lifetimeWeatherZone = Random.Range(config.minLifetime, config.maxLifetime);

                    GetComponent<BoxCollider>().size = new Vector3(scaleWeatherZone, scaleWeatherZone, scaleWeatherZone);
                    _weatherLifetime = new WaitForSecondsRealtime(lifetimeWeatherZone);

                    gameObject.SetActive(true);
                    StartCoroutine(WeatherLifetime());
                    return;
                }
            }
        }

        private IEnumerator WeatherLifetime()
        {
            FindObjectsInArea();

            yield return _weatherLifetime;
            gameObject.SetActive(false);
        }

        private void FindObjectsInArea()
        {
            //? separate to array

            var collidersInteractObjects = Physics.OverlapBox(
                transform.position, GetComponent<BoxCollider>().bounds.size)
                .Where(item => item.GetComponent(typeof(ICountryAreaFindSceneObjects))).ToArray();

            for (ushort i = 0; i < collidersInteractObjects.Length; i++)
                collidersInteractObjects[i].GetComponent<ICountryAreaFindSceneObjects>().ActivateWeatherEvent(this);
        }
    }
}
