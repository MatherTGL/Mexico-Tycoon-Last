using System;
using Config.Country.Climate;
using Country.Climate;

namespace Climate
{
    public interface IClimateZone
    {
        ISeasonControl seasonControl { get; }


        void Init(in ICountryClimate IcountryClimate);
    }
}
