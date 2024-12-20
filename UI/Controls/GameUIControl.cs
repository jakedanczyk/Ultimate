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

        bool menusOpen;
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

        void OpenCombatUI()
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
                case UI_MODE.CONSTRUCTION_PLANNING:
                    constructionPlanningUIControl.Disable();
                    break;
                case UI_MODE.WORK:
                    workUIControl.Disable();
                    break;
            }
            actualMode = UI_MODE.COMBAT;
            combatUIControl.Enable();
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
                return;
            }
            UI_MODE oldMode = actualMode;
            targetMode = newMode;
            if (menusOpen)
            {//Menu interface currently open, switch the open interface
                switch (oldMode)
                {
                    case UI_MODE.CRAFTING:
                        craftingUIControl.Disable();
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.Disable();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        constructionPlanningUIControl.Disable();
                        break;
                    case UI_MODE.WORK:
                        workUIControl.Disable();
                        break;
                }
                switch (targetMode)
                {
                    case UI_MODE.CRAFTING:
                        //craftingPanelControl.BuildAndPopulate();
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.Enable();
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        constructionPlanningUIControl.Enable();
                        inputLinkManager.SwitchActionMap(CONSTRUCTION_UI_ACTION_MAP);
                        break;
                    case UI_MODE.WORK:
                        workUIControl.Enable();
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
                case UI_MODE.CRAFTING:
                    //craftingPanelControl.BuildAndPopulate();
                    break;
                case UI_MODE.CHARACTER:
                    break;
                case UI_MODE.CONSTRUCTION_PLANNING:
                    constructionPlayer.StopPreview();
                    StaticsManager.Instance.ToggleConstructionHUD(false);
                    break;
                case UI_MODE.WORK:
                    //stop workPreviewIndicator
                    break;
            }
            switch (targetMode)
            {
                case UI_MODE.CRAFTING:
                    //craftingPanelControl.BuildAndPopulate();
                    break;
                case UI_MODE.CHARACTER:
                    break;
                case UI_MODE.CONSTRUCTION_PLANNING:
                    constructionPlayer.StartPreview();
                    StaticsManager.Instance.ToggleConstructionHUD(true);
                    break;
                case UI_MODE.WORK:
                    workUIControl.Enable();
                    //start workPreviewIndicator
                    break;
            }
        }

        void DisableCurrentUI()
        {

        }
        public void Initialize()
        {

        }

        public void OpenConstructionMenu(StaticPrefab staticPrefab)
        {
            constructionUIControl.Enable(staticPrefab);
        }
        public void CloseConstructionMenu()
        {
            constructionUIControl.Disable();
        }

        public void ToggleMenus()
        {
            if (menusOpen)
            {//Menus currently open, close them
                inputLinkManager.SwitchActionMap(GAMEPLAY_ACTION_MAP);
                menusOpen = false;
                switch (actualMode)
                {
                    case UI_MODE.WORK:
                        workUIControl.Disable();
                        break;
                    case UI_MODE.CRAFTING:
                        //craftingPanelControl.BuildAndPopulate();
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.Disable();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        //constructionUIControl.Disable();
                        constructionPlanningUIControl.Disable();
                        break;
                }
            }
            else
            {//no Menus open, open the Menus matching the current mode
                inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                menusOpen = true;
                switch (actualMode)
                {
                    case UI_MODE.WORK:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        workUIControl.Enable();
                        break;
                    case UI_MODE.CRAFTING:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        craftingUIControl.Enable();
                        break;
                    case UI_MODE.CHARACTER:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        characterUIControl.Enable();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        //constructionUIControl.Enable();
                        constructionPlanningUIControl.Enable();
                        inputLinkManager.SwitchActionMap(CONSTRUCTION_UI_ACTION_MAP);
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