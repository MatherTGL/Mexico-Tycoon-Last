using Hire.Employee;

namespace Building.Hire
{
    public interface IHiring
    {
        void ConstantUpdatingInfo();
        void Hire(in byte indexEmployee);
        void Firing(in byte indexEmployee);
    }
}
