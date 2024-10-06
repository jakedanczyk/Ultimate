using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Urth
{
    public enum UI_GRAB
    {
        TITLE,
        LEFT,RIGHT,TOP,BOTTOM,
        TOP_LEFT,TOP_RIGHT,BOTTOM_LEFT,BOTTOM_RIGHT
    }

    public class UIManager : MonoBehaviour
    {
        public GameUIControl gameUIControl;
        public MainMenu mainMenu;
        public GameMasterPanelControl gameMasterPanel;
        public CharacterCreationMenuControl characterCreationMenu;
        public InventoryPanelControl inventoryPanel;

        public InputActionAsset input;

        public string useItemKey;
        public string dropItemKey;
        public string transferItemKey;

        public GameObject voxelWorksitePrefab;

        public UIDocument gameUiDocument;
        public bool globalLock;
        public UIPanelControl activePanel;

        public static UIManager Instance { get; private set; }
        public void Awake()
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

        // Start is called before the first frame update
        void Start()
        {
            gameUiDocument.rootVisualElement.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (globalLock)
                {
                    globalLock = false;
                    activePanel.ReleaseGrab();
                }
                Debug.Log("mouse up");
            });
            useItemKey = input.FindActionMap(UrthConstants.UI_ACTION_MAP).FindAction("UseSelected").GetBindingDisplayString(0);
            dropItemKey = input.FindActionMap(UrthConstants.UI_ACTION_MAP).FindAction("DropSelected").GetBindingDisplayString(0);
            transferItemKey = input.FindActionMap(UrthConstants.UI_ACTION_MAP).FindAction("TransferSelected").GetBindingDisplayString(0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initialize()
        {
            characterCreationMenu.Initialize();
            gameUIControl.Initialize();
        }

        public bool ClaimGlobalLock(UIPanelControl uIPanelControl)
        {
            if (globalLock) { return false; }
            else
            {
                globalLock = true;
                activePanel = uIPanelControl;
                return true;
            }
        }
    }

}