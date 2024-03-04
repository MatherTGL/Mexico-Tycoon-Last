namespace Building.Hire
{
    public interface IHiringView
    {
        void HireEmployee(in byte indexEmployee);

        void FireEmployee(in byte indexEmployee);
    }
}