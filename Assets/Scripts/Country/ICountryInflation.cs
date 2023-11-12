using System;
using Config.Country.Inflation;

namespace Country.Inflation
{
    public interface ICountryInflation
    {
        ConfigInflationEditor configInflation { get; }

        event Action timeUpdated;
    }
}
