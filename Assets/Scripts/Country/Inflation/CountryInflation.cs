using System.Collections;
namespace Country.Inflation
{
    public sealed class CountryInflation : ICountryInflation
    {
        private float _currentTotalInflation;

        private float _percentageInflationPerTimeTick = 0.1f; //? Config load


        void ICountryInflation.Init(in CountryControl countryControl)
        {
            countryControl.timeUpdated += CalculateInflation;
        }

        private void CalculateInflation()
        {
            _currentTotalInflation += _percentageInflationPerTimeTick;
        }

        float ICountryInflation.GetTotalInflation() => _currentTotalInflation;
    }
}
