using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class ConstructionUIControl : MonoBehaviour
    {
        void Start()
        {
            doc = this.GetComponent<UIDocument>();
        }

        void Update()
        {

        }

        public ConstructionPlayer constructionPlayer;
        public ConstructionPanelControl constructionPanelControl;
        public UIDocument doc;
        public VisualElement constructionInterface;
        bool uiBuilt = false;

        public void Enable(StaticPrefab staticPrefab)
        {
            if (!uiBuilt)
            {
                Debug.Log("building construction UI");

                constructionInterface = doc.rootVisualElement.Query(UrthConstants.CONSTRUCTION_INTERFACE).First();
                constructionInterface.style.display = DisplayStyle.Flex;

                VisualElement constructionPanel = constructionInterface.Query(UrthConstants.CONSTRUCTION_PANEL).First();
                constructionPanelControl.Link(constructionPanel);

                uiBuilt = true;
            }
            else
            {
                Debug.Log("construction UI already built");
                constructionInterface.style.display = DisplayStyle.Flex;
            }
            constructionPanelControl.SetItem(staticPrefab);
        }
        public void Disable()
        {
            if (uiBuilt)
            {
                constructionInterface.style.display = DisplayStyle.None;
            }
        }
    }

}