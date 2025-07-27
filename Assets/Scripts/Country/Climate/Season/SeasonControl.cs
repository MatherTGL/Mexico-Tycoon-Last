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

        ConfigClimateZoneEditor.TypeSeasons ISeasonControl._currentSeason => _currentSeason;

        private WaitForSeconds _seasonLength;

        public event Action<float> updatedSeason;

        private float _percentageImpactCostMaintenance;

        private readonly ICountryClimate _IcountryClimate;


        public SeasonControl(ICountryClimate climateZone)
            => this._IcountryClimate = climateZone;

        void ISeasonControl.Init()
        {
            CoroutineManager.Instance.StartManagedCoroutine(SeasonChanger());
            CalculateImpact();
            _seasonLength = new WaitForSeconds(_IcountryClimate.configClimate.seasonLength);

            DebugSystem.Log($"Season in country: {_currentSeason}", DebugSystem.SelectedColor.Orange, tag: "Country");
        }

        private IEnumerator SeasonChanger()
        {
            //TODO добавить isPaused
            while (true)
            {
                Debug.Log("SeasonChanger coroutine is run");
                yield return _seasonLength;
                ChangeSeason();
            }
        }

        private void ChangeSeason()
        {
            if (++_currentSeason > ConfigClimateZoneEditor.TypeSeasons.Spring)
                _currentSeason = 0;

            CalculateImpact();
            updatedSeason.Invoke(0);
            DebugSystem.Log($"Season in country: {_currentSeason}", DebugSystem.SelectedColor.Orange, tag: "Country");
        }

        private void CalculateImpact()
        {
            _percentageImpactCostMaintenance
                = _IcountryClimate.configClimate.seasonsImpactExpenses.Get(_currentSeason);
        }

        float ISeasonControl.GetCurrentSeasonImpact() => _percentageImpactCostMaintenance;
    }
}
