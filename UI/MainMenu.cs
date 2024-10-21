using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class MainMenu : MonoBehaviour
    {
        public GameManager gameManager;
        public UltimateTerrains.UltimateTerrain ultimateTerrain;
        public CreaturesLibrary creatureLibrary;
        public GameObject playerCharacter;
        public PhysicsBasedCharacterController.CharacterManager characterManager;
        public GameObject flyCamera;
        public UIDocument mainMenu;

        public Button btnNewWorld;
        public Button btnLoadWorld;
        public Button btnSettings;
        public Button btnManual;
        public Button btnExit;
        public Button btnTestWorld;

        public VisualElement buttonsContainer;
        public VisualElement container;
        public VisualElement menu;

        public static MainMenu Instance { get; private set; }
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

            mainMenu = this.GetComponent<UIDocument>();
            buttonsContainer = mainMenu.rootVisualElement.Query("menuOptions").First();
            //container = mainMenu.rootVisualElement.Query("container").First();
            //menu = mainMenu.rootVisualElement.Query("menu").First();

            btnNewWorld = (Button)buttonsContainer.Query("new").First();
            btnNewWorld.RegisterCallback<ClickEvent>(OnNewWorld);

            btnLoadWorld = (Button)buttonsContainer.Query("load").First();
            btnLoadWorld.RegisterCallback<ClickEvent>(OnLoad);

            btnSettings = (Button)buttonsContainer.Query("settings").First();
            btnSettings.RegisterCallback<ClickEvent>(OnSettings);

            btnManual = (Button)buttonsContainer.Query("manual").First();
            btnManual.RegisterCallback<ClickEvent>(OnManual);

            btnExit = (Button)buttonsContainer.Query("exit").First();
            btnExit.RegisterCallback<ClickEvent>(OnExit);

            btnTestWorld = (Button)buttonsContainer.Query("testWorld").First();
            btnTestWorld.RegisterCallback<ClickEvent>(OnTestWorld);
        }


        public void OnNewWorld(ClickEvent e)
        {
            Debug.Log("OnNewWorld");
        }

        public void OnLoad(ClickEvent e)
        {
            Debug.Log("OnLoad");
        }

        public void OnSettings(ClickEvent e)
        {
            Debug.Log("OnSettings");
        }

        public void OnManual(ClickEvent e)
        {
            Debug.Log("OnManual");
        }

        public void OnExit(ClickEvent e)
        {
            Debug.Log("OnExit");

        }

        public void OnTestWorld(ClickEvent e)
        {
            Debug.Log("OnTestWorld");
            gameManager.worldGenManager.GenFlatWorld();
            gameManager.terrainManager.GenerateFlatTerrain();
            gameManager.terrainManager.ManageTerrain();
            ultimateTerrain.enabled = true;
        }
    }
}