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

        public static ISeasonControl IseasonControl { get; private set; }

        ISeasonControl IClimateZone.seasonControl => IseasonControl;


        private ClimateZoneControl() { }

        void IClimateZone.Init(in ICountryClimate IcountryClimate)
        {
            _IcountryClimate = IcountryClimate;

            IseasonControl = new SeasonControl(_IcountryClimate);
            IseasonControl.Init();

            var weatherClimate = GetComponent<IWeatherControl>();
            weatherClimate.Init(_IcountryClimate);
        }
    }
}
