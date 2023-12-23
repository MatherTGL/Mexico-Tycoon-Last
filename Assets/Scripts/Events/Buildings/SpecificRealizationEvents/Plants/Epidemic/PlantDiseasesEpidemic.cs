using Events.Buildings.PlantsDiseases.Epidemic;
using UnityEngine;

namespace Events.Buildings.Plants
{
    public sealed class PlantDiseasesEpidemic : IPlantDiseases
    {
        private ConfigEventPlantDiseasesEpidemic _config;

        private bool _isEpidemic;


        public PlantDiseasesEpidemic(in ConfigEventPlantDiseasesEpidemic config)
        {
            _config = config;
        }

        void IPlantDiseases.Update(in IUsesBuildingsEvents buildingsEvents)
        {
            if (!_isEpidemic)
            {
                float randomValueChance = Random.Range(0f, 1f);

                if (randomValueChance > _config.spawnChance)
                    return;

                _isEpidemic = true;
            }
            else
            {
                //! Doing Something
            }
        }
    }
}
