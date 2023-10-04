using System.Collections.Generic;
using Boot;
using Config.Expenses;
using Sirenix.OdinInspector;
using UnityEngine;
using static Boot.Bootstrap;

namespace Expense
{
    public sealed class ExpenseManagementControl : MonoBehaviour, IBoot, IExpensesManagement
    {
        public enum AddOrReduceNumber : byte { Add, Reduce }

        public enum Type : byte { Building, Transport }

        [ShowInInspector, ReadOnly]
        private List<IUsesExpensesManagement> l_usesExpensesObjects = new();

        [ShowInInspector, ReadOnly]
        private List<IObjectsExpensesImplementation> l_objectsExpensesImplementation = new();


        private ExpenseManagementControl() { }

        void IBoot.InitAwake()
        {
            Debug.Log("helllooo");
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
        {
            return (TypeLoadObject.SuperImportant, TypeSingleOrLotsOf.Single);
        }

        IObjectsExpensesImplementation IExpensesManagement.Registration(
            in IUsesExpensesManagement IusesExpensesManagement, in Type typeObject, 
            in ConfigExpensesManagementEditor configExpenses) //TODO: refactoring
        {
            l_usesExpensesObjects.Add(IusesExpensesManagement);
            return CheckTypeAndCreateComponent(typeObject, configExpenses);
        }

        private IObjectsExpensesImplementation CheckTypeAndCreateComponent(
            in Type typeObject, in ConfigExpensesManagementEditor configExpenses)
        {
            if (typeObject == Type.Building)
            {
                IObjectsExpensesImplementation objectExpenses = new ExpensesBuildings(configExpenses);
                l_objectsExpensesImplementation.Add(objectExpenses);
                return objectExpenses;
            }
            else return null;
        }


#if UNITY_EDITOR
        [Button("Add Expenses"), DisableInEditorMode]
        private void AddExpensesOnBuildings(in double addNumber, in ushort index,
                                            in ExpensesBuildings.TypeExpenses typeExpenses)
        {
            l_objectsExpensesImplementation[index]?.ChangeExpenses(addNumber, typeExpenses, AddOrReduceNumber.Add);
        }
    }
#endif
}
