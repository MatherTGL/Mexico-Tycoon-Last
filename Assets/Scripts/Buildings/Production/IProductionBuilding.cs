using System.Collections.Generic;
using Expense;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;

namespace Building.Additional.Production
{
    public interface IProductionBuilding
    {
        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; }

        TypeResource typeProductionResource { get; }

        Dictionary<TypeResource, double> amountResources { get; set; }

        Dictionary<TypeEmployee, byte> requiredEmployees { get; }

        List<TypeResource> requiredRawMaterials { get; }

        List<float> quantityRequiredRawMaterials { get; }

        uint[] localCapacityProduction { get; }

        ushort defaultProductionPerformance { get; }

        float harvestRipeningTime { get; }
    }
}
