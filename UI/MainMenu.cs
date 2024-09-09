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

        public Button newWorld;
        public Button loadWorld;
        public Button settings;
        public Button manual;
        public Button exit;
        public Button testWorld;

        public VisualElement outer;
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
            outer = mainMenu.rootVisualElement.Query("outer").First();
            container = mainMenu.rootVisualElement.Query("container").First();
            menu = mainMenu.rootVisualElement.Query("menu").First();

            testWorld = (Button)mainMenu.rootVisualElement.Query("testWorld").First();
            testWorld.RegisterCallback<ClickEvent>(OnTestWorld);

        }

        public void OnTestWorld(ClickEvent e)
        {
            gameManager.worldGenManager.GenFlatWorld();
            gameManager.terrainManager.GenerateFlatTerrain();
            gameManager.terrainManager.ManageTerrain();
            ultimateTerrain.enabled = true;
        }
    }
}