using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    public class CharacterUIControl : MonoBehaviour
    {
        void Start()
        {
            doc = this.GetComponent<UIDocument>();
        }

        void Update()
        {

        }

        public InventoryPanelControl inventoryPanelControl;
        public ItemDisplayPanelControl itemDisplayPanelControl;
        public UIDocument doc;
        public VisualElement characterInterface;
        bool built = false;

        public void Enable()
        {
            if (!built)
            {
                characterInterface = doc.rootVisualElement.Query(UrthConstants.CHARACTER_INTERFACE).First();
                VisualElement inventoryPanel = characterInterface.Query(UrthConstants.INVENTORY_PANEL).First();
                inventoryPanelControl.Link(inventoryPanel);

                VisualElement itemDisplayPanel = characterInterface.Query(UrthConstants.ITEM_DISPLAY_PANEL).First();
                itemDisplayPanelControl.Link(itemDisplayPanel);
                inventoryPanelControl.Populate();
                built = true;
            }
            characterInterface.style.display = DisplayStyle.Flex;
        }
        public void Disable()
        {
            characterInterface.style.display = DisplayStyle.None;
        }
    }

}