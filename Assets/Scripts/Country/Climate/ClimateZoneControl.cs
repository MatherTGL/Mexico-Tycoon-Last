using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Country.Climate;
using Config.Country.Climate;
using System;
using Country.Climate.Weather;

namespace Climate
{
    [RequireComponent(typeof(MeshCollider), typeof(WeatherControl))]
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone
    {
        private ICountryClimate _IcountryClimate;

        [SerializeField, ReadOnly]
        private ConfigClimateZoneEditor.TypeSeasons _currentSeason;

        private WaitForSeconds _seasonLength;

        public event Action<float> updatedSeason;

        private float _percentageImpactCostMaintenance;


        private ClimateZoneControl() { }

        void IClimateZone.Init(in ICountryClimate IcountryClimate)
        {
            _IcountryClimate = IcountryClimate;
            _seasonLength = new WaitForSeconds(IcountryClimate.configClimate.seasonLength);

            CalculateImpact();
            StartCoroutine(SeasonChanger());

            var weatherClimate = GetComponent<IWeatherControl>();
            weatherClimate.Init(_IcountryClimate);
        }

        private void ChangeSeason()
        {
            if (++_currentSeason > ConfigClimateZoneEditor.TypeSeasons.Spring)
                _currentSeason = 0;

            CalculateImpact();
            updatedSeason.Invoke(0);
        }

        private void CalculateImpact()
        {
            _percentageImpactCostMaintenance = _IcountryClimate.configClimate.seasonsImpactExpenses.Get(_currentSeason);
            Debug.Log(_percentageImpactCostMaintenance);
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

        float IClimateZone.GetCurrentSeasonImpact() => _percentageImpactCostMaintenance;
    }
}
