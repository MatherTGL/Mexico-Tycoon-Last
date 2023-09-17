using UnityEngine;
using System.Collections.Generic;


namespace Transport
{
    public sealed class TransportationDataStorage
    {
        public List<GameObject> l_purchasedTransportSprite { get; } = new List<GameObject>();

        public List<SelfTransport> l_purchasedTransportData { get; } = new List<SelfTransport>();

        public List<bool> l_transportTransferStatus { get; set; } = new List<bool>();


        private void RemoveTransportationFromList(in ushort index)
        {
            l_purchasedTransportData.RemoveAt(index);
            l_purchasedTransportSprite.RemoveAt(index);
        }

        public void AddObject(in GameObject objectSprite, in SelfTransport objectData)
        {
            l_purchasedTransportSprite.Add(objectSprite);
            l_purchasedTransportData.Add(objectData);
            l_transportTransferStatus.Add(false);
        }

        public void RemoveObjectFromList(in ushort index)
        {
            RemoveTransportationFromList(index);
        }

        public void RemoveObjectsFromList(in ushort[] indexes)
        {
            for (ushort index = 0; index < indexes.Length - 1; index++)
                RemoveTransportationFromList(index);
        }

        public GameObject DestroyTransport(in ushort index)
        {
            l_purchasedTransportData[index].Dispose();
            RemoveObjectFromList(index);
            return l_purchasedTransportSprite[index];
        }

        public void SetTransferStatus(in ushort index, in bool isStatus)
        {
            l_transportTransferStatus[index] = isStatus;
        }
    }
}
