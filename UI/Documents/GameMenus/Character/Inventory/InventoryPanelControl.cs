using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    /*Interface panel to display an inventory as list of items
     * There are a number of columns of data, columns are sortable
     * There is a search box
     * There are catergory filters thats can be toggled on and off
     */
    public class InventoryPanelControl : UIPanelControl
    {
        public VisualElement inventoryPanel;
        public VisualElement itemsListPanel;

        public ListView itemsList;
        public VisualElement itemTemplate;
        [SerializeField]
        VisualTreeAsset inventoryItemTemplate;

        public ItemDisplayPanelControl itemDisplayControl;
        public CreatureInventory playerInventory;

        public UItemData selectedItem;

        public List<UItemData> uiItemDataList;
        public List<int> sortedIds;
        public List<(int, float)> unsorteds;
        HashSet<ITEM_PROPERTY> stringSortPropsSet = new HashSet<ITEM_PROPERTY> { ITEM_PROPERTY.NAME };
        public ITEM_PROPERTY sortProp = ITEM_PROPERTY.NONE;


        public static InventoryPanelControl Instance { get; private set; }
        public override void Awake()
        {
            base.Awake();
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }


        public void Link(VisualElement root)
        {
            rootElement = root;
            RegisterBorderCallbacks();
            inventoryPanel = rootElement;
            itemsListPanel = inventoryPanel.Query("content").First().Children().First().Children().First();
            itemsList = itemsListPanel.Q<ListView>();
            itemTemplate = itemsListPanel.Query("InventoryItemTemplate");
        }


        public void Populate()
        {
            playerInventory = GameManager.Instance.playerCharacterBody.creatureInventory;
            Reorder();
        }



        void Reorder()
        {
            sortedIds = new List<int>(playerInventory.inventory.items.Count);
            switch (sortProp)
            {
                case ITEM_PROPERTY.NONE:
                    sortedIds = playerInventory.inventory.GetIds();
                    break;
                case ITEM_PROPERTY.NAME:
                    SortByString(sortProp);
                    break;
                default:
                    SortByFloat(sortProp);
                    break;
            }
            uiItemDataList = new List<UItemData>(playerInventory.inventory.items.Count);
            foreach (int id in sortedIds)
            {
                NewDataEntry(id);
            }
                itemsList.itemsSource = uiItemDataList;
                itemsList.makeItem = () => inventoryItemTemplate.Instantiate();
                itemsList.bindItem = (VisualElement element, int index) =>
                {
                    VisualElement itemElement = element.Query("inventoryItem").First();
                    VisualElement click = itemElement.Query("click").First();
                    click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                //click.RegisterCallback<ClickEvent>(OnItemClick);
                Label itemNameLabel = itemElement.Query("inventoryItemName").First().Query("inventoryItemNameLabel").First() as Label;
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

                itemNameLabel.text = uiItemDataList[index].GetName();
                };

        }


        public void NewDataEntry(int id)
        {
            //VisualElement newItem = 
            //var newItem = inventoryItemTemplate.Instantiate();
            //itemsList.Add(newItem);
            UItemData item = playerInventory.inventory.items[id];
            uiItemDataList.Add(item);
            //InventoryPanelItem newItemElement = new InventoryPanelItem();
            //newItemElement.SetItem(item);
        }

        public void OnItemClick(ClickEvent evt, int idx)
        {
            // Only perform this action at the target, not in a parent
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            // Assign a random new color
            var targetBox = evt.target as VisualElement;
            VisualElement parent = targetBox.parent;
            var currColor = parent.style.backgroundColor.value;
            parent.style.backgroundColor = Color.green ;// new Color(currColor.r,currColor.g,currColor.b,1f);

            selectedItem = uiItemDataList[idx];

            itemDisplayControl.SetDisplayItem(selectedItem);
        }

        private Color GetRandomColor()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
        void SortByFloat(ITEM_PROPERTY sortProp)
        {
            List<(int, float)> unsorteds = new List<(int, float)>(playerInventory.inventory.items.Count);
            foreach ((int id, UItemData item) in playerInventory.inventory.items)
            {
                switch (sortProp)
                {
                    case ITEM_PROPERTY.WEIGHT:
                        unsorteds.Add((id, item.Weight));
                        break;
                    case ITEM_PROPERTY.LENGTH:
                        unsorteds.Add((id, item.Length));
                        break;
                    case ITEM_PROPERTY.VOLUME:
                        unsorteds.Add((id, item.volumeComponent));
                        break;
                }
            }
            List<(int, float)> sorteds = unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((int id, float val) in sorteds)
            {
                sortedIds.Add(id);
            }
        }

        void SortByString(ITEM_PROPERTY sortProp)
        {
            List<(int, string)> unsorteds = new List<(int, string)>(playerInventory.inventory.items.Count);
            foreach ((int id, UItemData item) in playerInventory.inventory.items)
            {
                switch (sortProp)
                {
                    case ITEM_PROPERTY.NAME:
                        unsorteds.Add((id, item.GetName()));
                        break;
                }
            }
            List<(int, string)> sorteds = unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((int id, string val) in sorteds)
            {
                sortedIds.Add(id);
            }
        }
    }
}