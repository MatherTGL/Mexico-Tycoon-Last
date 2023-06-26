using UnityEngine;


namespace City
{
    public sealed class CityReproduction
    {
        private byte _mathematicalDivisor;

        private float _populationChangeStepPercentMax;

        private float _populationChangeStepPercentMin;


        public CityReproduction(in byte c_mathematicalDivisor,
                                in float populationChangeStepPercentMax,
                                in float populationChangeStepPercentMin)
        {
            _mathematicalDivisor = c_mathematicalDivisor;
            _populationChangeStepPercentMax = populationChangeStepPercentMax;
            _populationChangeStepPercentMin = populationChangeStepPercentMin;
        }

        public void ReproductionPopulation(ref uint populationCity)
        {
            var populationChangeStepPercent = Random.Range(_populationChangeStepPercentMin, _populationChangeStepPercentMax);
            uint addCountPeople = (uint)(populationCity * populationChangeStepPercent / _mathematicalDivisor);
            populationCity += addCountPeople;
        }
    }
}
