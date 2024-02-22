using System.Collections.Generic;
using Config.Expenses;
using Sirenix.OdinInspector;
using UnityEngine;
using static Config.Employees.ConfigEmployeeEditor;
using static Expense.ExpensesEnumTypes;

namespace Expense
{
    public sealed class ExpenseManagementControl : MonoBehaviour, IExpensesManagement
    {
        [ShowInInspector, ReadOnly]
        private readonly List<IUsesExpensesManagement> l_usesExpensesObjects = new();

        [ShowInInspector, ReadOnly]
        private readonly List<IObjectsExpensesImplementation> l_objectsExpensesImplementation = new();


        private ExpenseManagementControl() { }

        IObjectsExpensesImplementation IExpensesManagement.Registration(
            in IUsesExpensesManagement IusesExpensesManagement, in ExpensesTypeObject typeObject,
            in ConfigExpensesManagementEditor configExpenses) //TODO: refactoring
        {
            l_usesExpensesObjects.Add(IusesExpensesManagement);
            return CheckTypeAndCreateComponent(typeObject, configExpenses);
        }

        private IObjectsExpensesImplementation CheckTypeAndCreateComponent(
            in ExpensesTypeObject typeObject, in ConfigExpensesManagementEditor configExpenses)
        {
            if (typeObject == ExpensesTypeObject.Building)
            {
                IObjectsExpensesImplementation objectExpenses = new ExpensesBuildings(configExpenses);
                l_objectsExpensesImplementation.Add(objectExpenses);
                return objectExpenses;
            }
            else return null;
        }


        [SerializeField, ToggleLeft, BoxGroup("Change Expenses")]
        private bool _isAdd;

        [SerializeField, BoxGroup("Change Expenses")]
        private ushort _index;


        [Button("Change Expenses"), DisableInEditorMode, BoxGroup("Change Expenses")]
        private void ChangeExpensesOnBuildings(in double addNumber,
                                            in AreaExpenditureType typeExpenses)
        {
            if (!l_objectsExpensesImplementation.IsNotEmpty(_index) && l_objectsExpensesImplementation[_index] == null)
                return;

            l_objectsExpensesImplementation[_index]?.ChangeExpenses(addNumber, typeExpenses, _isAdd);
        }

        [Button("Change Employee Expenses"), DisableInEditorMode, BoxGroup("Change Expenses")]
        private void ChangeExpensesOnEmployees(in double addNumber, in TypeEmployee typeEmployee)
        {
            if (!l_objectsExpensesImplementation.IsNotEmpty(_index) && l_objectsExpensesImplementation[_index] == null)
                return;

            l_objectsExpensesImplementation[_index]?.ChangeEmployeesExpenses(addNumber, _isAdd, typeEmployee);
        }
    }
}
