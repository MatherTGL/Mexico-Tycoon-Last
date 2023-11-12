namespace Country.Inflation
{
    public interface IInflation
    {
        void Init(in ICountryInflation countryControl);

        float GetTotalInflation();
    }
}
