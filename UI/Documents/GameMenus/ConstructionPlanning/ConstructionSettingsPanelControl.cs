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
        public Label lengthMinLabel, lengthMaxLabel, widthMinLabel, widthMaxLabel, heightMinLabel, heightMaxLabel,
            rotMinLabel, rotMaxLabel, secMinLabel, secMaxLabel;
        public Slider wSlider;
        public Slider lSlider;
        public Slider hSlider;
        public Slider rSlider;
        public Slider secondarySlider;
        public Toggle snaptoToggle;
        public DropdownField snaptoDropdown;
        public static ConstructionSettingsPanelControl Instance { get; private set; }
        public StaticPrefab selectedPrefab;
        public ConstructionWorksite selectedWorksite;
        public Camera viewCam;
        public GameObject viewObject;
        public Transform viewTarget;

        public ConstructionPlayer constructionPlayer;

        public float width;
        public float length;
        public float height;
        public float rotation;
        public float secondaryValue;
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

                selectedWorksite.width = width;
                selectedWorksite.length = length;
                selectedWorksite.height = height;
                selectedWorksite.rotation = rotation;
                selectedWorksite.SetSecondaryValue(secondaryValue);
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
            VisualElement scrollView = (detailPanel.ElementAt(0));// Query<ListView>("scrollView").First();

            lSlider = (Slider)scrollView.ElementAt(2);// (Slider)container.Query<Slider>("lengthSlider").First();
            //wSlider = (Slider)container.Query<Slider>("widthSlider").First();
            //hSlider = (Slider)container.Query<Slider>("heightSlider").First();
            //rSlider = (Slider)container.Query<Slider>("rotationSlider").First();
            //secondarySlider = (Slider)container.Query<Slider>("secondarySlider").First();

            List<VisualElement> list = scrollView.Query<VisualElement>(className: "unity-list-view__item").ToList();

            //List<Slider> q = scrollView.Query<Slider>().ToList();
            //VisualElement container1 = scrollView.ElementAt(0);
            //VisualElement container2 = container1.ElementAt(0);
            //VisualElement container3 = container2.ElementAt(0);
            ////VisualElement container4 = scrollView.ElementAt(0).ElementAt(0).ElementAt(0);
            //VisualElement container = scrollView.ElementAt(0).ElementAt(0).ElementAt(0);
            lSlider = (Slider)scrollView.Query<Slider>("lengthSlider").First();
            wSlider = (Slider)scrollView.Query<Slider>("widthSlider").First();
            hSlider = (Slider)scrollView.Query<Slider>("heightSlider").First();
            rSlider = (Slider)scrollView.Query<Slider>("rotationSlider").First();
            secondarySlider = (Slider)scrollView.Query<Slider>("secondarySlider").First();

            lSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                length = evt.newValue;
            });
            wSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                width = evt.newValue;
            });
            hSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                height = evt.newValue;
            });
            rSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                rotation = evt.newValue;
            });
            secondarySlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                secondaryValue = evt.newValue;
            });
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

                width = selectedWorksite.width;
                length = selectedWorksite.length;
                height = selectedWorksite.height;
                rotation = selectedWorksite.rotation;
                secondaryValue = selectedWorksite.GetSecondaryValue();

                SetDisplay();
            }
        }
        public void SetDisplay()
        {
            SetCamView();
            SetSliders();
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

        void SetSliders()
        {
            lSlider.lowValue = selectedWorksite.minLength;
            lSlider.highValue = selectedWorksite.maxLength;
            wSlider.lowValue = selectedWorksite.minWidth;
            wSlider.highValue = selectedWorksite.maxWidth;
            hSlider.lowValue = selectedWorksite.minHeight;
            hSlider.highValue = selectedWorksite.maxHeight;
            rSlider.lowValue = selectedWorksite.minRotation;
            rSlider.highValue = selectedWorksite.maxRotation;
            lSlider.value = selectedWorksite.length;
            wSlider.lowValue = 1f;
            wSlider.highValue = 20f;
            wSlider.value = selectedWorksite.width;
            hSlider.value = selectedWorksite.height;
            rSlider.value = selectedWorksite.rotation;
            if (selectedWorksite.hasSecondary)
            {
                secondarySlider.style.display = DisplayStyle.Flex;
                secondarySlider.lowValue = selectedWorksite.minSecondary;
                secondarySlider.highValue = selectedWorksite.maxSecondary;
                secondarySlider.value = selectedWorksite.GetSecondaryValue();
            }
            else
            {
                secondarySlider.style.display = DisplayStyle.None;
            }
        }
    }

}