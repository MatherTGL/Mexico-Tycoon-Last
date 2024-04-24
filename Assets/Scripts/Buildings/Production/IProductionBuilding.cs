using System.Collections.Generic;
using Expense;
using SerializableDictionary.Scripts;
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

        Dictionary<TypeResource, SerializableDictionary<TypeResource, int>> requiredRawMaterials { get; }

        Dictionary<TypeResource, uint> localCapacityProduction { get; }

        float harvestRipeningTime { get; }


        int GetBaseProductionPerformance(in TypeResource typeResource);

        void SetNewProductionResource(in TypeResource typeResource);
    }
}
