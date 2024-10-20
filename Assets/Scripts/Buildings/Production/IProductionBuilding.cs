using System.Collections.Generic;
using Building.Additional.Crop;
using Expense;
using SerializableDictionary.Scripts;
using static Config.Employees.ConfigEmployeeEditor;
using static Resources.TypeProductionResources;

namespace Building.Additional.Production
{
    public interface IProductionBuilding
    {
        ConfigCropSpoilage configCropSpoilage { get; }

        IObjectsExpensesImplementation IobjectsExpensesImplementation { get; }

        TypeResource typeProductionResource { get; }

        Dictionary<TypeResource, double> amountResources { get; set; }

        Dictionary<TypeEmployee, byte> requiredEmployees { get; }

        Dictionary<TypeResource, SerializableDictionary<TypeResource, int>> requiredRawMaterials { get; }

        Dictionary<TypeResource, uint> localCapacityProduction { get; }

        Dictionary<TypeResource, float> harvestRipeningTime { get; }


        int GetBaseProductionPerformance(in TypeResource typeResource);

        void SetNewProductionResource(in TypeResource typeResource);
    }
}
