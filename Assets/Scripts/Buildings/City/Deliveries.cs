using Sirenix.OdinInspector;
using UnityEngine;

namespace Building.City.Deliveries
{
    public sealed class Deliveries : MonoBehaviour, IDeliveries
    {
        private IDeliveriesType _IdeliveriesType;

        private enum TypeDeliveries : byte { General, Individual }

        [SerializeField, EnumToggleButtons]
        private TypeDeliveries _typeDeliveries;


        void IDeliveries.Init()
        {
            if (_typeDeliveries is TypeDeliveries.General)
                _IdeliveriesType = new GeneralDeliveries();
            else
                _IdeliveriesType = new IndividualDeliveries();
        }
    }
}
