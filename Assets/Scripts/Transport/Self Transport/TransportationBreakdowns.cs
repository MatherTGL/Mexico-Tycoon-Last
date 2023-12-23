using Data;
using static Data.Player.DataPlayer;
using Deb = DebugCustomSystem.DebugSystem;

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
            Deb.Log(_currentStrength, Deb.SelectedColor.Green, "Transport current strength", "Transport");
            if ((_currentStrength - _typeTransport.damageInflicted) > _typeTransport.minStrength && !_isRepairs)
                _currentStrength -= _typeTransport.damageInflicted;
            else
                Repair();
        }

        public bool IsNotInRepair() => !_isRepairs;

        public void Repair()
        {
            do
            {
                _isRepairs = true;
                _currentStrength += _typeTransport.speedOfRepairPerTimeStep;
                Deb.Log(_currentStrength, Deb.SelectedColor.Green, "Transport current strength", "Transport");
                Deb.Log(_isRepairs, Deb.SelectedColor.Blue, "Transport in repair", "Transport");

                if (_currentStrength >= _typeTransport.maxStrength)
                {
                    DataControl.IdataPlayer.CheckAndSpendingPlayerMoney(_typeTransport.repairCost, SpendAndCheckMoneyState.Spend);

                    _currentStrength = _typeTransport.maxStrength;
                    _isRepairs = false;
                    Deb.Log(_currentStrength, Deb.SelectedColor.Green, "Transport current strength", "Transport");
                    Deb.Log(_isRepairs, Deb.SelectedColor.Orange, "Transport in repair", "Transport");
                }
            } while (_isRepairs);
        }
    }
}
