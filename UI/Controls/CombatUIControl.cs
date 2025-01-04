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

        void Awake()
        {
            uiDocument = this.GetComponent<UIDocument>();
        }

        private void Start()
        {
            Build();
        }

        void Update()
        {

        }
        public void Enable()
        {
            if (!uiBuilt)
            {
                Build();
            }
            combatInterface.BringToFront();
            combatInterface.style.display = DisplayStyle.Flex;
        }
        public void Disable()
        {
            if (uiBuilt)
            {
                combatInterface.style.display = DisplayStyle.None;
            }
        }
        public void EnableMenus()
        {
            if (!uiBuilt)
            {
                Build();
            }
            combatInterface.style.display = DisplayStyle.Flex;
        }
        public void DisableMenus()
        {
            if (uiBuilt)
            {
                combatInterface.style.display = DisplayStyle.None;
            }
        }

        public void Build()
        {
            Debug.Log("building combat UI");

            combatInterface = uiDocument.rootVisualElement.Query(UrthConstants.COMBAT_INTERFACE).First();
            //combatInterface.style.display = DisplayStyle.Flex;

            uiBuilt = true;
        }
    }

}