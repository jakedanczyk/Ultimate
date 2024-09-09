using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class ConstructionPanelControl : UIPanelControl
    {
        public VisualElement optionsPanel;
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

        public Label labelType;
        public Label labelMaterial;
        public Label labelDesignQuality;
        public Label labelBuildQuality;

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
            optionsPanel = rootElement;
            labelType = (Label)((List<VisualElement>)optionsPanel.Query("titleBar").First().Children())[0];
            List<VisualElement> contentElements = (List<VisualElement>)optionsPanel.Query("content").First().Children();
            //previewPanel = contentElements[0];
            //inputPanel = contentElements[1];
            //lSlider = (Slider)inputPanel.Query("lSlider").First();
        }

        public void SetItem(USTATIC staticType)
        {
            SetItem(StaticsLibrary.Instance.prefabsDict[staticType]);
        }
        public void SetItem(StaticPrefab newSelectedPrefab)
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
        }

        void SetText()
        {
            labelType.text = selectedPrefab.staticTypeName;
            //labelMaterial.text = selectedPrefab.materialFractions != null ? selectedPrefab.PrimaryMaterial().ToString() : "No Material";
            labelDesignQuality.text = selectedPrefab.designQuality.ToString();
            labelBuildQuality.text = selectedPrefab.buildQuality.ToString();
        }
    }
}