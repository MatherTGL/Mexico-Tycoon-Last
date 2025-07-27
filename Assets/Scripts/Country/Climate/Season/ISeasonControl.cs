using System;
using Config.Country.Climate;

namespace Climate
{
    public interface ISeasonControl
    {
        ConfigClimateZoneEditor.TypeSeasons currentSeason { get; }

        event Action<float> updatedSeason;


        void Init();

        float GetCurrentSeasonImpact();
    }
}
