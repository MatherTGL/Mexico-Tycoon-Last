using Data;
using UnityEngine;
using static Data.Player.DataPlayer;

namespace Transport.Fuel
{
    public sealed class TransportationFuel
    {
        private readonly TypeTransport _typeTransport;

        private float _currentFuelQuantity;

        private bool _isVehiclesAreRefueling;


        public TransportationFuel(in TypeTransport typeTransport)
        {
            _typeTransport = typeTransport;
            _currentFuelQuantity = typeTransport.maxFuelLoad;
        }

        public void FuelConsumption()
        {
            if (IsFuelAvailable())
                _currentFuelQuantity -= Random.Range(_typeTransport.minFuelConsumptionInTimeStep,
                                                     _typeTransport.maxFuelConsumptionInTimeStep);
            else
                RefuelTransportation();
        }

        public bool IsFuelAvailable()
            => _currentFuelQuantity > 0 && !_isVehiclesAreRefueling;

        private void RefuelTransportation()
        {
            _isVehiclesAreRefueling = true;
            _currentFuelQuantity = 0;

            if (IsBuyFuel())
                for (ushort liter = 0; liter < _typeTransport.maxFuelLoad; liter++)
                    _currentFuelQuantity += _typeTransport.fillingFuelRatePerTimeStep;

            _isVehiclesAreRefueling = false;
        }

        private bool IsBuyFuel()
        {
            double totalCost = Mathf.RoundToInt(_typeTransport.fuelCostPerLiter * _typeTransport.maxFuelLoad);
            return DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(totalCost, SpendAndCheckMoneyState.Spend);
        }
    }
}
