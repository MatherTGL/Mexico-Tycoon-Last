using System.Collections.Generic;


namespace Road
{
    public sealed class RoadResourcesManagement
    {
        private Dictionary<IPluggableingRoad, IPluggableingRoad[]> d_allConnectedObjects = new Dictionary<IPluggableingRoad, IPluggableingRoad[]>();


        public void CreateNewRoute(IPluggableingRoad fromSending, IPluggableingRoad whereSending)
        {
            if (!d_allConnectedObjects.ContainsKey(fromSending))
            {
                d_allConnectedObjects.Add(fromSending, new IPluggableingRoad[4]); //! заменить 4 на константу максимального количество подкл

                for (int freeIndexElement = 0; freeIndexElement < d_allConnectedObjects[fromSending].Length; freeIndexElement++)
                {
                    if (d_allConnectedObjects[fromSending][freeIndexElement] is null)
                    {
                        d_allConnectedObjects[fromSending][freeIndexElement] = whereSending;
                        return;
                    }
                }
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

        public void DestroyRoute(IPluggableingRoad fromSending, IPluggableingRoad whereSending)
        {
            if (d_allConnectedObjects.ContainsKey(fromSending))
            {
                var allRouteFromSending = d_allConnectedObjects[fromSending];

                for (int i = 0; i < allRouteFromSending.Length; i++)
                    if (d_allConnectedObjects[fromSending][i] == whereSending)
                        d_allConnectedObjects[fromSending][i] = null;
            }
        }

        public IPluggableingRoad[] CheckAllConnectionObjectsRoad(IPluggableingRoad sendingObject)
        {
            return d_allConnectedObjects[sendingObject];
        }
    }
}
