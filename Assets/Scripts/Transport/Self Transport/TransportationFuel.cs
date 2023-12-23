using Data;
using UnityEngine;
using static Data.Player.DataPlayer;
using Deb = DebugCustomSystem.DebugSystem;

namespace Transport.Fuel
{
    public sealed class TransportationFuel
    {
        private TypeTransport _typeTransport;

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
            {
                _currentFuelQuantity -= Random.Range(
                    _typeTransport.minFuelConsumptionInTimeStep, _typeTransport.maxFuelConsumptionInTimeStep);
                Deb.Log(_currentFuelQuantity, Deb.SelectedColor.Red, "Transport current fuel", "Transport");
            }
            else
                RefuelTransportation();
        }

        public bool IsFuelAvailable()
        {
            if (_currentFuelQuantity > 0 && !_isVehiclesAreRefueling)
                return true;
            else
                return false;
        }

        private void RefuelTransportation()
        {
            Deb.Log(_currentFuelQuantity, Deb.SelectedColor.Green, "Transport current fuel", "Transport");
            _isVehiclesAreRefueling = true;
            _currentFuelQuantity = 0;
            Deb.Log(_isVehiclesAreRefueling, Deb.SelectedColor.Green, "Transport in fuel", "Transport");

            if (BuyFuel())
            {
                for (ushort liter = 0; liter < _typeTransport.maxFuelLoad; liter++)
                {
                    _currentFuelQuantity += _typeTransport.fillingFuelRatePerTimeStep;
                    Deb.Log(_currentFuelQuantity, Deb.SelectedColor.Blue, "Transport current fuel", "Transport");
                }
            }
   
            _isVehiclesAreRefueling = false;
        }

        private bool BuyFuel()
        {
            double totalCost = Mathf.RoundToInt(_typeTransport.fuelCostPerLiter * _typeTransport.maxFuelLoad);
            return DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(totalCost, SpendAndCheckMoneyState.Spend);
        }
    }
}
