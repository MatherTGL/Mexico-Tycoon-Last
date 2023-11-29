using Country.Climate.Weather;
using Expense;

namespace Building.Additional
{
    public interface IUsesWeather
    {
        protected IObjectsExpensesImplementation IobjectsExpensesImplementation { get; }


        void ActivateWeatherEvent(in IWeatherZone IweatherZone) => CalculateTemporaryImpact(IweatherZone.impactWeatherZone);

        void DeactiveWeatherEvent(in IWeatherZone IweatherZone) => CalculateTemporaryImpact(0);

        private void CalculateTemporaryImpact(float impact)
        {
            double addingNumber = IobjectsExpensesImplementation.GetTotalExpenses() * impact / 100;
            IobjectsExpensesImplementation.ChangeSeasonExpenses(addingNumber);
        }
    }
}
