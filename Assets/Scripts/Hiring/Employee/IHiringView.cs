namespace Building.Hire
{
    public interface IHiringView
    {
        void HireEmployee(in byte indexEmployee, in IPossibleEmployees IpossibleEmployees);

        void FireEmployee(in byte indexEmployee, in IPossibleEmployees IpossibleEmployees);
    }
}