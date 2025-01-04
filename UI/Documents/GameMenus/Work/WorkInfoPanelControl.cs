using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    public class WorkInfoPanelControl : UIPanelControl
    {
        public VisualElement infoPanel;
        public VisualElement listPanel;
        public ListView listView;
        [SerializeField]
        VisualTreeAsset itemTemplate;

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
            infoPanel = rootElement;
            listPanel = infoPanel.Query("content").First().ElementAt(0).ElementAt(0);
            listView = listPanel.Q<ListView>();
        }

        public void PopulateTerrain(TerrainWorksite tw)
        {
            List<string> strings = new List<string>();
            foreach(TerrainBlockFraction layer in tw.GetExistentFractions())
            {
                strings.Add(layer.mat.ToString() + ":" + layer.volumeFraction);
            }
        }
        public void Populate(List<string> strings)
        {
            listView.itemsSource = strings;
            listView.makeItem = () => itemTemplate.Instantiate();
            listView.bindItem = (VisualElement element, int index) =>
            {
                VisualElement itemElement = element.Query("item").First();
                VisualElement click = itemElement.Query("click").First();
                click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                //click.RegisterCallback<ClickEvent>(OnItemClick);
                Label itemLabel = itemElement.Query("itemName").First().Query("itemNameLabel").First() as Label;
                Debug.Log("itemNamLabel is...");
                Debug.Log(itemLabel);
                Debug.Log(itemLabel.text);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

                itemLabel.text = strings[index].ToString();
            };
        }
    }

}