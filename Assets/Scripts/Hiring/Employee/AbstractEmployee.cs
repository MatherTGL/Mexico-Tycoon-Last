using System.Collections.Generic;
using Config.Employees;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;

namespace Hire.Employee
{
    public abstract class AbstractEmployee
    {
        public TypeEmployee typeEmployee { get; protected set; }

        public ConfigEmployeeEditor config { get; protected set; }

        public ushort paymentCostPerDay { get; protected set; }

        public byte rating { get; protected set; }

        public Dictionary<TypeResource, ushort> efficiencyDictionary { get; protected set; } = new();


        public abstract AbstractEmployee Clone();
    }
}
