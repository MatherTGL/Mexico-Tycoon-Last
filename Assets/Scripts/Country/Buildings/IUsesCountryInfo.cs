namespace Country
{
    public interface IUsesCountryInfo
    {
        protected ICountryBuildings IcountryBuildings { get; set; }


        void SetCountry(in ICountryBuildings IcountryBuildings)
        {
            this.IcountryBuildings = IcountryBuildings;
        }
    }
}
