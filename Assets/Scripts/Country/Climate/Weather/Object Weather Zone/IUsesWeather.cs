using Country.Climate.Weather;

namespace Building.Additional
{
    public interface IUsesWeather
    {
        void ActivateWeatherEvent(in IWeatherZone IweatherZone);

        void DeactiveWeatherEvent();
    }
}
