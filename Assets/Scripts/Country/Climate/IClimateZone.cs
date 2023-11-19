using System;
using Config.Country.Climate;
using Country.Climate;

namespace Climate
{
    public interface IClimateZone
    {
        event Action updatedSeason;


        void Init(in ICountryClimate IcountryClimate);

        ConfigClimateZoneEditor.TypeSeasons GetCurrentSeason();

        float GetCurrentSeasonImpact();
    }
}
