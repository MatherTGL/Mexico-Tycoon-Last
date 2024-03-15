using System.Collections.Generic;
using Config.Employees;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;

namespace Hire.Employee
{
    public abstract class AbstractEmployee
    {
        public TypeEmployee type { get; protected set; }

        public ConfigEmployeeEditor config { get; protected set; }

        public double paymentCostPerDay { get; protected set; }

        public int rating { get; protected set; }

        public Dictionary<TypeResource, ushort> efficiencyDictionary { get; protected set; } = new();


        public abstract AbstractEmployee Clone();

        public abstract void UpdateOffer(AbstractEmployee[] possibleEmployeesInShop = null);

        public abstract void UpdateState();
    }
}
