using UnityEngine;
using Country.Climate;
using System;
using Country.Climate.Weather;

namespace Climate
{
    [RequireComponent(typeof(MeshCollider), typeof(WeatherControl))]
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone
    {
        private ICountryClimate _IcountryClimate;

        private static ISeasonControl _IseasonControl;

        ISeasonControl IClimateZone.seasonControl => _IseasonControl;


        private ClimateZoneControl() { }

        void IClimateZone.Init(in ICountryClimate IcountryClimate)
        {
            _IcountryClimate = IcountryClimate;

            _IseasonControl = new SeasonControl(_IcountryClimate);
            _IseasonControl.Init();

            var weatherClimate = GetComponent<IWeatherControl>();
            weatherClimate.Init(_IcountryClimate);
        }
    }
}
