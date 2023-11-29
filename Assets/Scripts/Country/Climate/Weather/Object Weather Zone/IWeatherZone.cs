namespace Country.Climate.Weather
{
    public interface IWeatherZone
    {
        float impactWeatherZone { get; }


        void Init(in ICountryClimate countryClimate);
    }
}
