using UnityEngine;

namespace Transport.Breakdowns
{
    public sealed class TransportationBreakdowns
    {
        private TypeTransport _typeTransport;

        private float _currentStrength;

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

        private void Repair()
        {
            _currentStrength = 0f;
            _isRepairs = true;

            for (ushort i = 0; i < _typeTransport.maxStrength; i += 1)
                _currentStrength += _typeTransport.speedOfRepairPerTimeStep;

            Debug.Log($"repair: {_currentStrength}");
            _isRepairs = false;
        }
    }
}
