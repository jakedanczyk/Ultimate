using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;


namespace Urth
{
    public class GameMasterPanelControl : UIPanelControl
    {
        public GameManager gameManager;
        public CreaturesLibrary creatureLibrary;
        public GameObject playerCharacter;
        public PhysicsBasedCharacterController.CharacterManager characterManager;
        public GameObject flyCamera;
        public UIDocument gmMenu;

        public Button fly;
        public Button disable;
        public Button invulnerable;

        public TextField typeText;
        public TextField genderText;
        public TextField sizeText;
        public TextField lengthText;
        public TextField heightText;
        public TextField widthText;
        public TextField weightText;
        public TextField ageText;
        public Button spawn;

        public VisualElement outer;
        public VisualElement container;
        public VisualElement menu;

        public static GameMasterPanelControl Instance { get; private set; }
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

            //gmMenu = this.GetComponent<UIDocument>();
            //outer = gmMenu.rootVisualElement.Query("outer").First();
            //outer.pickingMode = PickingMode.Ignore;
            //container = gmMenu.rootVisualElement.Query("container").First();
            //menu = gmMenu.rootVisualElement.Query("menu").First();
            //disable = (Button)gmMenu.rootVisualElement.Query("Disable").First();
            //disable.RegisterCallback<ClickEvent>(OnDisablePlayer);
            //invulnerable = (Button)gmMenu.rootVisualElement.Query("Invulnerable").First();
            //invulnerable.RegisterCallback<ClickEvent>(OnInvulnerable);

            //typeText = (TextField)gmMenu.rootVisualElement.Query("type");
            //genderText = (TextField)gmMenu.rootVisualElement.Query("gender");
            //lengthText = (TextField)gmMenu.rootVisualElement.Query("length");
            //heightText = (TextField)gmMenu.rootVisualElement.Query("height");
            //widthText = (TextField)gmMenu.rootVisualElement.Query("width");
            //weightText = (TextField)gmMenu.rootVisualElement.Query("weight");
            //ageText = (TextField)gmMenu.rootVisualElement.Query("age");
            //spawn = (Button)gmMenu.rootVisualElement.Query("spawn").First();
            //spawn.RegisterCallback<ClickEvent>(OnSpawnCreature);
        }

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.Instance;
            creatureLibrary = CreaturesLibrary.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void OnFly(ClickEvent e)
        {
            Debug.Log("fly");
            flyCamera.SetActive(!flyCamera.activeInHierarchy);
            playerCharacter.SetActive(!playerCharacter.activeInHierarchy);
        }
        public void OnDisablePlayer(ClickEvent e)
        {
            Debug.Log("disable");
            playerCharacter.SetActive(!playerCharacter.activeSelf);
        }
        public void OnInvulnerable(ClickEvent e)
        {
            Debug.Log("invulnv");

        }


        public void OnSpawnCreature(ClickEvent e)
        {
            CREATURE type = (CREATURE)System.Enum.Parse(typeof(CREATURE), typeText.value);
            GENDER gender = (GENDER)System.Enum.Parse(typeof(GENDER), genderText.value);
            Vector3 spawnPos = GameManager.Instance.playerCharacterObject.transform.position + GameManager.Instance.playerCharacterObject.transform.forward * 3;
            GameObject newCreature = Instantiate(creatureLibrary.prefabsDict[type], spawnPos, Quaternion.identity);
            CreatureManager newCreatureManager = newCreature.GetComponent<CreatureManager>();

            float length = float.Parse(lengthText.value);
            float height = float.Parse(heightText.value);
            float weight = float.Parse(weightText.value);
            float width = float.Parse(widthText.value);
            float age = float.Parse(ageText.value);

            CreatureSpecies species = creatureLibrary.speciesDict[type];
            CreatureData newData = new CreatureData(CreaturesManager.Instance.GetNextId(), type, gender, age, weight, length, height, width, spawnPos);
            CreatureBody newBody = new CreatureBody(newData, species);
            newCreatureManager.PrepareForSpawn(newBody);
            CreaturesManager.Instance.AddCustomCreature(newBody);
        }

        public void OnSpawnItem(ClickEvent e)
        {
            ITEM type = (ITEM)System.Enum.Parse(typeof(ITEM), typeText.value);
            //GENDER gender = (GENDER)System.Enum.Parse(typeof(GENDER), genderText.value);
            //GameObject newCreature = Instantiate(creatureLibrary.prefabsDict[type]);
            //CreatureManager creatureManager = newCreature.GetComponent<CreatureManager>();

            //float length = float.Parse(lengthText.value);
            //float height = float.Parse(heightText.value);
            //float mass = float.Parse(massText.value);
            //float width = float.Parse(widthText.value);
            //float age = float.Parse(ageText.value);

            //CreatureData data = new CreatureData(CreaturesManager.Instance.GetNextId(), type, gender, age, mass, length, height, width, float3.zero);
            //CreatureBody body = new CreatureBody(data);
            //creatureManager.PrepareForSpawn(body);
        }

    }
}