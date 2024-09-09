using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class ConstructionPlanningUIControl : MonoBehaviour
    {
        void Start()
        {
            doc = this.GetComponent<UIDocument>();
        }

        void Update()
        {

        }

        public ConstructionPlayer constructionPlayer;
        public ConstructionItemsPanelControl constructionItemsPanelControl;
        public ConstructionSettingsPanelControl constructionSettingsPanelControl;
        public UIDocument doc;
        public VisualElement constructionPlanningInterface;
        bool uiBuilt = false;

        public void Enable()
        {
            if (!uiBuilt)
            {
                Initialize();
            }
            else
            {
                constructionPlanningInterface.style.display = DisplayStyle.Flex;
            }
        }
        public void Disable()
        {
            constructionPlanningInterface.style.display = DisplayStyle.None;
        }
        public void Initialize()
        {
            constructionPlanningInterface = doc.rootVisualElement.Query(UrthConstants.CONSTRUCTION_PLANNING_INTERFACE).First();
            constructionPlanningInterface.style.display = DisplayStyle.Flex;

            VisualElement itemsPanel = constructionPlanningInterface.Query(UrthConstants.CONSTRUCTION_ITEMS_PANEL).First();
            constructionItemsPanelControl.Link(itemsPanel);
            constructionItemsPanelControl.Populate();

            VisualElement settingsPanel = constructionPlanningInterface.Query(UrthConstants.CONSTRUCTION_SETTINGS_PANEL).First();
            constructionSettingsPanelControl.Link(settingsPanel);
            //constructionOptionsControl.Populate();

            uiBuilt = true;
        }
    }

}