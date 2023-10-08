using Data;
using UnityEngine;
using static Data.Player.DataPlayer;

namespace Transport.Breakdowns
{
    public sealed class TransportationBreakdowns
    {
        private TypeTransport _typeTransport;

        private ushort _currentStrength;

        private bool _isRepairs;


        public TransportationBreakdowns(in TypeTransport typeTransport)
        {
            _typeTransport = typeTransport;
            _currentStrength = _typeTransport.maxStrength;
        }

        public void DamageVehicles()
        {
            Debug.Log(_currentStrength);
            if ((_currentStrength - _typeTransport.damageInflicted) > _typeTransport.minStrength && !_isRepairs)
                _currentStrength -= _typeTransport.damageInflicted;
            else
                Repair();
        }

        public bool IsNotInRepair() => !_isRepairs;

        public void Repair()
        {
            _isRepairs = true;
            _currentStrength += _typeTransport.speedOfRepairPerTimeStep;

            if (_currentStrength >= _typeTransport.maxStrength)
            {
                DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(_typeTransport.repairCost, 
                                                                    SpendAndCheckMoneyState.Spend);

                _currentStrength = _typeTransport.maxStrength;
                _isRepairs = false;
            }
        }
    }
}
