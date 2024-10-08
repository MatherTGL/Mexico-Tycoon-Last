using UnityEngine;

namespace Country.Inflation
{
    public sealed class CountryInflation : IInflation
    {
        private ICountryInflation _IcountryInflation;

        private float _currentTotalInflation;

        private float _currentPercentageInflation;


        void IInflation.Init(in ICountryInflation countryControl)
        {
            countryControl.timeUpdated += CalculateInflation;
            _currentPercentageInflation = countryControl.configInflation.startedInflation;
            _IcountryInflation = countryControl;
        }

        private void CalculateInflation()
        {
            _currentPercentageInflation = Random.Range(_IcountryInflation.configInflation.percentageDeflationMax,
                                                       _IcountryInflation.configInflation.percentageInflationMax);

            _currentTotalInflation += _currentPercentageInflation;
        }

        float IInflation.GetTotalInflation() => _currentTotalInflation;
    }
}
