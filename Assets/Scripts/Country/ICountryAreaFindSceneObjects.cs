using Country.Climate.Weather;

namespace Country
{
    public interface ICountryAreaFindSceneObjects
    {
        void SetCountry(in ICountryBuildings IcountryBuildings);

        void ActivateWeatherEvent(in IWeatherZone IweatherZone);

        void DeactiveWeatherEvent(in IWeatherZone IweatherZone);
    }
}
