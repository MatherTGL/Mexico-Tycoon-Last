using Config.Country.Climate;
using Country.Climate;

namespace Climate
{
    public interface IClimateZone
    {
        void Init(in ICountryClimate IcountryClimate);

        ConfigClimateZoneEditor.TypeSeasons GetCurrentSeason();
    }
}
