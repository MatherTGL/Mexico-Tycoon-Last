using UnityEngine;
using Config.Climate;
using Building;
using Sirenix.OdinInspector;
using Boot;
using System.Linq;
using static Boot.Bootstrap;
using System.Collections;
using Country.Climate;

namespace Climate
{
    [RequireComponent(typeof(MeshCollider))]
    public sealed class ClimateZoneControl : MonoBehaviour, IClimateZone
    {
        private ICountryClimate _IcountryClimate;

        [SerializeField, ReadOnly]
        private ConfigClimateZoneEditor.TypeSeasons _currentSeason;

        private WaitForSeconds _seasonLength;


        private ClimateZoneControl() { }

        void IClimateZone.Init(in ICountryClimate IcountryClimate)
        {
            _IcountryClimate = IcountryClimate;
            _seasonLength = new WaitForSeconds(IcountryClimate.configClimate.seasonLength);
            StartCoroutine(SeasonChanger());
        }

        private void ChangeSeason()
        {
            if (++_currentSeason > ConfigClimateZoneEditor.TypeSeasons.Spring)
                _currentSeason = 0;
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
    }
}
