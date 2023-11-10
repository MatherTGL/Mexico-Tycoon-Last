namespace Country.Inflation
{
    public interface ICountryInflation
    {
        void Init(in CountryControl countryControl);

        float GetTotalInflation();
    }
}
