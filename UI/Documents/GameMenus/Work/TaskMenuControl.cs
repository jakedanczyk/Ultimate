using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    public class TaskMenuControl : UIPanelControl
    {
        public VisualElement taskMenuPanel;
        public VisualElement listPanel;
        public ListView taskListView;
        [SerializeField]
        VisualTreeAsset taskListItemTemplate;

        public VisualElement itemTemplate;

        public ItemDisplayPanelControl itemDisplayControl;
        public CreatureInventory playerCreatureInventory;

        public UItemData selectedItem;

        public List<UItemData> uiItemDataList;
        public List<int> sortedIds;
        public List<(int, float)> unsorteds;
        HashSet<ITEM_PROPERTY> stringSortPropsSet = new HashSet<ITEM_PROPERTY> { ITEM_PROPERTY.NAME };
        public ITEM_PROPERTY sortProp = ITEM_PROPERTY.NONE;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Link(VisualElement root)
        {
            rootElement = root;
            RegisterBorderCallbacks();
            taskMenuPanel = rootElement;
            listPanel = taskMenuPanel.Query("content").First().ElementAt(0).ElementAt(0);
            taskListView = listPanel.Q<ListView>();
        }   


        public void Populate(List<WORKTASK> tasks)
        {
            playerCreatureInventory = GameManager.Instance.playerCharacterBody.creatureInventory;
            taskListView.itemsSource = tasks;
            taskListView.makeItem = () => taskListItemTemplate.Instantiate();
            taskListView.bindItem = (VisualElement element, int index) =>
            {
                VisualElement itemElement = element.Query("taskItem").First();
                VisualElement click = itemElement.Query("click").First();
                click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                //click.RegisterCallback<ClickEvent>(OnItemClick);
                Label itemNameLabel = itemElement.Query("taskItemName").First().Query("taskItemNameLabel").First() as Label;
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

                itemNameLabel.text = tasks[index].ToString();
            };
        }
    }

}