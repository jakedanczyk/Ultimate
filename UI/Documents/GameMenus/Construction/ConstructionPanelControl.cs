using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class ConstructionPanelControl : UIPanelControl
    {
        public VisualElement constructionPanel;
        public VisualElement previewPanel;
        public VisualElement inputPanel;

        public Slider wSlider;
        public Slider lSlider;
        public Slider hSlider;
        public Slider rSlider;
        public Toggle snaptoToggle;
        public DropdownField snaptoDropdown;
        public static ConstructionPanelControl Instance { get; private set; }
        public StaticPrefab selectedPrefab;
        public ConstructionWorksite selectedWorksite;
        public Camera viewCam;
        public GameObject viewObject;
        public Transform viewTarget;

        public Label labelConstructionType;
        public Label labelMaterial;
        public Label labelDesignQuality;
        public Label labelBuildQuality;
        public Label labelProgress;
        public Label labelLength;
        public Label labelWidth;
        public Label labelHeight;
        public Label labelMass;
        public Label labelSecondaryLabel;
        public Label labelSecondaryData;
        

        public VisualElement suppliesListPanel;
        public ListView suppliesListView;
        public VisualElement suppliesTemplate;
        [SerializeField]
        VisualTreeAsset supplyItemTemplate;

        public ListView inventoryListView;
        [SerializeField]
        VisualTreeAsset inventoryItemTemplate;

        public List<UItemData> uiItemDataList;
        public List<int> sortedIds;
        public List<(int, float)> unsorteds;
        HashSet<ITEM_PROPERTY> stringSortPropsSet = new HashSet<ITEM_PROPERTY> { ITEM_PROPERTY.NAME };
        public ITEM_PROPERTY sortProp = ITEM_PROPERTY.NONE;

        public float scale;
        public STATIC_SIZE size;

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

        public override void Start()
        {
            rootElement = document.rootVisualElement.Query(UrthConstants.CONSTRUCTION_PANEL).First();
            //Build();
        }

        public void Link(VisualElement root)
        {
            rootElement = root;
            RegisterBorderCallbacks();
            constructionPanel = rootElement;
            labelConstructionType = (Label)((List<VisualElement>)constructionPanel.Query("titleBar").First().Children())[0];
            //suppliesListPanel = constructionPanel.Query("content").First().Query(";

            labelProgress = (Label)(constructionPanel.Query("progressLabel").First());

            labelBuildQuality = (Label)(constructionPanel.Query("quality").First().Children().Last());
            labelLength = (Label)(constructionPanel.Query("length").First().Children().Last());
            labelWidth = (Label)(constructionPanel.Query("width").First().Children().Last());
            labelHeight = (Label)(constructionPanel.Query("height").First().Children().Last());
            labelMass = (Label)(constructionPanel.Query("mass").First().Children().Last());
            labelSecondaryLabel = (Label)(constructionPanel.Query("secondary").First().Children().First());
            labelSecondaryData = (Label)(constructionPanel.Query("secondary").First().Children().Last());

            VisualElement content = constructionPanel.Query("content");
            VisualElement listElement = content.Query("listElement");
            VisualElement listElementTwo = listElement.Query("listElementTwo");
            inventoryListView = (ListView)(listElementTwo.Children().ToList().Last());
            suppliesListView = (ListView)(listElementTwo.Children().ToList()[1]);
            
            //List<VisualElement> contentElements = (List<VisualElement>)constructionPanel.Query("content").First().Children();
            List<VisualElement> q1 = listElementTwo.Query("suppliesNeededList").ToList();
            List<VisualElement> q2 = listElementTwo.Query("inventoryList").ToList();
            ListView ql1 = listElementTwo.Q<ListView>();
            //suppliesListView = listElementTwo.Q<ListView>("suppliesNeededList");
            //suppliesListView = (ListView)(listElementTwo.Children().ToList().First());//(ListView)(content.Query("suppliesNeededList").First());
            //inventoryListView = listElementTwo.Q<ListView>("inventoryList");
            //inventoryListView = (ListView)(listElementTwo.Children().ToList().First());//(content.Query("inventoryList").First());

            //previewPanel = contentElements[0];
            //inputPanel = contentElements[1];
            //lSlider = (Slider)inputPanel.Query("lSlider").First();
        }

        public void SetConstruction(USTATIC staticType)
        {
            SetConstruction(StaticsLibrary.Instance.prefabsDict[staticType]);
        }
        public void SetConstruction(StaticPrefab newSelectedPrefab)
        {
            selectedPrefab = newSelectedPrefab;
            selectedWorksite = newSelectedPrefab.constructionWorksite;
            SetDisplay();
        }

        //void SetCamView()
        //{
        //    if (viewObject != null)
        //    {
        //        Destroy(viewObject);
        //    }
        //    viewObject = Instantiate(StaticsLibrary.Instance.prefabsDict[selectedPrefab.staticType].finalPrefab, Vector3.zero, Quaternion.identity, viewTarget);
        //}

        public void SetDisplay()
        {
            //SetCamView();
            SetText();
            SetSuppliesListView();
        }

        void SetText()
        {
            labelConstructionType.text = selectedPrefab.staticTypeName;
            //labelMaterial.text = selectedPrefab.materialFractions != null ? selectedPrefab.PrimaryMaterial().ToString() : "No Material";
            //labelDesignQuality.text = selectedPrefab.designQuality.ToString();
            labelBuildQuality.text = selectedPrefab.buildQuality.ToString();

        }

        
        void SetSuppliesListView()
        {
            //sortedIds = new List<int>(playerInventory.inventory.items.Count);
            //switch (sortProp)
            //{
            //    case ITEM_PROPERTY.NONE:
            //        sortedIds = playerInventory.inventory.GetIds();
            //        break;
            //    case ITEM_PROPERTY.NAME:
            //        SortByString(sortProp);
            //        break;
            //    default:
            //        SortByFloat(sortProp);
            //        break;
            //}
            //uiItemDataList = new List<UItemData>(playerInventory.inventory.items.Count);
            //foreach (int id in sortedIds)
            //{
            //    NewDataEntry(id);
            //}
            suppliesListView.itemsSource = selectedWorksite.supplies;
            suppliesListView.makeItem = () => supplyItemTemplate.Instantiate();
            suppliesListView.bindItem = (VisualElement element, int index) =>
            {
                VisualElement itemElement = element.Query("constructionSupplyItem").First();
                VisualElement click = itemElement.Query("click").First();
                click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                //click.RegisterCallback<ClickEvent>(OnItemClick);
                Label itemNameLabel = itemElement.Query("itemNameLabel").First() as Label;//.Query("inventoryItemNameLabel").First() as Label;
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                itemNameLabel.text = selectedWorksite.supplies[index].type.ToString();

                Label itemCountActualLabel = itemElement.Query("itemCountActualLabel").First() as Label;//.Query("inventoryItemNameLabel").First() as Label;
                itemCountActualLabel.text = selectedWorksite.supplies[index].countActual.ToString();
                Label itemCountNeededLabel = itemElement.Query("itemCountNeededLabel").First() as Label;//.Query("inventoryItemNameLabel").First() as Label;
                itemCountNeededLabel.text = selectedWorksite.supplies[index].countNeeded.ToString();
            };
        }

        public void NewInventoryListItem(int id)
        {
            //VisualElement newItem = 
            //var newItem = inventoryItemTemplate.Instantiate();
            //itemsList.Add(newItem);
            UItemData item = selectedPrefab.inventory.items[id];
            uiItemDataList.Add(item);
            //InventoryPanelItem newItemElement = new InventoryPanelItem();
            //newItemElement.SetItem(item);
        }

        void SetInventoryListView()
        {
            //sortedIds = new List<int>(playerInventory.inventory.items.Count);
            //switch (sortProp)
            //{
            //    case ITEM_PROPERTY.NONE:
            //        sortedIds = playerInventory.inventory.GetIds();
            //        break;
            //    case ITEM_PROPERTY.NAME:
            //        SortByString(sortProp);
            //        break;
            //    default:
            //        SortByFloat(sortProp);
            //        break;
            //}
            //uiItemDataList = new List<UItemData>(playerInventory.inventory.items.Count);
            //foreach (int id in sortedIds)
            //{
            //    NewDataEntry(id);
            //}
            uiItemDataList = new List<UItemData>(selectedPrefab.inventory.items.Count);
            foreach (int id in sortedIds)
            {
                NewInventoryListItem(id);
            }
            inventoryListView.itemsSource = selectedWorksite.supplies;
            inventoryListView.makeItem = () => supplyItemTemplate.Instantiate();
            inventoryListView.bindItem = (VisualElement element, int index) =>
            {
                VisualElement itemElement = element.Query("constructionSupplyItem").First();
                VisualElement click = itemElement.Query("click").First();
                click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                //click.RegisterCallback<ClickEvent>(OnItemClick);
                Label itemNameLabel = itemElement.Query("itemNameLabel").First() as Label;//.Query("inventoryItemNameLabel").First() as Label;
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

                itemNameLabel.text = selectedWorksite.supplies[index].type.ToString();
            };
        }
    }
}