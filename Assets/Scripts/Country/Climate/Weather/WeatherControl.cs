using UnityEngine;
using Sirenix.OdinInspector;
using Climate;
using System.Collections.Generic;

namespace Country.Climate.Weather
{
    [RequireComponent(typeof(ClimateZoneControl))]
    public sealed class WeatherControl : MonoBehaviour, IWeatherControl
    {
        private ICountryClimate _IcountryClimate;

        [SerializeField, Required]
        private WeatherZoneControl _weatherZonePrefab;

        [SerializeField, ReadOnly]
        private List<IWeatherZone> l_activeWeatherZones = new();


        void IWeatherControl.Init(in ICountryClimate IcountryClimate)
        {
            _IcountryClimate = IcountryClimate;
        }

        [Button("Invoke Random Weather")]
        private void InvokeRandomWeather()
        {
            l_activeWeatherZones.Add(CreateWeatherZoneAndGet());
            l_activeWeatherZones[l_activeWeatherZones.Count - 1].Init(_IcountryClimate);
        }

        private IWeatherZone CreateWeatherZoneAndGet()
        {
            var weatherZone = Instantiate(_weatherZonePrefab, transform.position, Quaternion.identity, transform);
            return weatherZone.GetComponent<IWeatherZone>();
        }
    }
}
