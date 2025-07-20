using Events.Buildings.PlantsDiseases.Epidemic;
using UnityEngine;

namespace Events.Buildings.Plants
{
    //TODO: https://yougile.com/team/bf00efa6ea26/#chat:bf97e526c303
    public sealed class PlantDiseasesEpidemic : IPlantDiseases
    {
        private readonly ConfigEventPlantDiseasesEpidemic _config;

        private bool _isEpidemic;


        public PlantDiseasesEpidemic(in ConfigEventPlantDiseasesEpidemic config)
        {
            if (config != null)
                _config = config;
            else
                throw new System.Exception("Config is null");
        }

        //TODO: complete
        void IPlantDiseases.Update(in IUsesBuildingsEvents buildingsEvents)
        {
            Debug.Log("IPlantDiseases.Update");
            if (!_isEpidemic)
            {
                Debug.Log("IPlantDiseases.Update _isEpidemic == false");
                float randomValueChance = Random.Range(0f, 1f);

                if (randomValueChance > _config.spawnChance)
                    return;

                _isEpidemic = true;
            }
            else
            {
                //! Doing Something
                Debug.Log("IPlantDiseases.Update _isEpidemic == true");
            }
        }
    }
}
