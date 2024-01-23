using System.Collections.Generic;

namespace Building.City.Deliveries
{
    public interface IDeliveries
    {
        List<IDeliveriesType> l_deliveriesType { get; }


        void AddNewContract(in IDeliveriesType deliveriesType);
    }
}
