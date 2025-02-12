using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public enum UI_MODE
    {
        COMBAT,
        CHARACTER,
        CONSTRUCTION_PLANNING,
        CONSTRUCTION,
        CRAFTING,
        WORK
    }

    public class GameUIControl : MonoBehaviour
    {
        public UrthControl urthControl;
        public MalbersAnimations.MInputLink inputLink;
        public MalbersAnimations.MInputLinkManager inputLinkManager;

        [SerializeField]
        VisualTreeAsset inventoryItemTemplate;
        
        public UIDocument gameUiDocument;

        public Button newWorld;
        public Button loadWorld;
        public Button settings;
        public Button manual;
        public Button exit;
        public Button testWorld;

        //public VisualElement outer;
        //public VisualElement container;
        //public VisualElement menu;
        //public VisualElement inventoryPanel;
        //public VisualElement selectionPanel;
        //public VisualElement statsPanel;
        //public VisualElement characterPanel;

        public PlayerCreatureManager playerCreatureManager;
        public ConstructionPlayer constructionPlayer;
        public CharacterUIControl characterUIControl;
        public ConstructionPlanningUIControl constructionPlanningUIControl;
        public ConstructionUIControl constructionUIControl;

        public CraftingUIControl craftingUIControl;
        public WorkUIControl workUIControl;

        public CombatUIControl combatUIControl;

        public HUDControl hudControl;


        static string CHARACTER_UI = "character";
        static string CONSTRUCTION_UI = "construction";
        static string CONSTRUCTION_PLANNING_UI = "construction_planning";
        static string CRAFTING_UI = "crafting";
        static string CHARACTER_ACTION_MAP = "CharacterUI";
        static string GAMEPLAY_ACTION_MAP = "Gameplay";
        static string CONSTRUCTION_ACTION_MAP = "ConstructionPlanning";
        static string CRAFTING_ACTION_MAP = "Crafting";
        static string UI_ACTION_MAP = "UI";
        static string CONSTRUCTION_UI_ACTION_MAP = "ConstructionUI";
        
        Dictionary<UI_MODE, string> modeActionMapDicts = new Dictionary<UI_MODE, string>
        {
            { UI_MODE.CHARACTER, CHARACTER_ACTION_MAP },
            { UI_MODE.CONSTRUCTION_PLANNING, CONSTRUCTION_ACTION_MAP },
            { UI_MODE.CRAFTING, CRAFTING_ACTION_MAP },
        };

        public bool menusOpen = false;
        public UI_MODE targetMode = UI_MODE.CHARACTER;
        public UI_MODE actualMode = UI_MODE.CHARACTER;
        Dictionary<UI_MODE, bool> uisOpenedDict = new Dictionary<UI_MODE, bool>()
        {
            { UI_MODE.CHARACTER,false },
            { UI_MODE.CONSTRUCTION_PLANNING,false },
            { UI_MODE.CRAFTING,false },
        };
        Dictionary<UI_MODE, UIDocument> uisDict;
        Dictionary<string, bool> menusOpenedDict = new Dictionary<string, bool>()
        {
            { CHARACTER_UI,false },
            { CONSTRUCTION_UI,false },
            { CONSTRUCTION_PLANNING_UI,false }
        };
        Dictionary<string, UIDocument> docsDict;

        public static GameUIControl Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            //inventoryPanel = characterUI.rootVisualElement.Query("inventoryPanel").First();
            //selectionPanel = characterUI.rootVisualElement.Query("displayPanel").First();
            //statsPanel = characterUI.rootVisualElement.Query("statsPanel").First();
            //characterPanel = characterUI.rootVisualElement.Query("characterPanel").First();
        }


        void Start()
        {
            //docsDict = new Dictionary<string, UIDocument>()
            //{
            //    { CHARACTER_UI,characterUI },
            //    { CONSTRUCTION_UI,constructionUI }
            //};
            //uisDict = new Dictionary<UI_MODE, UIDocument>()
            //{
            //    { UI_MODE.CHARACTER,characterUI },
            //    { UI_MODE.CONSTRUCTION,constructionUI },
            //    { UI_MODE.CRAFTING,craftingUI }
            //};
        }

        void Update()
        {
            if (playerCreatureManager is not null)
            {
                if (playerCreatureManager.combatMode && actualMode != UI_MODE.COMBAT)
                {
                    OpenCombatUI();
                }
                else if (actualMode == UI_MODE.COMBAT && !playerCreatureManager.combatMode)
                {
                    CloseCombatUI();
                }
            }
        }

        void EnableCombatMode()
        {
            if (actualMode == UI_MODE.COMBAT)
            {
                return;
            }
            targetMode = actualMode;
            switch (targetMode)
            {
                case UI_MODE.CRAFTING:
                    craftingUIControl.Disable();
                    break;
                case UI_MODE.CHARACTER:
                    characterUIControl.Disable();
                    break;
                case UI_MODE.CONSTRUCTION:
                    constructionUIControl.Disable();
                    break;
                case UI_MODE.WORK:
                    workUIControl.Disable();
                    break;
            }
            combatUIControl.Enable();
            Debug.Log("GameUIDocument Order is now...");
            foreach (VisualElement child in gameUiDocument.rootVisualElement.Children())
            {
                Debug.Log(child.name);
            }
            actualMode = UI_MODE.COMBAT;
            if (menusOpen)
            {
                combatUIControl.EnableMenus();
            }
        }

        void OpenCombatUI()
        {
           
        }

        void CloseCombatUI()
        { 
            combatUIControl.Disable();
            ChangeMode(targetMode);
        }


        public void ChangeMode(string modeName)
        {
            Debug.Log("ChangeMode" + modeName);
            UI_MODE inputMode = (UI_MODE)System.Enum.Parse(typeof(UI_MODE), modeName);
            ChangeMode(inputMode);
        }

        private void ChangeMode(UI_MODE newMode)
        {
            if(actualMode == newMode)
            {
                Debug.Log("already in requested mode");
                return;
            }
            UI_MODE oldMode = actualMode;
            targetMode = newMode;
            actualMode = newMode;
            if (menusOpen)
            {//Menu interface currently open, switch the open interface
                Debug.Log("menus are open, swappping out old for new");
                switch (oldMode)
                {
                    case UI_MODE.COMBAT:
                        combatUIControl.DisableMenus();
                        break;
                    case UI_MODE.CRAFTING:
                        craftingUIControl.DisableMenus();
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.DisableMenus();
                        break;
                    case UI_MODE.CONSTRUCTION:
                        constructionPlanningUIControl.DisableMenus();
                        break;
                    case UI_MODE.WORK:
                        workUIControl.DisableMenus();
                        break;
                }
                switch (targetMode)
                {
                    case UI_MODE.COMBAT:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        combatUIControl.EnableMenus();
                        break;
                    case UI_MODE.CRAFTING:
                        //craftingPanelControl.BuildAndPopulate();
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        craftingUIControl.EnableMenus();
                        break;
                    case UI_MODE.CHARACTER:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        characterUIControl.EnableMenus();
                        break;
                    case UI_MODE.CONSTRUCTION:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        constructionUIControl.EnableMenus();
                        break;
                    case UI_MODE.WORK:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        workUIControl.EnableMenus();
                        break;
                }
            }

            //make changes that must occur whether or not Menus are open
            //-set character CreatureMode
            //-start/stop ConstructionPlayer
            // -change inputActionMap (actually maybe won't do this, instead keep controlls mostly the same and handle the few exceptions in the functions bound to inputs with switch statements
            //
            switch (oldMode)
            {
                case UI_MODE.COMBAT:
                    playerCreatureManager.ExitCombatMode();
                    combatUIControl.Disable();
                    break;
                case UI_MODE.CRAFTING:
                    craftingUIControl.Disable();
                    //craftingPanelControl.BuildAndPopulate();
                    break;
                case UI_MODE.CHARACTER:
                    characterUIControl.Disable();
                    break;
                case UI_MODE.CONSTRUCTION:
                    constructionUIControl.Disable();
                    constructionPlanningUIControl.Disable();
                    ConstructionUIControl.Instance.placing = false;
                    StaticsManager.Instance.ToggleConstructionHUD(false);
                    break;
                case UI_MODE.WORK:
                    workUIControl.Disable();
                    hudControl.DisableWorkHUD();
                    //stop workPreviewIndicator
                    break;
            }
            switch (targetMode)
            {
                case UI_MODE.CRAFTING:
                    craftingUIControl.Enable();
                    //craftingPanelControl.BuildAndPopulate();
                    break;
                case UI_MODE.CHARACTER:
                    characterUIControl.Enable();
                    break;
                case UI_MODE.CONSTRUCTION:
                    constructionUIControl.Enable();
                    constructionPlanningUIControl.Enable();
                    //constructionPlayer.StartPreview();
                    StaticsManager.Instance.ToggleConstructionHUD(true);
                    break;
                case UI_MODE.WORK:
                    workUIControl.Enable();
                    playerCreatureManager.ChangeMode(CREATURE_MODE.WORK);
                    hudControl.EnableWorkHUD();
                    //start workPreviewIndicator
                    break;
            }
            Debug.Log("GameUIDocument Order is now...");
            foreach (VisualElement child in gameUiDocument.rootVisualElement.Children())
            {
                Debug.Log(child.name);
            }
        }

        void DisableCurrentUI()
        {

        }
        public void Initialize()
        {
            ChangeMode(UI_MODE.CHARACTER);
        }


        public void OpenConstructionItemMenu(StaticPrefab staticPrefab)
        {
            constructionUIControl.SetItemMenu(staticPrefab);
        }

        public void SetConstructionItemMenu(StaticPrefab staticPrefab)
        {
            constructionUIControl.SetItemMenu(staticPrefab);
        }

        public void ToggleMenus()
        {
            if (menusOpen)
            {//Menus currently open, close them
                inputLinkManager.SwitchActionMap(GAMEPLAY_ACTION_MAP);
                menusOpen = false;
                switch (actualMode)
                {
                    case UI_MODE.COMBAT:
                        combatUIControl.DisableMenus();
                        break;
                    case UI_MODE.WORK:
                        workUIControl.DisableMenus();
                        break;
                    case UI_MODE.CRAFTING:
                        craftingUIControl.DisableMenus();
                        //craftingPanelControl.BuildAndPopulate();
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.DisableMenus();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        //constructionUIControl.Disable();
                        constructionPlanningUIControl.DisableMenus();
                        break;
                }
            }
            else
            {//no Menus open, open the Menus matching the current mode
                inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                menusOpen = true;
                switch (actualMode)
                {
                    case UI_MODE.COMBAT:
                        combatUIControl.EnableMenus();
                        break;
                    case UI_MODE.WORK:
                        workUIControl.EnableMenus();
                        break;
                    case UI_MODE.CRAFTING:
                        craftingUIControl.EnableMenus();
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.EnableMenus();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        //constructionUIControl.Enable();
                        constructionPlanningUIControl.EnableMenus();
                        break;
                }
            }
        }

        public void ScrollUp(float inp)
        {
            Debug.Log("Up");
            Debug.Log(inp);
        }
        public void ScrollDown(float inp)
        {
            Debug.Log("Down");
            Debug.Log(inp);
        }

        public void CallKeybind(int slot)
        {
            /*if weapon
             *  if preferred hand open set as preferred hand weapon
             *  else if other hand open set as other hand weapon
             *  else set as preferred hand weapon
             *  
             *  if guard drawn
             *      if weapon already in chosen hand, drop it
             *      draw weapon
             *
             *if action
             *  set as action
             */
            //PlayerPreferences.Instance.keybinds[slot]
        }

        public void InteractMenu()
        {
            Debug.Log("InteractMenu");
        }

        public void Activate()
        {
            gameUiDocument.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameUiDocument.gameObject.SetActive(false);
        }

        public void OpenSkipTimeMenu()
        {
            Debug.Log("TODO SkipTime menu");
        }

        public void TogglePauseTime()
        {
            Debug.Log("TODO PauseTime");
        }

        public void ToggleEscapeMenu()
        {
            Debug.Log("TODO EscapeMenu");
        }
    }

}