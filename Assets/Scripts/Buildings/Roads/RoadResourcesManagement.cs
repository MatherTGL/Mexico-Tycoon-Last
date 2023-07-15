using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


namespace Road
{
    public sealed class RoadResourcesManagement
    {
        private Dictionary<IPluggableingRoad, IPluggableingRoad[]> d_allConnectedObjects = new Dictionary<IPluggableingRoad, IPluggableingRoad[]>();
        public Dictionary<IPluggableingRoad, IPluggableingRoad[]> allConnectedObjects => d_allConnectedObjects;


        public void CreateNewRoute(IPluggableingRoad fromSending, IPluggableingRoad whereSending)
        {
            if (d_allConnectedObjects.ContainsKey(fromSending) is false)
            {
                d_allConnectedObjects.Add(fromSending, new IPluggableingRoad[4]); //! заменить 4 на константу максимального количество подкл
                d_allConnectedObjects[fromSending][0] = whereSending;
            }
            else
            {
                for (int i = 0; i < d_allConnectedObjects[fromSending].Length; i++)
                {
                    if (d_allConnectedObjects[fromSending][i] is null)
                    {
                        d_allConnectedObjects[fromSending][i] = whereSending;
                        return;
                    }
                }
            }
        }

        public IPluggableingRoad[] CheckAllConnectionObjectsRoad(IPluggableingRoad sendingObject)
        {
            return d_allConnectedObjects[sendingObject]; //! тут проблема
        }
    }
}
