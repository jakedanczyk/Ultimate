using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Urth
{

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
    }

}