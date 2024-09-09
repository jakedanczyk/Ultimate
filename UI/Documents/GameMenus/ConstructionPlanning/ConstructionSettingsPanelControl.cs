using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    public class ConstructionSettingsPanelControl : UIPanelControl
    {
        public VisualElement optionsPanel;
        public VisualElement itemCamPanel;
        public VisualElement detailPanel;

        public Label title;
        public Label suppliesList;
        public Slider wSlider;
        public Slider lSlider;
        public Slider hSlider;
        public Slider rSlider;
        public Toggle snaptoToggle;
        public DropdownField snaptoDropdown;
        public static ConstructionSettingsPanelControl Instance { get; private set; }
        public StaticPrefab selectedPrefab;
        public ConstructionWorksite selectedWorksite;
        public Camera viewCam;
        public GameObject viewObject;
        public Transform viewTarget;

        public ConstructionPlayer constructionPlayer;

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
            rootElement = document.rootVisualElement.Query(UrthConstants.CONSTRUCTION_SETTINGS_PANEL).First();
            //Build();
        }

        public void Update()
        {
            if (selectedPrefab != null && selectedWorksite != null 
                && selectedWorksite.supplies != null  && selectedWorksite.supplies.Count > 0)
            {
                Debug.Log(selectedWorksite.supplies[0].countNeeded);
                suppliesList.text = selectedWorksite.supplies[0].countNeeded.ToString();
            }
        }

        public void Link(VisualElement root)
        {
            rootElement = root;
            RegisterBorderCallbacks();
            optionsPanel = rootElement;
            title = (Label)((List<VisualElement>)optionsPanel.Query("titleBar").First().Children())[0];
            List<VisualElement> contentElements = (List<VisualElement>)optionsPanel.Query("content").First().Children();
            itemCamPanel = contentElements[0];
            detailPanel = contentElements[1];
            lSlider = (Slider)detailPanel.Query("lSlider").First();
            suppliesList = detailPanel.Query<Label>("suppliesList").First();
            Debug.Log(suppliesList);
        }


        public void SetItem(USTATIC staticType)
        {
            SetItem(StaticsLibrary.Instance.prefabsDict[staticType]);
        }
        public void SetItem(StaticPrefab newSelectedPrefab)
        {
            if(selectedPrefab == null || newSelectedPrefab.staticType != selectedPrefab.staticType)
            {
                selectedPrefab = newSelectedPrefab;
                selectedWorksite = constructionPlayer.currentPreview.constructionWorksite;

                SetDisplay();
            }
        }
        public void SetDisplay()
        {
            SetCamView();
            SetText();
        }

        void SetCamView()
        {
            if(viewObject != null)
            {
                Destroy(viewObject);
            }
            viewObject = Instantiate(StaticsLibrary.Instance.prefabsDict[selectedPrefab.staticType].finalPrefab,Vector3.zero,Quaternion.identity, viewTarget);
        }

        void SetText()
        {
            title.text = selectedPrefab.staticTypeName;
            //Debug.Log(selectedWorksite.supplies.Capacity);
        }

    }

}