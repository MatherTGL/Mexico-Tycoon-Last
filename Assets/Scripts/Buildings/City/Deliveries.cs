using Config.Building.Deliveries;
using Mono.Cecil;
using Resources;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TimeControl;
using UnityEngine;

namespace Building.City.Deliveries
{
    //! The component is added after the game starts in runtime
    public sealed class Deliveries : MonoBehaviour, IDeliveries
    {
        private List<IDeliveriesType> l_deliveriesType = new();
        List<IDeliveriesType> IDeliveries.l_deliveriesType => l_deliveriesType;

        private ConfigDeliveriesEditor _configDeliveries;

        private CostResourcesConfig _costResourcesConfig;

        public enum TypeDeliveries : byte { General, Individual }

        private WaitForSeconds _waitForSeconds;


        void IDeliveries.Init(in ConfigDeliveriesEditor config, in CostResourcesConfig costResourcesConfig)
        {
            _waitForSeconds = new WaitForSeconds(FindAnyObjectByType<TimeDateControl>().GetCurrentTimeOneDay());
            _costResourcesConfig = costResourcesConfig;
            _configDeliveries = config;

            l_deliveriesType.Add(new GeneralDeliveries());
            StartCoroutine(TimeUpdate());
        }

        double IDeliveries.GetResourceCosts(in TypeProductionResources.TypeResource typeResource)
        {
            return l_deliveriesType[l_deliveriesType.Count - 1].GetResourceCost(typeResource, _costResourcesConfig);
        }

        [Button("Conclusion Individual Contract")]
        private void ConclusionOfContract(DataIndividualDeliveries contractData)
        {
            //TODO refactoring this code


            if (IsAvailabilityCompletedContracts(contractData))
            {
                Debug.Log("ConclusionOfContract updated success!");
                return;
            }
            else if (_configDeliveries.maxIndividualContracts > l_deliveriesType.Where(item => item.typeDeliveries is TypeDeliveries.Individual).ToArray().Length)
            {
                var individualDeliveries = new IndividualDeliveries(_configDeliveries, contractData);

                if (IsContractAlreadyExists(contractData))
                    return;

                l_deliveriesType.Add(individualDeliveries);
                Debug.Log($"AllContracts: {l_deliveriesType.Count}");
                Debug.Log("ConclusionOfContract created success!");
            }
        }

        private bool IsContractAlreadyExists(in DataIndividualDeliveries contractData)
        {
            for (byte typeDeliveries = 0; typeDeliveries < l_deliveriesType.Count; typeDeliveries++)
            {
                if (l_deliveriesType[typeDeliveries].typeDeliveries is TypeDeliveries.General)
                    continue;

                IIndividualDeliveries individualDeliveries = l_deliveriesType[typeDeliveries] as IIndividualDeliveries;

                if (individualDeliveries.GetResourceBeingSent() == contractData.resource)
                {
                    Debug.Log("ConclusionOfContract there is already a similar contract in place!");
                    return true;
                }
            }
            return false;
        }

        private bool IsAvailabilityCompletedContracts(in DataIndividualDeliveries contractData)
        {
            Debug.Log($"l_deliveriesType.Count: {l_deliveriesType.Count}");
            for (byte i = 0; i < l_deliveriesType.Count; i++)
            {
                if (l_deliveriesType[i] is IIndividualDeliveries individualContract && individualContract.IsContractIsFinalized())
                {
                    individualContract.UpdateContract(contractData);
                    return true;
                }
            }
            return false;
        }

        private IEnumerator TimeUpdate()
        {
            while (true)
            {
                for (byte i = 0; i < l_deliveriesType.Count; i++)
                {
                    if (l_deliveriesType[i] is IIndividualDeliveries contract && contract.IsContractIsFinalized() == false 
                        || l_deliveriesType[i] is not IIndividualDeliveries)
                    {
                        l_deliveriesType[i].UpdateTime();
                    }
                }          

                yield return _waitForSeconds;
            }
        }
    }
}
