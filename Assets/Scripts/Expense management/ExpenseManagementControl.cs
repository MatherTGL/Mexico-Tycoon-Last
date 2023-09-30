using System;
using System.Collections.Generic;
using Boot;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Expense
{
    public sealed class ExpenseManagementControl : MonoBehaviour, IBoot, IExpensesManagement
    {
        public enum Type
        {
            Building, Transport
        }

        [ShowInInspector, ReadOnly]
        private List<IUsesExpensesManagement> l_usesExpensesObjects = new();

        [ShowInInspector, ReadOnly]
        private List<IObjectsExpensesImplementation> l_objectsExpensesImplementation = new();


        private ExpenseManagementControl() { }

        void IBoot.InitAwake()
        {
            Debug.Log("helllooo");
        }

        (Bootstrap.TypeLoadObject typeLoad, bool isSingle) IBoot.GetTypeLoad()
        {
            return (Bootstrap.TypeLoadObject.SuperImportant, true);
        }

        void IExpensesManagement.Registration(in IUsesExpensesManagement IusesExpensesManagement, in Type typeObject)
        {
            l_usesExpensesObjects.Add(IusesExpensesManagement);
            CheckTypeAndCreateComponent(typeObject);
        }

        private void CheckTypeAndCreateComponent(in Type typeObject)
        {
            if (typeObject == Type.Building)
                l_objectsExpensesImplementation.Add(new ExpensesBuildings());
        }

        [Button("Expenses Security"), DisableInEditorMode, Tooltip("Change Maintenance Expenses On Security")]
        private void ChangeMaintenanceExpensesOnSecurity(in double addNumber, in ushort index)
        {
            l_objectsExpensesImplementation[index]?.ChangeMaintenance(addNumber);
            l_usesExpensesObjects[index]?.SetMaintenanceExpensesOnSecurity(addNumber);
        }
    }
}
