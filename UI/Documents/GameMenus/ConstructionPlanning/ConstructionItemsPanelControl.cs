using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    /*Interface panel to display available construction objects
     * There are a number of columns of data, columns are sortable
     * There is a search box
     * There are catergory filters thats can be toggled on and off
     */
    public class ConstructionItemsPanelControl : UIPanelControl
    {
        public VisualElement constructionPanel;
        public VisualElement listPanel;

        public ListView listView;
        public VisualElement itemTemplate;
        [SerializeField]
        VisualTreeAsset constructionItemTemplate;

        public ConstructionPlayer constructionPlayer;
        public ConstructionSettingsPanelControl constructionOptionsControl;
        //public StaticTemplate selectedTemplate;
        public USTATIC selectedType;
        public StaticPrefab selectedPrefab;

        //public List<StaticTemplate> uiTemplateData;
        public List<StaticPrefab> dataList;
        public List<USTATIC> sortedIds;
        public List<(USTATIC, float)> unsorteds;
        HashSet<ITEM_PROPERTY> stringSortPropsSet = new HashSet<ITEM_PROPERTY> { ITEM_PROPERTY.NAME };
        public ITEM_PROPERTY sortProp = ITEM_PROPERTY.NONE;

        public static ConstructionItemsPanelControl Instance { get; private set; }
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
            constructionPanel = rootElement;
            listPanel = constructionPanel.Query("content").First().Children().First().Children().First();
            listView = listPanel.Q<ListView>();
            itemTemplate = listPanel.Query("ConstructionItemTemplate");
        }


        public void BuildAndPopulate()
        {
            Build();
            Populate();
            listView.RefreshItems();
        }


        void Build()
        {
            constructionPanel = rootElement;
            listPanel = constructionPanel.Query("content").First().Children().First().Children().First();
            listView = listPanel.Q<ListView>();
            itemTemplate = listPanel.Query("ConstructionItemTemplate");
            //.Query("itemsList").First();
        }

        public void Populate()
        {
            Reorder();
        }

        
        
        public void Reorder()
        {
            ConstructionLibrary cm = ConstructionLibrary.Instance;
            sortedIds = new List<USTATIC>(cm.previewList.Count);
            switch (sortProp)
            {
                case ITEM_PROPERTY.NONE:
                    sortedIds = new List<USTATIC>(cm.previewDict.Keys);
                    break;
                case ITEM_PROPERTY.NAME:
                    SortByString(sortProp);
                    break;
                default:
                    SortByFloat(sortProp);
                    break;
            }
            dataList = new List<StaticPrefab>(cm.previewDict.Count);
            foreach (USTATIC type in sortedIds)
            {
                NewDataEntry(type);
            }
            if (listView.itemsSource == null)
            {
                listView.itemsSource = dataList;
                listView.makeItem = () => MakeListItem();// constructionItemTemplate.Instantiate();
                listView.bindItem = (VisualElement element, int index) =>
                {
                    VisualElement itemElement = element.Query("constructionItem").First();
                    VisualElement click = itemElement.Query("click").First();
                    click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                    //click.RegisterCallback<ClickEvent>(OnItemClick);
                    Label itemNameLabel = itemElement.Query("constructionItemName").First().Query("constructionItemNameLabel").First() as Label;
                    //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                    //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

                    itemNameLabel.text = dataList[index].staticTypeName;
                    if (dataList[index].staticType == selectedType)
                    {
                        element.style.backgroundColor = Color.green;// new Color(currColor.r,currColor.g,currColor.b,1f);
                    }
                };
            }

        }

        public VisualElement MakeListItem()
        {
            VisualElement listItem = constructionItemTemplate.Instantiate();
            
            return listItem;
        }


        public void NewDataEntry(USTATIC type)
        {
            //VisualElement newItem = 
            //var newItem = inventoryItemTemplate.Instantiate();
            //itemsList.Add(newItem);
            //StaticTemplate item = StaticsLibrary.Instance.templatesDict[type];
            StaticPrefab item = StaticsLibrary.Instance.prefabsDict[type];
            dataList.Add(item);
            //InventoryPanelItem newItemElement = new InventoryPanelItem();
            //newItemElement.SetItem(item);
        }

        public void OnItemClick(ClickEvent evt, int idx)
        {
            // Only perform this action at the target, not in a parent
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            //var targetBox = evt.target as VisualElement;
            //VisualElement parent = targetBox.parent;
            //var currColor = parent.style.backgroundColor.value;
            //parent.style.backgroundColor = Color.green;// new Color(currColor.r,currColor.g,currColor.b,1f);

            selectedType = dataList[idx].staticType;
            listView.Rebuild();
                
            constructionPlayer.ChangePreviewType(selectedType);
            constructionOptionsControl.SetItem(selectedType);

            //ConstructionManager.Instance.
        }

        private Color GetRandomColor()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
        void SortByFloat(ITEM_PROPERTY sortProp)
        {
            ConstructionLibrary cm = ConstructionLibrary.Instance;
            List<(USTATIC, float)> unsorteds = new List<(USTATIC, float)>(cm.previewList.Count);
            foreach (ConstructionPreview preview in cm.previewList)
            {
                StaticTemplate staticTemplate = StaticsLibrary.Instance.templatesDict[preview.staticType];
                switch (sortProp)
                {
                    //case ITEM_PROPERTY.WEIGHT:
                    //    unsorteds.Add((preview.staticType, staticTemplate.CalcStaticProps.Weight));
                    //    break;
                    case ITEM_PROPERTY.LENGTH:
                        unsorteds.Add((staticTemplate.staticType, staticTemplate.lwh.x));
                        break;
                    case ITEM_PROPERTY.WIDTH:
                        unsorteds.Add((staticTemplate.staticType, staticTemplate.lwh.y));
                        break;
                    case ITEM_PROPERTY.HEIGHT:
                        unsorteds.Add((staticTemplate.staticType, staticTemplate.lwh.z));
                        break;
                    case ITEM_PROPERTY.VOLUME:
                        unsorteds.Add((staticTemplate.staticType, staticTemplate.filledVolume));
                        break;
                }
            }
            List<(USTATIC, float)> sorteds = unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((USTATIC id, float val) in sorteds)
            {
                sortedIds.Add(id);
            }
        }

        void SortByString(ITEM_PROPERTY sortProp)
        {
            ConstructionLibrary cm = ConstructionLibrary.Instance;
            List <(USTATIC, string)> unsorteds = new List<(USTATIC, string)>(cm.previewList.Count);
            foreach (ConstructionPreview preview in cm.previewList)
            {
                switch (sortProp)
                {
                    case ITEM_PROPERTY.NAME:
                        unsorteds.Add((preview.staticType, preview.staticTypeName));
                        break;
                }
            }
            List<(USTATIC, string)> sorteds = unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((USTATIC type, string val) in sorteds)
            {
                sortedIds.Add(type);
            }
        }
    }
}