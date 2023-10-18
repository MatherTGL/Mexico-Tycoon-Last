using Config.Employees;
using static Config.Employees.ConfigEmployeeEditor;

namespace Hire.Employee
{
    public abstract class AbstractEmployee
    {
        public TypeEmployee typeEmployee { get; protected set; }

        public ConfigEmployeeEditor config { get; protected set; }

        public ushort paymentCostPerDay { get; protected set; }

        public byte rating { get; protected set; }

        public byte efficiency { get; protected set; }


        public abstract AbstractEmployee Clone();
    }
}
