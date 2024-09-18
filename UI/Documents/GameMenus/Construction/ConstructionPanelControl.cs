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
        public Camera viewCam;
        public GameObject viewObject;
        public Transform viewTarget;

        public Label labelConstructionType;
        public Label labelMaterial;
        public Label labelDesignQuality;
        public Label labelBuildQuality;

        public VisualElement suppliesListPanel;
        public ListView suppliesListView;
        public VisualElement suppliesTemplate;
        [SerializeField]
        VisualTreeAsset supplyItemTemplate;

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
            suppliesListView = (ListView)constructionPanel.Query("supliesList").First();


            List<VisualElement> contentElements = (List<VisualElement>)constructionPanel.Query("content").First().Children();
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
            labelDesignQuality.text = selectedPrefab.designQuality.ToString();
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
            //itemsList.itemsSource = uiItemDataList;
            //itemsList.makeItem = () => inventoryItemTemplate.Instantiate();
            //itemsList.bindItem = (VisualElement element, int index) =>
            //{
            //    VisualElement itemElement = element.Query("inventoryItem").First();
            //    VisualElement click = itemElement.Query("click").First();
            //    click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
            //    //click.RegisterCallback<ClickEvent>(OnItemClick);
            //    Label itemNameLabel = itemElement.Query("inventoryItemName").First().Query("inventoryItemNameLabel").First() as Label;
            //    //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
            //    //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

            //    itemNameLabel.text = uiItemDataList[index].GetName();
            //};
        }
    }
}