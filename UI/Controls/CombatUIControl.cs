using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class CombatUIControl : MonoBehaviour
    {
        public UIDocument uiDocument;
        public VisualElement combatInterface;
        bool uiBuilt = false;

        void Start()
        {
            uiDocument = this.GetComponent<UIDocument>();
        }

        void Update()
        {

        }

        public void Enable()
        {
            if (!uiBuilt)
            {
                Debug.Log("building combat UI");

                combatInterface = uiDocument.rootVisualElement.Query(UrthConstants.COMBAT_INTERFACE).First();
                combatInterface.style.display = DisplayStyle.Flex;

                VisualElement combatPanel = combatInterface.Query(UrthConstants.CONSTRUCTION_PANEL).First();
                uiBuilt = true;
            }
            else
            {
                Debug.Log("combat UI already built");
                combatInterface.style.display = DisplayStyle.Flex;
            }
        }
        public void Disable()
        {
            if (uiBuilt)
            {
                combatInterface.style.display = DisplayStyle.None;
            }
        }
    }

}