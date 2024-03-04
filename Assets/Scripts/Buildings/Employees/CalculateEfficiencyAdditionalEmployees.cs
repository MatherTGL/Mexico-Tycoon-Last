using System.Collections.Generic;
using Expense;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;

namespace Building.Additional
{
    public sealed class CalculateEfficiencyAdditionalEmployees
    {
        //TODO: complete and refactoring || maybe change to efficiency percentage
        public ushort GetEfficiencyAdditionalEmployees(in IObjectsExpensesImplementation objectsExpenses,
            in Dictionary<TypeEmployee, byte> requiredEmployees, in TypeResource typeProductionResource)
        {
            ushort effenciency = 0;

            foreach (var employee in objectsExpenses.IhiringModel.GetAllEmployees().Keys)
            {
                var countEmployees = objectsExpenses.IhiringModel.GetAllEmployees()[employee].Count;

                if (countEmployees > requiredEmployees[employee] == false)
                    continue;

                for (int indexEmployee = requiredEmployees[employee] - 1; indexEmployee < countEmployees; indexEmployee++)
                {
                    if (!objectsExpenses.IhiringModel.GetAllEmployees()[employee][indexEmployee].efficiencyDictionary.ContainsKey(typeProductionResource))
                        continue;

                    effenciency += objectsExpenses.IhiringModel
                        .GetAllEmployees()[employee][indexEmployee].efficiencyDictionary[typeProductionResource];
                }
            }
            return effenciency;
        }
    }
}
