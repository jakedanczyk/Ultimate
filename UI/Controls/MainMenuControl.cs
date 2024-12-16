using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class MainMenuControl : MonoBehaviour
    {
        public GameManager gameManager;
        public UltimateTerrains.UltimateTerrain ultimateTerrain;
        public CreaturesLibrary creatureLibrary;
        public GameObject playerCharacter;
        public PhysicsBasedCharacterController.CharacterManager characterManager;
        public GameObject flyCamera;
        public UIDocument mainMenuDocument;
        public GameObject mainMenuCam;

        public Button btnNewWorld;
        public Button btnLoadWorld;
        public Button btnSettings;
        public Button btnManual;
        public Button btnExit;
        public Button btnTestWorld;
        public Button btnFlatWorld;

        public VisualElement buttonsContainer;
        public VisualElement container;
        public VisualElement menu;

        public static MainMenuControl Instance { get; private set; }
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

            mainMenuDocument = this.GetComponent<UIDocument>();
            buttonsContainer = mainMenuDocument.rootVisualElement.Query("menuOptions").First();
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

            btnFlatWorld = (Button)buttonsContainer.Query("flatWorld").First();
            btnFlatWorld.RegisterCallback<ClickEvent>(OnFlatWorld);
        }


        public void OnNewWorld(ClickEvent e)
        {
            Debug.Log("OnNewWorld");
            //Open NewWorld Menu
        }

        public void OnLoad(ClickEvent e)
        {
            Debug.Log("OnLoad");
            //Open Load SaveGame Menu
        }

        public void OnSettings(ClickEvent e)
        {
            Debug.Log("OnSettings");
            //Open Settings Menu
        }

        public void OnManual(ClickEvent e)
        {
            Debug.Log("OnManual");
            //Open Manual
        }

        public void OnExit(ClickEvent e)
        {
            Debug.Log("OnExit");
            if (Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
            }
            else
            {
                Application.Quit();
            }
        }

        public void OnTestWorld(ClickEvent e)
        {
            Debug.Log("OnTestWorld");
            gameManager.worldGenManager.GenWorld();
            gameManager.worldGenManager.GenerateTerrain();
            gameManager.terrainManager.ManageTerrain();
            ultimateTerrain.enabled = true;
        }

        public void OnFlatWorld(ClickEvent e)
        {
            Debug.Log("OnFlatWorld");
            gameManager.worldGenManager.GenFlatWorld();
            gameManager.terrainManager.GenerateFlatTerrain();
            gameManager.terrainManager.ManageTerrain();
            ultimateTerrain.enabled = true;
        }

        public void Activate()
        {
            Debug.Log("Main menu Activaate");
            mainMenuCam.SetActive(true);
            mainMenuDocument.enabled = true;// .SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log("Main menu DeActivaate");
            mainMenuCam.SetActive(false);
            mainMenuDocument.gameObject.SetActive(false);
        }

        public void SetVisible()
        {
            mainMenuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void SetHidden()
        {
            mainMenuDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}