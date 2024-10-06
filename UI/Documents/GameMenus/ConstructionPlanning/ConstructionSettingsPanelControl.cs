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
        public Label suppliesLabel;
        public Label detailsLabel;

        public VisualElement heightOffsetSliderContainer;
        public Label heightOffsetDisplay;
        public VisualElement heightSliderContainer;
        public Label heightDisplay;
        public VisualElement widthSliderContainer;
        public Label widthDisplay;
        public VisualElement lengthSliderContainer;
        public Label lengthDisplay;


        public VisualElement snapToElement,heightOffsetElement,heightElement,widthElement,lengthElement,rotationElement,secondaryElement,tertiaryElement;
        public TextField heightOffsetInput, lengthInput, heightInput, widthInput, rotationInput, secondaryInput,tertiaryInput;
        public Label heightOffsetMinLabel, heightOffsetMaxLabel, lengthMinLabel, lengthMaxLabel, widthMinLabel, widthMaxLabel, heightMinLabel, heightMaxLabel,
            rotationMinLabel, rotationMaxLabel, secondaryMinLabel, secondaryMaxLabel, tertiaryMinLabel, tertiaryMaxLabel;
        public Slider heightOffsetSlider;
        public Slider wSlider;
        public Slider lSlider;
        public Slider hSlider;
        public Slider rSlider;
        public Slider secondarySlider;
        public Slider tertiarySlider;
        public Toggle snaptoToggle;
        public DropdownField snaptoDropdown;
        public static ConstructionSettingsPanelControl Instance { get; private set; }
        public StaticPrefab selectedPrefab;
        public ConstructionWorksite selectedWorksite;
        public Camera viewCam;
        public GameObject viewObject;
        public Transform viewTarget;

        public ConstructionPlayer constructionPlayer;

        public float heightOffset;
        public float width;
        public float length;
        public float height;
        public float rotation;
        public float secondaryValue;
        public int tertiaryValue;
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
                //suppliesList.text = selectedWorksite.supplies[0].countNeeded.ToString();

                selectedWorksite.SetHeightOffset(heightOffset);
                selectedWorksite.width = width;
                selectedWorksite.length = length;
                selectedWorksite.SetHeight(height);
                selectedWorksite.rotation = rotation;
                selectedWorksite.SetSecondaryValue(secondaryValue);
                selectedWorksite.SetTertiaryValue(tertiaryValue);
                SetText();

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
            suppliesLabel = (Label)scrollView.Query("suppliesLabel");
            detailsLabel = (Label)scrollView.Query("detailsLabel");

            snapToElement = scrollView.Query("snapTo");
            snaptoToggle = (Toggle)snapToElement.Query("snapToToggle");
            snaptoToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
            {
                constructionPlayer.snapToGrid = evt.newValue;
            });
            snaptoDropdown = (DropdownField)snapToElement.Query<DropdownField>("snapToDropdown");
            heightOffsetElement = scrollView.Query("heightOffset");
            heightOffsetDisplay = (Label)heightOffsetElement.Query("heightOffsetDisplay");
            heightOffsetSliderContainer = heightOffsetElement.Query("heightOffsetSliderContainer");
            heightOffsetInput = (TextField)heightOffsetElement.Query("heightOffsetInput");
            heightOffsetMinLabel = (Label)heightOffsetElement.Query("heightOffsetMin");
            heightOffsetMaxLabel = (Label)heightOffsetElement.Query("heightMax");
            heightElement = scrollView.Query("height");
            heightDisplay = (Label)heightElement.Query("heightDisplay");
            heightSliderContainer = heightElement.Query("heightSliderContainer");
            heightInput = (TextField)heightElement.Query("heightInput");
            heightMinLabel = (Label)heightElement.Query("heightMin");
            heightMaxLabel = (Label)heightElement.Query("heightMax");
            widthElement = scrollView.Query("width");
            widthDisplay = (Label)widthElement.Query("widthDisplay");
            widthSliderContainer = widthElement.Query("widthSliderContainer");
            widthInput = (TextField)widthElement.Query("widthInput");
            widthMinLabel = (Label)widthElement.Query("widthMin");
            widthMaxLabel = (Label)widthElement.Query("widthMax"); 
            lengthElement = scrollView.Query("length");
            lengthDisplay = (Label)lengthElement.Query("lengthDisplay");
            lengthSliderContainer = lengthElement.Query("lengthSliderContainer");
            lengthInput = (TextField)lengthElement.Query("lengthInput");
            lengthMinLabel = (Label)lengthElement.Query("lengthMin");
            lengthMaxLabel = (Label)lengthElement.Query("lengthMax");
            rotationElement = scrollView.Query("rotation");
            rotationInput = (TextField)rotationElement.Query("rotationInput");
            rotationMinLabel = (Label)rotationElement.Query("rotationMin");
            rotationMaxLabel = (Label)rotationElement.Query("rotationMax");
            secondaryElement = scrollView.Query("secondary");
            secondaryInput = (TextField)secondaryElement.Query("secondaryInput");
            secondaryMinLabel = (Label)secondaryElement.Query("secondaryMin");
            secondaryMaxLabel = (Label)secondaryElement.Query("secondaryMax");
            tertiaryElement = scrollView.Query("tertiary");
            tertiaryInput = (TextField)secondaryElement.Query("tertiaryInput");
            tertiaryMinLabel = (Label)secondaryElement.Query("tertiaryMin");
            tertiaryMaxLabel = (Label)secondaryElement.Query("tertiaryMax");

            //lSlider = (Slider)scrollView.ElementAt(2);// (Slider)container.Query<Slider>("lengthSlider").First();
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
            heightOffsetSlider = (Slider)scrollView.Query<Slider>("heightOffsetSlider").First();
            lSlider = (Slider)scrollView.Query<Slider>("lengthSlider").First();
            wSlider = (Slider)scrollView.Query<Slider>("widthSlider").First();
            hSlider = (Slider)scrollView.Query<Slider>("heightSlider").First();
            rSlider = (Slider)scrollView.Query<Slider>("rotationSlider").First();
            secondarySlider = (Slider)scrollView.Query<Slider>("secondarySlider").First();

            heightOffsetSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                heightOffset = evt.newValue;
                heightInput.SetValueWithoutNotify(evt.newValue.ToString());
            });
            lSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                length = evt.newValue;
                lengthInput.SetValueWithoutNotify(evt.newValue.ToString());
            });
            wSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                width = evt.newValue;
                widthInput.SetValueWithoutNotify(evt.newValue.ToString());
            });
            hSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                height = evt.newValue;
                heightInput.SetValueWithoutNotify(evt.newValue.ToString());
            });
            rSlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                rotation = evt.newValue;
                rotationInput.SetValueWithoutNotify(evt.newValue.ToString());
            });
            secondarySlider.RegisterCallback<ChangeEvent<float>>((evt) =>
            {
                secondaryValue = evt.newValue;
                secondaryInput.SetValueWithoutNotify(evt.newValue.ToString());
            });

            heightOffsetInput.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                float val = float.Parse(evt.newValue);
                val = val > selectedWorksite.maxHeightOffset ? selectedWorksite.maxHeightOffset : val < selectedWorksite.minHeightOffset ? selectedWorksite.minHeightOffset : val;
                heightOffsetInput.value = val.ToString();
                heightOffset = val;
                heightOffsetSlider.value = val;
            });
            lengthInput.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                float val = float.Parse(evt.newValue);
                Debug.Log(val);
                val = val > selectedWorksite.maxLength ? selectedWorksite.maxLength : val < selectedWorksite.minLength ? selectedWorksite.minLength : val;
                lengthInput.value = val.ToString();
                length = val;
                lSlider.value = val;
            });
            widthInput.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                float val = float.Parse(evt.newValue);
                val = val > selectedWorksite.maxWidth ? selectedWorksite.maxWidth : val < selectedWorksite.minWidth ? selectedWorksite.minWidth : val;
                widthInput.value = val.ToString();
                width = val;
                wSlider.value = val;
            });
            heightInput.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                float val = float.Parse(evt.newValue);
                val = val > selectedWorksite.maxHeight ? selectedWorksite.maxHeight : val < selectedWorksite.minHeight ? selectedWorksite.minHeight : val;
                heightInput.value = val.ToString();
                height = val;
                hSlider.value = val;
            });
            rotationInput.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                float val = float.Parse(evt.newValue);
                val = val > selectedWorksite.maxRotation ? selectedWorksite.maxRotation : val < selectedWorksite.minRotation ? selectedWorksite.minRotation : val;
                rotationInput.value = val.ToString();
                rotation = val;
                rSlider.value = val;
            });
            secondaryInput.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                float val = float.Parse(evt.newValue);
                val = val > selectedWorksite.maxSecondary ? selectedWorksite.maxSecondary : val < selectedWorksite.minSecondary ? selectedWorksite.minSecondary : val;
                secondaryInput.value = val.ToString();
                secondaryValue = val;
                secondarySlider.value = val;
            });
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
                heightOffset = selectedWorksite.heightOffset;
                rotation = selectedWorksite.rotation;
                secondaryValue = selectedWorksite.GetSecondaryValue();

                SetDisplay();
                if (selectedWorksite.disableHeight)
                {
                    heightInput.style.display = DisplayStyle.None;
                    heightSliderContainer.style.display = DisplayStyle.None;
                    heightDisplay.style.display = DisplayStyle.Flex;
                }
                else
                {
                    heightInput.style.display = DisplayStyle.Flex;
                    heightSliderContainer.style.display = DisplayStyle.Flex;
                    heightDisplay.style.display = DisplayStyle.None;
                }
                if (selectedWorksite.disableWidth)
                {
                    widthInput.style.display = DisplayStyle.None;
                    widthSliderContainer.style.display = DisplayStyle.None;
                    widthDisplay.style.display = DisplayStyle.Flex;
                }
                else
                {
                    widthInput.style.display = DisplayStyle.Flex;
                    widthSliderContainer.style.display = DisplayStyle.Flex;
                    widthDisplay.style.display = DisplayStyle.None;
                }
                if (selectedWorksite.disableLength)
                {
                    lengthInput.style.display = DisplayStyle.None;
                    lengthSliderContainer.style.display = DisplayStyle.None;
                    lengthDisplay.style.display = DisplayStyle.Flex;
                }
                else
                {
                    lengthInput.style.display = DisplayStyle.Flex;
                    lengthSliderContainer.style.display = DisplayStyle.Flex;
                    lengthDisplay.style.display = DisplayStyle.None;
                }
            }
        }
        public void SetDisplay()
        {
            SetLabels();
            SetCamView();
            SetSliders();
            SetText();
        }


        void SetLabels()
        {
            heightOffsetMinLabel.text = selectedWorksite.minHeightOffset.ToString();
            heightOffsetMaxLabel.text = selectedWorksite.maxHeightOffset.ToString();

            heightMinLabel.text = selectedWorksite.minHeight.ToString();
            heightMaxLabel.text = selectedWorksite.maxHeight.ToString();
            widthMinLabel.text = selectedWorksite.minWidth.ToString();
            widthMaxLabel.text = selectedWorksite.maxWidth.ToString();
            lengthMinLabel.text = selectedWorksite.minLength.ToString();
            lengthMaxLabel.text = selectedWorksite.maxLength.ToString();
            rotationMinLabel.text = selectedWorksite.minRotation.ToString();
            rotationMaxLabel.text = selectedWorksite.maxRotation.ToString();
            secondaryMinLabel.text = selectedWorksite.minSecondary.ToString();
            secondaryMaxLabel.text = selectedWorksite.maxSecondary.ToString();
            tertiaryMinLabel.text = selectedWorksite.minTertiary.ToString();
            tertiaryMaxLabel.text = selectedWorksite.maxTertiary.ToString();
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
            string suppliesString = "";
            foreach (SupplyCount sc in selectedWorksite.supplies)
            {
                suppliesString += sc.type.ToString() + " " + sc.countActual.ToString() + "/" + sc.countNeeded.ToString() + "\n";
            }
            suppliesString += "test";
            suppliesLabel.text = suppliesString;
            string detailsString = "5 labor hours\n";
            detailsString += "1.0 difficulty\n";
            detailsString += "50 kg\n";
            detailsString += "100 Durability\n";
            detailsLabel.text = detailsString;
        }

        void SetSliders()
        {
            heightOffsetSlider.lowValue = selectedWorksite.minHeightOffset;
            heightOffsetSlider.highValue = selectedWorksite.maxHeightOffset;
            heightOffsetSlider.value = selectedWorksite.heightOffset;
            lSlider.lowValue = selectedWorksite.minLength;
            lSlider.highValue = selectedWorksite.maxLength;
            lSlider.value = selectedWorksite.length;
            wSlider.lowValue = selectedWorksite.minWidth;
            wSlider.highValue = selectedWorksite.maxWidth;
            wSlider.value = selectedWorksite.width;
            hSlider.lowValue = selectedWorksite.minHeight;
            hSlider.highValue = selectedWorksite.maxHeight;
            hSlider.value = selectedWorksite.height;
            rSlider.lowValue = selectedWorksite.minRotation;
            rSlider.highValue = selectedWorksite.maxRotation;
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
            if (selectedWorksite.hasTertiary)
            {
                tertiarySlider.style.display = DisplayStyle.Flex;
                tertiarySlider.lowValue = selectedWorksite.minTertiary;
                tertiarySlider.highValue = selectedWorksite.maxTertiary;
                tertiarySlider.value = selectedWorksite.GetTertiaryValue();
            }
            else
            {
                tertiarySlider.style.display = DisplayStyle.None;
            }
        }

        public float AdjustHeightOffset(float adj)
        {
            heightOffset += adj;
            heightOffset = heightOffset > selectedWorksite.maxHeightOffset ? selectedWorksite.maxHeightOffset : heightOffset < selectedWorksite.minHeightOffset ? selectedWorksite.minHeightOffset : heightOffset;
            heightOffsetInput.value = heightOffset.ToString();
            heightOffsetSlider.value = heightOffset;
            return heightOffset;
        }
    }

}