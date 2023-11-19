using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using Country.Climate;
using Config.Country.Climate;
using System;

namespace Climate
{
    [RequireComponent(typeof(MeshCollider))]
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone
    {
        private ICountryClimate _IcountryClimate;

        [SerializeField, ReadOnly]
        private ConfigClimateZoneEditor.TypeSeasons _currentSeason;

        private WaitForSeconds _seasonLength;

        public event Action updatedSeason;

        private float _percentageImpactCostMaintenance;


        private ClimateZoneControl() { }

        void IClimateZone.Init(in ICountryClimate IcountryClimate)
        {
            _IcountryClimate = IcountryClimate;
            _seasonLength = new WaitForSeconds(IcountryClimate.configClimate.seasonLength);

            CalculateImpact();
            StartCoroutine(SeasonChanger());
        }

        private void ChangeSeason()
        {
            if (++_currentSeason > ConfigClimateZoneEditor.TypeSeasons.Spring)
                _currentSeason = 0;

            CalculateImpact();
            updatedSeason();
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
                Debug.Log("SeasonChanger");
                ChangeSeason();
            }
        }

        ConfigClimateZoneEditor.TypeSeasons IClimateZone.GetCurrentSeason() => _currentSeason;

        float IClimateZone.GetCurrentSeasonImpact() => _percentageImpactCostMaintenance;
    }
}
