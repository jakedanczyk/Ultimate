using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Urth
{
    public enum URTH_DOCUMENT
    {
        MAIN_MENU,
        PAUSE_MENU,
        GAME_INTERFACE
    }

    public enum UI_GRAB
    {
        TITLE,
        LEFT,RIGHT,TOP,BOTTOM,
        TOP_LEFT,TOP_RIGHT,BOTTOM_LEFT,BOTTOM_RIGHT
    }

    public class UIManager : MonoBehaviour
    {
        public GameUIControl gameUIControl;
        public MainMenuControl mainMenu;
        public PauseMenuControl pauseMenu;
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

        //
        /// <summary>
        /// Sets which 'UIDocument' (a Unity.UIElements class) is active.
        /// 
        /// </summary>
        /// <param name="document"></param>
        public void SetActiveDocument(URTH_DOCUMENT document)
        {
            switch (document)
            {
                case URTH_DOCUMENT.MAIN_MENU:
                    mainMenu.Activate();
                    pauseMenu.Deactivate();
                    gameUIControl.Deactivate();
                    break;
                case URTH_DOCUMENT.PAUSE_MENU:
                    //pauseMenu.Activate();
                    mainMenu.Deactivate();
                    gameUIControl.Deactivate();
                    break;
                case URTH_DOCUMENT.GAME_INTERFACE:
                    gameUIControl.Activate();
                    mainMenu.Deactivate();
                    //pauseMenu.Deactive();
                    break;
            }
        }

        public void OpenGameUI()
        {
            gameUIControl.enabled = true;
            SetActiveDocument(URTH_DOCUMENT.GAME_INTERFACE);
            gameUIControl.Activate();
        }

        public void OpenMainMenu()
        {
            //set active
            SetActiveDocument(URTH_DOCUMENT.MAIN_MENU);
            //set visible
            mainMenu.SetVisible();
        }

        public void OpenPauseMenu()
        {
            SetActiveDocument(URTH_DOCUMENT.PAUSE_MENU);
            pauseMenu.SetVisible();
        }



    }

}