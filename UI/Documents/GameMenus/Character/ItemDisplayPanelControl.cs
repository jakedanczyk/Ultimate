using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    /*Interface panel to display an inventory as list of items
     * There are a number of columns of data, columns are sortable
     * There is a search box
     * There are catergory filters thats can be toggled on and off
     */
    public class ItemDisplayPanelControl : UIPanelControl
    {
        public VisualElement displayPanel;
        public VisualElement content;
        public Label titleLabel;
        public VisualElement controls;
        public Label useLabel;
        public Label dropLabel;
        public Label transferLabel;
        public VisualElement renderElement;

        public UMA.CharacterSystem.DynamicCharacterAvatar umaAvatar;
        public PlayerCreatureManager playerCreatureManager;
        public CreatureInventory playerInventory;
        public UItemData selectedItem;

        public Transform itemAnchor;

        public static ItemDisplayPanelControl Instance { get; private set; }
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
            playerInventory = playerCreatureManager.body.creatureInventory;
        }

        public void Link(VisualElement root)
        {
            rootElement = root;
            RegisterBorderCallbacks();
            displayPanel = rootElement;
            displayPanel.RegisterCallback<ClickEvent>(OnItemClick);
            displayPanel.Children().First().RegisterCallback<ClickEvent>(OnItemClick);
            content = displayPanel.Children().First().Query("content").First();
            content.RegisterCallback<ClickEvent>(OnItemClick);
            titleLabel = (Label)titleBar.Children().First();
            renderElement = content.Query("render").First();
            renderElement.RegisterCallback<WheelEvent>(OnWheel);
            renderElement.RegisterCallback<ClickEvent>(OnItemClick);

            controls = content.Query("controls").First();
            useLabel = (Label)controls.Query("useLabel").First();
            useLabel.text = UIManager.Instance.useItemKey;
            dropLabel = (Label)controls.Query("dropLabel").First();
            dropLabel.text = UIManager.Instance.dropItemKey;
            transferLabel = (Label)controls.Query("transferLabel").First();
            transferLabel.text = UIManager.Instance.transferItemKey;
        }



        public void OnItemClick(ClickEvent evt)
        {
            Debug.Log(evt);
        }


        public void OnWheel(WheelEvent evt)
        {
            Debug.Log(evt.delta);
            // Only perform this action at the target, not in a parent
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            Debug.Log(evt.delta);

            float z = itemAnchor.localPosition.z + evt.delta.y;
            z = Math.Clamp(z, .01f, 100f);

            itemAnchor.localPosition = new Vector3(itemAnchor.localPosition.x, itemAnchor.localPosition.y, z);
        }


        public void SetDisplayItem(UItemData data)
        {
            selectedItem = data;
            titleLabel.text = selectedItem.GetName();
            GameObject camObject = Instantiate(ItemsLibrary.Instance.prefabsDict[data.type].componentPrefab);
            camObject.transform.parent = itemAnchor;
            camObject.transform.localPosition = Vector3.zero;
        }

        /*Use selected item
        */
        public void OnUseSelected()
        {
            Debug.Log("OnUseSelected");
            if (selectedItem != null)
            {
                if (ItemsLibrary.Instance.templatesDict[selectedItem.type].tags.Contains(ITEM_TAG.WEARABLE))
                {
                    UrthResponse response = playerInventory.EquipItem(selectedItem);
                    if (response.msg != null)
                    {
                        MessageLogControl.Instance.NewMessage(response.msg);
                    }
                    if (response.success)
                    {
                        UMA.UMATextRecipe textRecp = ItemsLibrary.Instance.prefabsDict[selectedItem.type].textRecip;
                        umaAvatar.SetSlot(textRecp);
                        umaAvatar.BuildCharacter();
                        //TODO update character model clothing
                    }
                }
                else if (ItemsLibrary.Instance.templatesDict[selectedItem.type].tags.Contains(ITEM_TAG.WIELDABLE))
                {
                    bool preferLeft = PlayerPreferences.Instance.offhandWeapons.Contains(selectedItem.type);
                    if(preferLeft && playerInventory.leftItem == null && !playerInventory.carryingLeft)
                    {
                        Debug.Log("equip left");
                        UrthResponse response = playerInventory.EquipLeft(selectedItem);
                        if (response.msg != null)
                        {
                            MessageLogControl.Instance.NewMessage(response.msg);
                        }
                        if (response.success)
                        {
                            //TODO update character model clothing
                        }
                    }
                    else if (playerInventory.rightItem == null && !playerInventory.carryingRight)
                    {
                        Debug.Log("equip right");
                        UrthResponse response = playerInventory.EquipRight(selectedItem);
                        if (response.msg != null)
                        {
                            MessageLogControl.Instance.NewMessage(response.msg);
                        }
                        if (response.success)
                        {
                            //TODO update character model clothing
                        }
                    }
                    else if (playerInventory.leftItem == null && !playerInventory.carryingLeft)
                    {
                        Debug.Log("equip left");
                        UrthResponse response = playerInventory.EquipLeft(selectedItem);
                        if (response.msg != null)
                        {
                            MessageLogControl.Instance.NewMessage(response.msg);
                        }
                        if (response.success)
                        {
                            //TODO update character model clothing
                        }
                    }
                }
            }
        }
        /*Drop selected item as loose item
        */
        public void OnDropSelected()
        {
            Debug.Log("OnDropSelected");
            if (selectedItem != null)
            {

            }
        }
        /*Transfer selected item to/from an inventory other than the player character inventory
        */
        public void OnTransferSelected()
        {
            Debug.Log("OnTransferSelected");
            if (selectedItem != null)
            {

            }
        }
    }
}