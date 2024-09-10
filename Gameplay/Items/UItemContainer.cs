using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public enum CONTAINER
    {
        HAND,
        MOUTH,
        POCKET,
        BAG,
        BOX
    }

    /*Represents a physical item container, such as bag, rack, or hands
     * It belongs to an inventory, which can have more than one container 
     * For example a creature inventory will have separate containers for each bag it equips, 
     *  as well as natural containers such as hands or mouth
     * A 
     */
    public class UItemContainer
    {
        public int id;
        public CONTAINER type;
        public BodyPartId containerBodyPartId;
        public UItemData containerItemData;
        public UFurniture containerFurniture;
        public float totalSpace, filledSpace, availableSpace;
        public HashSet<int> ItemIds { get; set; }
        public UInventory inventory;

        public UItemContainer(HashSet<int> iids=null, BodyPartId ibpid=null, UItemData iitem=null, UFurniture ifurniture=null) 
        {
            ItemIds = iids == null ? new HashSet<int>() : iids;
            containerBodyPartId = ibpid;
            containerItemData = iitem;
            containerFurniture = ifurniture;
        }
        public void AddItem(UItemData idata)
        {
            ItemIds.Add(idata.ID);
            filledSpace += idata.volumeAssembled;
            availableSpace = totalSpace - filledSpace;
        }
        public void RemoveItem(int itemId)
        {
            ItemIds.Remove(itemId);
            UItemData itemData = inventory.items[itemId];
            filledSpace -= itemData.volumeAssembled;
            availableSpace = totalSpace - filledSpace;
        }
        public void SetEmpty()
        {
            filledSpace = 0;
            availableSpace = totalSpace;
            ItemIds = new HashSet<int>();
        }
        public void CalcSpace()
        {
            filledSpace = 0f;
            foreach(int id in ItemIds)
            {
                filledSpace += inventory.GetItem(id).volumeAssembled;
            }
            availableSpace = totalSpace - filledSpace;
        }
    }
}
