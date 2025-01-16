using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class ConstructionUIControl : MonoBehaviour
    {

        public static ConstructionUIControl Instance { get; private set; }
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        void Start()
        {
            doc = this.GetComponent<UIDocument>();
        }

        void Update()
        {

        }

        public bool placing;
        public ConstructionPlayer constructionPlayer;
        public ConstructionProgressMenuControl constructionProgressMenuControl;
        public UIDocument doc;
        public VisualElement constructionInterface;
        public VisualElement constructionPlanningMenu;
        public VisualElement constructionProgressMenu;
        bool uiBuilt = false;
        public void Build()
        {
            Debug.Log("building construction UI");

            constructionInterface = doc.rootVisualElement.Query(UrthConstants.CONSTRUCTION_INTERFACE).First();

            constructionPlanningMenu = constructionInterface.Query(UrthConstants.CONSTRUCTION_PLANNING_MENU).First();

            constructionProgressMenu = constructionInterface.Query(UrthConstants.CONSTRUCTION_PANEL).First();
            constructionProgressMenuControl.Link(constructionProgressMenu);

            uiBuilt = true;
        }

        public void Enable()
        {
            if (!uiBuilt)
            {
                Build();
            }
            else
            {
                Debug.Log("construction UI already built");
            }
            constructionInterface.BringToFront();
        }
        public void Disable()
        {
            constructionPlayer.StopPreview();
            DisableMenus();
            if (uiBuilt)
            {
                constructionInterface.style.display = DisplayStyle.None;
            }
        }

        public void SetItemMenu(StaticPrefab staticPrefab)
        {
            constructionProgressMenuControl.SetConstruction(staticPrefab);
        }

        public void EnableMenus()
        {
            if (!uiBuilt)
            {
                Build();
            }
            constructionInterface.style.display = DisplayStyle.Flex;
            constructionPlanningMenu.style.display = DisplayStyle.Flex;
        }
        public void DisableMenus()
        {
            if (uiBuilt)
            {
                constructionPlanningMenu.style.display = DisplayStyle.None;
                constructionProgressMenu.style.display = DisplayStyle.None;
            }
        }

        public void OpenItemMenu()
        {
            if (!uiBuilt)
            {
                Build();
            }
            constructionProgressMenu.style.display = DisplayStyle.Flex;
        }
    }

}