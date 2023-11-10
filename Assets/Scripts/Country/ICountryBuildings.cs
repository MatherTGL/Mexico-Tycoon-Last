using Climate;
using Config.Climate;
using Country.Inflation;

namespace Country
{
    public interface ICountryBuildings
    {
        ConfigClimateZoneEditor configClimate { get; }


        ICountryInflation IcountryInflation { get; }

        IClimateZone IclimateZone { get; }
    }
}
