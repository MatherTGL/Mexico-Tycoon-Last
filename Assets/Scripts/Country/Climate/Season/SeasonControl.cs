using System;
using System.Collections;
using Config.Country.Climate;
using Country.Climate;
using DebugCustomSystem;
using GameSystem;
using UnityEngine;

namespace Climate
{
    public sealed class SeasonControl : ISeasonControl
    {
        private static ConfigClimateZoneEditor.TypeSeasons _currentSeason;

        ConfigClimateZoneEditor.TypeSeasons ISeasonControl.currentSeason => _currentSeason;

        private WaitForSeconds _seasonLength;

        public event Action<float> updatedSeason;

        private float _percentageImpactCostMaintenance;

        private readonly ICountryClimate _IcountryClimate;


        public SeasonControl(ICountryClimate climateZone)
            => this._IcountryClimate = climateZone;

        void ISeasonControl.Init()
        {
            CalculateImpact();
            CoroutineManager.Instance.StartManagedCoroutine(SeasonChanger());
        }

        private IEnumerator SeasonChanger()
        {
            _seasonLength = new WaitForSeconds(_IcountryClimate.configClimate.seasonLength);

            //TODO добавить isPaused
            while (true)
            {
                yield return _seasonLength;
                ChangeSeason();
            }
        }

        private void ChangeSeason()
        {
            if (++_currentSeason > ConfigClimateZoneEditor.TypeSeasons.Spring)
                _currentSeason = 0;

            CalculateImpact();
            updatedSeason.Invoke((float)_currentSeason);
        }

        private void CalculateImpact()
        {
            _percentageImpactCostMaintenance
                = _IcountryClimate.configClimate.seasonsImpactExpenses.Get(_currentSeason);
        }

        float ISeasonControl.GetCurrentSeasonImpact() => _percentageImpactCostMaintenance;
    }
}
