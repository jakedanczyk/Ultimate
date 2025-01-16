using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    public class CharacterUIControl : MonoBehaviour
    {
        void Awake()
        {
            doc = this.GetComponent<UIDocument>();
        }

        void Start()
        {
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
                Build();
            }
            else
            {
                inventoryPanelControl.Reorder();
            }
            characterInterface.BringToFront();
            characterInterface.style.display = DisplayStyle.Flex;
        }
        public void Disable()
        {
            DisableMenus();
        }
        public void EnableMenus()
        {
            if (!built)
            {
                Build();
            }
            else
            {
                inventoryPanelControl.Reorder();
            }

            characterInterface.style.display = DisplayStyle.Flex;
        }
        public void DisableMenus()
        {
            if (built)
            {
                characterInterface.style.display = DisplayStyle.None;
            }
        }

        public void Build()
        {
            characterInterface = doc.rootVisualElement.Query(UrthConstants.CHARACTER_INTERFACE).First();
            VisualElement inventoryPanel = characterInterface.Query(UrthConstants.INVENTORY_PANEL).First();
            inventoryPanelControl.Link(inventoryPanel);
            inventoryPanelControl.Populate();

            VisualElement itemDisplayPanel = characterInterface.Query(UrthConstants.ITEM_DISPLAY_PANEL).First();
            itemDisplayPanelControl.Link(itemDisplayPanel);
            itemDisplayPanelControl.enabled = true;
            built = true;
            Debug.Log("Built CharUI");
        }
    }

}