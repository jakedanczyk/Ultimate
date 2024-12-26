using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Urth
{   /* 
     * Creatures have a CreatureInventory, 
     * which has an Inventory and adds creature things,
     * such as equipping, encumbrance calculations,
     * and allowing items to be sub-assigned to equipped containers,
     * which allows dropping those containers with the items inside,
     * and could affect the encumbrance effect of the items
     * 
     * Items may need to be dropped if a creature raises its guard or attacks
     * For the player, items are "carried in hand" as a default container when the rest of containers are full,
     * (or if they choose to assign items to be carried by hand)
     * For many animal-like npcs, carrying items with a bodypart will be the norm 
     * and they will tend to drop anything in inventory upon entering combat
     * 
     * There is a list and hashset for equipped items
     * When equipping an item:
     *  -we must ensure that it can be equipped (sizing, layer space, skill or other requirements)
     *  -Then add its ID to the list and hashset
     *  -Then equipment values are recalculated:
     *      -iterate over equipped items list
     *          -iterate over item bodypart slots
     *              -for each part, add equipment values to part layers list
     *              -item parts belong to a layer group: soft, hard, or cover
     *               within each group, physical layer order is determined by equipping order
     *               (hardcore mode could require removing cover/hard layer before adding soft/hard layers)
     *      
     */
    public class CreatureInventory : ICreatureInventory
    {
        public int inventoryId;
        public UInventory inventory;
        public CreatureBody body;
        public HashSet<int> equippedHashSet; 
        public List<int> equippedList;
        int rightContainerId;
        int leftContainerId;
        int mouthContainerId;
        public bool carryingLeft = false, carryingRight = false, carryingMouth = false;
        public UItemData rightItem;
        public UItemData leftItem;
        public float leftItemReach;
        public float rightItemReach;

        int nextContainerID = 0;
        public int GetNextContainerId()
        {
            return nextContainerID++;
        }
        public Dictionary<int, UItemContainer> containers;//id-container dict
        public HashSet<int> reserveContainers;//reserve containers get filled last in AutoPack
        public List<int> containerPreferenceOrder;//generic preference order. items go into first container with enough space
        public List<int> reservePreferenceOrder;//generic preference order. items go into first container with enough space
        public Dictionary<int, int> itemsToContainersMap;//which container items are actually in
        public Dictionary<int, VisualItemContainer> visualContainers;//which visual container items are actually in

        public Dictionary<int, int> itemAssignedContainers;//specifically assigned containers for items
        public Dictionary<ITEM, int> itemTypePreferredContainers;//preferred containers for items of a given type, e.g. clothes bag, ore bag
        //public HashSet<int> preferredContainersSet;//not if this is needed
        public CreatureInventory(CreatureBody ibody)
        {
            body = ibody;
            (inventoryId, inventory) = InventoriesManager.Instance.NextInventory();
            //inventory = new UInventory();
            itemsToContainersMap = new Dictionary<int, int>();
            containers = new Dictionary<int, UItemContainer>();
            reserveContainers = new HashSet<int>();
            visualContainers = new Dictionary<int, VisualItemContainer>();
            itemAssignedContainers = new Dictionary<int, int>();
            itemTypePreferredContainers = new Dictionary<ITEM, int>();
            containerPreferenceOrder = new List<int>();
            reservePreferenceOrder = new List<int>();
            //preferredContainersSet = new HashSet<int>();
            InitBodyContainers();
            equippedHashSet = new HashSet<int>();
            equippedList = new List<int>();
        }

        public void InitBodyContainers()
        {
            foreach(var tuple in UrthConstants.standardBodyContainers)
            {//TODO add stuff for visual containers
                (BodyPartId bp, float f, CONTAINER c) = tuple;
                int id = GetNextContainerId();
                UItemContainer newContainer = new UItemContainer();
                newContainer.type = c;
                newContainer.totalSpace = f * body.data.height * body.data.height * body.data.height;
                newContainer.availableSpace = newContainer.totalSpace;
                newContainer.containerBodyPartId = bp;
                containers.Add(id, newContainer);
                reserveContainers.Add(id);
                reservePreferenceOrder.Add(id);
                switch (bp.sym)
                {
                    case SYMMETRY.CENTER:
                        mouthContainerId = id;
                        break;
                    case SYMMETRY.LEFT:
                        leftContainerId = id;
                        break;
                    case SYMMETRY.RIGHT:
                        rightContainerId = id;
                        break;
                }
            }
        }

        

        public UItemData GetItem(int id)
        {
            return inventory.GetItem(id);
        }

        public List<int> GetIds()
        {
            return inventory.GetIds();
        }
        public void RemoveItem(UItemData itemData)
        {
            inventory.RemoveItem(itemData);
        }
        public bool AddItem(UItemData itemData, int containerId)
        {
            itemsToContainersMap[itemData.ID] = itemData.ID;
            containers[containerId].AddItem(itemData);
            return inventory.AddItem(itemData, containerId);
        }
        public int GetTargetContainerId(UItemData itemData)
        {
            var check = CheckForPreferredContainer(itemData);
            if (check.Item1)
            { //if there is a preferred container, check for space
                if (itemData.volumeAssembled < containers[check.Item2].availableSpace)
                {//put it in preferred container
                    return check.Item2;
                }
            }
            //else try put in first container in general preference order with space available
            foreach (int containerId in containerPreferenceOrder)
            {
                if (itemData.volumeAssembled < containers[containerId].availableSpace)
                {
                    return containerId;
                }
            }
            //else try put in first reserve container in reserve preference order with space available
            foreach (int containerId in reservePreferenceOrder)
            {
                if (itemData.volumeAssembled < containers[containerId].availableSpace)
                {
                    return containerId;
                }
            }
            return -1;
        }
        public UrthResponse TryPickupItem(UItemData itemData)
        {
            //check if creature can bear the weight
            if (itemData.weightAssembled + effectiveEncumbrance > body.stats.GetMaxEncumbrance())
            {
                return new UrthResponse(false, "weight");
            }
            int targetContainerId = GetTargetContainerId(itemData);
            if(targetContainerId == -1)
            {
                return new UrthResponse(false, "volume");
            }
            AddItem(itemData, targetContainerId);
            if (visualContainers.ContainsKey(targetContainerId))
            {
                visualContainers[targetContainerId].AddItem(itemData.item);
            }
            else
            {
                itemData.item.gameObject.SetActive(false);
            }
            return new UrthResponse(true);
        }

        public (bool, int) CheckForPreferredContainer(UItemData item)
        {
            if (itemAssignedContainers != null && itemAssignedContainers.ContainsKey(item.ID))
            {
                return (true, itemAssignedContainers[item.ID]);
            }
            else if (itemTypePreferredContainers != null && itemTypePreferredContainers.ContainsKey(item.type))
            {
                return (true, itemTypePreferredContainers[item.type]);
            }
            return (false, -1);
        }
        public void CalculateEquipmentValues()
        {

        }

        public UrthResponse CheckCanWieldItem(int id)
        {
            UItemData item = GetItem(id);
            return CheckCanWieldItem(item);
        }
        public UrthResponse CheckCanWieldItem(UItemData item)
        {
            UrthResponse response = new UrthResponse(true);
            //float itemMoi = 0f; //TODO test if item can be wielded properly
            return response;
        }

        public UrthResponse CheckCanEquipItem(int id)
        {
            UItemData item = GetItem(id);
            return CheckCanEquipItem(item);
        }
        
        public UrthResponse CheckCanEquipItem(UItemData item)
        {
            UrthResponse response = new UrthResponse(true);
            if (!CheckCanEquipItemSize(item))
            {
                response.success = false;
                response.msg = "Item wrong size for this creature";
            }
            else if (!CheckCanEquipItemStats(item))
            {
                response.success = false;
                response.msg = "Creature stats wrong for this item";
            }
            else if (!CheckCanEquipItemSpace(item))
            {
                response.success = false;
                response.msg = "Body parts too crowded to equip this item";
            }
            return response;
        }
        public bool CheckCanEquipItemSize(UItemData item)
        {//is the item sized for this creature (big enough but not too big)?
            return true;
        }
        public bool CheckCanEquipItemStats(UItemData item)
        {//does the creature have the skills and attributes needed to equip this item?
            return true;
        }
        public bool CheckCanEquipItemSpace(UItemData item)
        {//can the item fit over/under already-equipped items
            //check over all body slots to be covered
            return true;
        }
        public UrthResponse EquipItem(UItemData itemData)
        {
            return EquipItem(itemData.ID);
        }
        public UrthResponse EquipItem(int id)
        {
            UItemData item = GetItem(id);
            UrthResponse canEquipResponse = CheckCanEquipItem(item);
            if (!canEquipResponse.success)
            {
                return canEquipResponse;
            }
            UrthResponse response = new UrthResponse(true);
            EquipItemRaw(id);
            CalculateEquipmentValues();
            return response;
        }
        private bool EquipItemRaw(int id)
        {
            if (equippedHashSet.Add(id))
            {
                equippedList.Add(id);
                return true;
            }
            else return false;
        }

        public UrthResponse EquipLeft(UItemData itemData)
        {
            return EquipLeft(itemData.ID);
        }
        public UrthResponse EquipLeft(int id)
        {
            UItemData item = GetItem(id);
            UrthResponse canWieldResponse = CheckCanWieldItem(item);
            //TODO check for injury before can wield
            if (!canWieldResponse.success)
            {
                return canWieldResponse;
            }
            UrthResponse response = new UrthResponse(true);
            leftItem = item;
            CalculateEquipmentValues();
            return response;
        }
        public UrthResponse EquipRight(UItemData itemData)
        {
            return EquipRight(itemData.ID);
        }
        public UrthResponse EquipRight(int id)
        {
            UItemData item = GetItem(id);
            UrthResponse canWieldResponse = CheckCanWieldItem(item);
            //TODO check for injury before can wield
            if (!canWieldResponse.success)
            {
                return canWieldResponse;
            }
            UrthResponse response = new UrthResponse(true);
            rightItem = item;
            CalculateEquipmentValues();
            return response;
        }
        public int GetLeftToolIdx()
        {
            if(leftItem == null)
            {
                return 0;
            }
            return leftItem.item.weapon.toolIdx;
        }
        public int GetRightToolIdx()
        {
            if (rightItem == null)
            {
                return 0;
            }
            return rightItem.item.weapon.toolIdx;
        }
        public int GetLeftWeaponIdx()
        {
            if (leftItem == null || leftItem.item.weapon == null)
            {
                return 0;
            }
            switch (leftItem.item.weapon.weaponGroup)
            {
                case WEAPON_GROUP.ARM:
                    return 0;
                case WEAPON_GROUP.ONE_HANDED:
                    return 1;
                case WEAPON_GROUP.POLEARM:
                    return 2;
                case WEAPON_GROUP.TWO_HANDED:
                    return 3;
                case WEAPON_GROUP.BOW:
                    return 4;
                case WEAPON_GROUP.SHIELD:
                    return 5;
                case WEAPON_GROUP.MARKSMAN:
                    return 6;
                case WEAPON_GROUP.THROWN:
                    return 7;
                default:
                    return 0;
            }
        }
        public int GetRightWeaponIdx()
        {
            if (rightItem == null || rightItem.item.weapon == null)
            {
                return 0;
            }
            switch (rightItem.item.weapon.weaponGroup)
            {
                case WEAPON_GROUP.ARM:
                    return 0;
                case WEAPON_GROUP.ONE_HANDED:
                    return 1;
                case WEAPON_GROUP.POLEARM:
                    return 2;
                case WEAPON_GROUP.TWO_HANDED:
                    return 3;
                case WEAPON_GROUP.BOW:
                    return 4;
                case WEAPON_GROUP.SHIELD:
                    return 5;
                case WEAPON_GROUP.MARKSMAN:
                    return 6;
                case WEAPON_GROUP.THROWN:
                    return 7;
                default:
                    return 0;
            }
        }

        public float totalEncumbrance;//total body encumbrance
        public float effectiveEncumbrance;//effective encumbrance depending on stance and gait
        /*Several sub-encumbrances are also calculated, these are used in combination with total encumbrance
         * to calculate effective encumbrance, depending on current stance and gait
         * e.g. while standing still, effEnc = totalEnc - feetEnc - 0.75*(lowerLegEnc + kneeEnc) - 0.5*upperLegEnc  - 0.25*hipEnc
         * while moving slow, effEnc = totalEnc
         * while walking, effEnc = totalEnc + feetEnc + 0.5*lowerLegEnc
         * while sprinting, effEnc = totalEnc + 2*feetEnc + lowerLegEnc + kneeEnc
         */
        public float feetEncumbrance;//encumbrance of feet only. 
        public float lowerLegEncumbrance;//encumbrance of lower legs only. 
        public float kneeEncumbrance;//encumbrance of knees only. 
        public float upperLegEncumbrance;//encumbrance of upper legs only. 
        public float hipEncumbrance;//encumbrance of hips only. 
        public float CalcTotalEncumbrance()
        {
            float itemEncumb;
            foreach(int id in GetIds())
            {
                UItemData item = GetItem(id);
                itemEncumb = item.Weight;
                totalEncumbrance += itemEncumb;
            }
            return totalEncumbrance;
        }
        public float CurrentEncumbrance()
        {
            effectiveEncumbrance = EffectiveEncumbrance(body.status.gait, body.status.posture);
            return effectiveEncumbrance;
        }
        public float EffectiveEncumbrance(GAIT gait, POSTURE posture)
        {
            switch (posture)
            {
                case POSTURE.STAND:
                    switch (gait)
                    {
                        case GAIT.IDLE:
                            effectiveEncumbrance = totalEncumbrance - feetEncumbrance - 0.75f * (lowerLegEncumbrance + kneeEncumbrance) - 0.5f * upperLegEncumbrance - 0.25f * hipEncumbrance;
                            break;
                        default:
                            float speedFrac = ((int)gait - 1) / 8f; //currently 9 gaits, first entry IDLE is handled above, so this line should produce between 0-1 e.g (0-8)/8
                            float fFeet = feetEncumbrance * speedFrac * 2f;
                            float fLLeg = lowerLegEncumbrance * speedFrac * 1.5f;
                            float fKnee = kneeEncumbrance * speedFrac * 1f;
                            float fULeg = upperLegEncumbrance * speedFrac * 0.5f;
                            effectiveEncumbrance = totalEncumbrance + fFeet + fLLeg + fKnee + fULeg;
                            break;
                    }
                    break;
            }
            return effectiveEncumbrance;
        }



        public float availablePreferredSpace = 0f;
        //public float availableReserveSpace = 0f;
        public void CalcAvailableVolume()
        {
            availablePreferredSpace = 0f;
            //availableReserveSpace = 0f;
            foreach ((int id, UItemContainer container) in containers)
            {
                //if (preferredContainersSet.Contains(container.id))
                //{
                //    availablePreferredSpace += container.availableSpace;
                //}
                //else
                //{
                //    availableReserveSpace += container.availableSpace;
                //}
                availablePreferredSpace += container.availableSpace;
            }
        }

        public bool packSmallestItemsFirst = true;
        public bool packSmallestBagsFirst = true;
        public void AutoPack()
        {
            List<(int, UItemData)> unsorted = inventory.items.Select(x => (First: x.Key,Second: x.Value)).ToList();
            List<(int, UItemData)> sorted = UrthUtility.SortItemsByFloat(ITEM_PROPERTY.VOLUME, unsorted);
            int containerIdx = 0;
            foreach(int id in containerPreferenceOrder)
            {
                UItemContainer container = containers[id];
                container.SetEmpty();
            }
            foreach (int id in reservePreferenceOrder)
            {
                UItemContainer container = containers[id];
                container.SetEmpty();
            }
            bool inReserves = false, reservesFilled = false;
            foreach ((int key, UItemData item) in sorted)
            {
                if (equippedHashSet.Contains(key))
                {
                    continue;
                }
                while(!inReserves && containerIdx < containerPreferenceOrder.Count)
                {
                    int containerId = containerPreferenceOrder[containerIdx];
                    UItemContainer container = containers[containerId];
                    if(item.volumeAssembled < container.availableSpace)
                    {
                        container.AddItem(item);
                    }
                    else
                    {
                        containerIdx++;
                        if(containerIdx > containerPreferenceOrder.Count - 1)
                        {
                            inReserves = true;
                            containerIdx = 0;
                        }
                    }
                }
                if(inReserves)
                {//in reserves
                    while (containerIdx < reservePreferenceOrder.Count)
                    {
                        int containerId = reservePreferenceOrder[containerIdx];
                        UItemContainer container = containers[containerId];
                        if (item.volumeAssembled < container.availableSpace)
                        {
                            container.AddItem(item);
                        }
                        else
                        {
                            containerIdx++;
                            if (containerIdx > reservePreferenceOrder.Count - 1)
                            {
                                reservesFilled = true;
                                Debug.Log("Reserved containers fully filled, that shouldn't happen " +
                                    "because you can't overfill and if you lose a container you lose the items it holds");
                            }
                        }
                    }
                }
                //check if body containers have items
                carryingLeft = containers[leftContainerId].filledSpace > 0;
                carryingRight = containers[rightContainerId].filledSpace > 0;
                carryingMouth = containers[mouthContainerId].filledSpace > 0;
            }
        }
        public void SortContainers()
        {
            List<(int, UItemContainer)> unsorted = containers.Select(x => (First: x.Key,Second: x.Value)).ToList();
            List<(int, UItemContainer)> sorted = UrthUtility.SortContainersBySize(unsorted, packSmallestBagsFirst);
            containerPreferenceOrder = new List<int>();
            foreach((int id, UItemContainer uItemContainer) in sorted)
            {
                containerPreferenceOrder.Add(id);
            }
            containerPreferenceOrder = new List<int>(from tuple in sorted select tuple.Item1);
        }

        public void SetReserveContainersOrder(List<UItemContainer> list)
        {
            reservePreferenceOrder = new List<int>(reserveContainers.Count);
            foreach (UItemContainer container in list)
            {
                if (reserveContainers.Contains(container.id))
                {
                    reservePreferenceOrder.Add(container.id);
                }
                else
                {
                    MessageLogControl.Instance.DebugMessage($"In SetReserveContainersOrder, container with id {container.id} is not found in attached inventory");
                }
            }
        }
    }
}