using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public enum UI_MODE
    {
        CHARACTER,
        CONSTRUCTION_PLANNING,
        CONSTRUCTION,
        CRAFTING
    }

    public class GameUIControl : MonoBehaviour
    {
        public MalbersAnimations.MInputLink inputLink;
        public MalbersAnimations.MInputLinkManager inputLinkManager;

        [SerializeField]
        VisualTreeAsset inventoryItemTemplate;

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

        public ConstructionPlayer constructionPlayer;
        public CharacterUIControl characterUIControl;
        public ConstructionPlanningUIControl constructionPlanningUIControl;
        public ConstructionUIControl constructionUIControl;


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
        public UI_MODE mode = UI_MODE.CHARACTER;
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

        }

        public void ChangeMode(string modeName)
        {
            UI_MODE mode = (UI_MODE)System.Enum.Parse(typeof(UI_MODE), modeName);
            ChangeMode(mode);
        }

        private void ChangeMode(UI_MODE newMode)
        {
            if(mode == newMode)
            {
                return;
            }
            UI_MODE oldMode = mode;
            mode = newMode;
            if (menusOpen)
            {//Menu interface currently open, switch the open interface
                switch (oldMode)
                {
                    case UI_MODE.CRAFTING:
                        //craftingPanelControl.BuildAndPopulate();
                        break;
                    case UI_MODE.CHARACTER:
                        characterUIControl.Disable();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        constructionPlanningUIControl.Disable();
                        break;
                }
                switch (mode)
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
                }
            }
            else
            {
                switch (mode)
                {
                    case UI_MODE.CRAFTING:
                        inputLinkManager.SwitchActionMap(GAMEPLAY_ACTION_MAP);
                        //craftingPanelControl.BuildAndPopulate();
                        break;
                    case UI_MODE.CHARACTER:
                        inputLinkManager.SwitchActionMap(GAMEPLAY_ACTION_MAP);
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        constructionPlayer.StartPreview();
                        StaticsManager.Instance.ToggleConstructionHUD(true);
                        inputLinkManager.SwitchActionMap(CONSTRUCTION_ACTION_MAP);
                        break;
                }
            }
            //make changes that must occur whether or not Menus are open
            // -start/stop ConstructionPlayer
            // -change inputActionMap
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
            }
            switch (mode)
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
            }

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
            {//Menus already open, close it
                menusOpen = false;
                constructionUIControl.Disable();
                switch (mode)
                {
                    case UI_MODE.CRAFTING:
                        inputLinkManager.SwitchActionMap(GAMEPLAY_ACTION_MAP);
                        //craftingPanelControl.BuildAndPopulate();
                        break;
                    case UI_MODE.CHARACTER:
                        inputLinkManager.SwitchActionMap(GAMEPLAY_ACTION_MAP);
                        characterUIControl.Disable();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        inputLinkManager.SwitchActionMap(CONSTRUCTION_ACTION_MAP);
                        constructionPlanningUIControl.Disable();
                        break;
                }
            }
            else
            {//no Menus open, open the Menus matching the current mode
                menusOpen = true;
                switch (mode)
                {
                    case UI_MODE.CRAFTING:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        //craftingPanelControl.BuildAndPopulate();
                        break;
                    case UI_MODE.CHARACTER:
                        inputLinkManager.SwitchActionMap(UI_ACTION_MAP);
                        characterUIControl.Enable();
                        break;
                    case UI_MODE.CONSTRUCTION_PLANNING:
                        inputLinkManager.SwitchActionMap(CONSTRUCTION_UI_ACTION_MAP);
                        constructionPlanningUIControl.Enable();
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
    }

}