using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /*
     * An inventory is a collection of items, tied to a physical location
     * The items are distributed among 1 or more "containers"
     * containers could include 
     *      literal containers: bags, boxes, chests, etc.
     *      creature bodyparts that are capable of grip, such as hands,arms,mouth,etc
     *      bodyparts that items are "equipped" onto
     *      "piles of stuff" to group multiple game objects
     *      
     *      
     *  Creatures have a CreatureInventory, 
     *  which has an Inventory and adds creature things,
     *  such as equipping, encumbrance calculations, 
     *  and allowing items to be sub-assigned to equipped containers,
     *  which allows dropping those containers with the items inside, 
     *  and could affect the encumbrance effect of the items
     *  
     */
    public class UInventory : IInventory
    {
        private int id = -1;
        public int ID { get { return id; } }
        public Dictionary<int, UItemData> items;

        public UInventory(List<UItemData> iitems=null)
        {
            items = new Dictionary<int, UItemData>();
            if(iitems != null)
            {
                foreach (UItemData it in iitems)
                {
                    items.Add(it.ID, it);
                }
            }
        }

        public void RemoveItem(UItemData itemData)
        {
            items.Remove(itemData.id);
        }
        public bool AddItem(UItemData itemData, int containerId)
        {
            if (!items.TryAdd(itemData.id, itemData)) return false;


            return true;
        }
        public UItemData GetItem(int id)
        {
            return items[id];
        }
        public List<int> GetIds()
        {
            return new List<int>(items.Keys);
        }


    }
}