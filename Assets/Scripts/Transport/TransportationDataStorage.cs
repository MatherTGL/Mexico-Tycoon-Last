using UnityEngine;
using System.Collections.Generic;

namespace Transport
{
    public sealed class TransportationDataStorage
    {
        public List<GameObject> l_purchasedTransportSprite { get; } = new();

        public List<SelfTransport> l_purchasedTransportData { get; } = new();

        public List<bool> l_transportTransferStatus { get; set; } = new();


        private void RemoveTransportationFromList(in ushort index)
        {
            l_purchasedTransportData[index].Dispose();
            l_purchasedTransportData.RemoveAt(index);
            l_transportTransferStatus.RemoveAt(index);
            l_purchasedTransportSprite.RemoveAt(index);
        }

        private void RemoveObjectsFromList(in ushort index)
        {
            RemoveTransportationFromList(index);
        }

        public void AddObject(in GameObject objectSprite, in SelfTransport objectData)
        {
            l_purchasedTransportSprite.Add(objectSprite);
            l_purchasedTransportData.Add(objectData);
            l_transportTransferStatus.Add(false);
        }

        public void RemoveObjectsFromList(in ushort[] indexes)
        {
            for (ushort index = 0; index < indexes.Length - 1; index++)
                RemoveTransportationFromList(index);
        }

        public GameObject DestroyTransport(in ushort index)
        {
            GameObject spriteObject = l_purchasedTransportSprite[index]; 
            RemoveObjectsFromList(index);
            return spriteObject;
        }

        public void SetTransferStatus(in ushort index, in bool isStatus)
        {
            l_transportTransferStatus[index] = isStatus;
        }
    }
}
