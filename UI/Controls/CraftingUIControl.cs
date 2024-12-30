using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Urth
{
    public class CraftingUIControl : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public UIDocument doc;
        public VisualElement craftingInterface;
        bool uiBuilt = false;

        public ListView listView;

        public void Disable()
        {
            craftingInterface.style.display = DisplayStyle.None;
        }

        public void Enable()
        {
            if (!uiBuilt)
            {
                Initialize();
            }
            else
            {
                craftingInterface.style.display = DisplayStyle.Flex;
            }
        }
        public void DisableMenus()
        {
            craftingInterface.style.display = DisplayStyle.None;
        }

        public void EnableMenus()
        {
            if (!uiBuilt)
            {
                Initialize();
            }
            else
            {
                craftingInterface.style.display = DisplayStyle.Flex;
            }
        }

        public void Initialize()
        {
            craftingInterface = doc.rootVisualElement.Query(UrthConstants.CONSTRUCTION_PLANNING_INTERFACE).First();
            craftingInterface.style.display = DisplayStyle.Flex;

            uiBuilt = true;
        }
    }
}