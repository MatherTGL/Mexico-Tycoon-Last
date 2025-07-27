using System;
using Config.Country.Climate;
using UnityEngine;

namespace Climate
{
    public interface ISeasonControl
    {
        ConfigClimateZoneEditor.TypeSeasons _currentSeason { get; }

        event Action<float> updatedSeason;


        void Init();

        float GetCurrentSeasonImpact();
    }
}
