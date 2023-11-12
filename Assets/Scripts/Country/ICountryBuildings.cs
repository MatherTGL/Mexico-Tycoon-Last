using Climate;
using Config.Country.Climate;
using Country.Inflation;

namespace Country
{
    public interface ICountryBuildings
    {
        ConfigClimateZoneEditor configClimate { get; }


        IInflation IcountryInflation { get; }

        IClimateZone IclimateZone { get; }
    }
}
