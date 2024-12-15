using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using MalbersAnimations.Controller;

namespace Urth
{
    public enum PLAYER_STATE
    {
        NULL,
        INACTIVE,
        FREECAM,
        CHARACTER,
    }

    public enum SCENE_TYPE
    {
        URTH,
        VOXEL_ARENA,
        MESH_ARENA
    }

    public enum GAME_MODE
    {
        DEV,
        MAIN
    }

    public class GameManager : MonoBehaviour
    {
        public string savepath;
        public UltimateTerrains.UltimateTerrain ultimateTerrain;
        public WorldGenManager worldGenManager;
        public TerrainManager terrainManager;
        public UIManager uiManager;
        public SCENE_TYPE scene;
        public PLAYER_STATE playerState;
        public GAME_MODE gameMode;
        public PROGRAM_MODE programMode;

        public int2 gameOriginCell = new int2(19, 21);
        public float3 gameWorldOffset;

        public int OFFENSE_LAYER;
        public int ATTACK_MASK;
        public int DEFENSE_LAYER;

        public int CREATURE_LAYER;
        public int CREATURE_MASK;
        public int VISION_MASK;

        public int BUILDABLE_LAYER;


        public static GameManager Instance { get; private set; }
        private void Awake()
        {
            savepath = Application.persistentDataPath;
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                OFFENSE_LAYER = LayerMask.NameToLayer("Offense");

                //ATTACK_MASK = 1 << (0 & DEFENSE_LAYER);//Attacks can hit default layer (terrain, buildings, items), or defense colliders
                ATTACK_MASK = 1 << (DEFENSE_LAYER);//Lets make it just defense colliders for now

                CREATURE_LAYER = LayerMask.NameToLayer("Creature");
                VISION_MASK = 1 << (0 & CREATURE_LAYER);
                DEFENSE_LAYER = LayerMask.NameToLayer("Defense");
                BUILDABLE_LAYER = LayerMask.NameToLayer("Buildable");

                if (scene == SCENE_TYPE.URTH)
                {
                    int2 originCoord = gameOriginCell * TerrainManager.TILE_LENGTH_M;
                    gameWorldOffset = new float3(originCoord.x, 0, originCoord.y);
                }
                else
                {
                    gameOriginCell = int2.zero;
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //programMode is set in Awake by the ProgramManager
            switch (programMode)
            {

                case PROGRAM_MODE.MAIN:
                    SetPlayerState(PLAYER_STATE.NULL);
                    uiManager.OpenMainMenu();
                    break;
                case PROGRAM_MODE.DEV_CHAR:
                    EnableTestArena();
                    SetPlayerState(PLAYER_STATE.CHARACTER);
                    UIManager.Instance.Initialize();
                    UIManager.Instance.characterCreationMenu.SetPlayerCharacter();
                    UIManager.Instance.characterCreationMenu.playerAnimal.enabled = true;
                    //ACTIVATE OTHER PLAYER MONOS
                    break;
                case PROGRAM_MODE.DEV_FLAT:
                    SetPlayerState(PLAYER_STATE.CHARACTER);
                    UIManager.Instance.Initialize();
                    UIManager.Instance.characterCreationMenu.SetPlayerCharacter();
                    UIManager.Instance.characterCreationMenu.playerAnimal.enabled = true;
                    //ACTIVATE OTHER PLAYER MONOS
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (worldGenManager is null)
            {
                worldGenManager = WorldGenManager.Main;
            }
            else if (terrainManager is null)
            {
                terrainManager = worldGenManager.terrainManager;
            }

            if (targetState != currentState)
            {
                SetPlayerState(targetState);
            }
        }

        public GameObject testPlane;
        public GameObject playerCharacter;
        public GameObject playerCam;
        public CreatureBody playerCharacterBody;

        public MAnimal malbersPlayer;
        public MalbersAnimations.StateID flyStateID;
        public MalbersAnimations.StateID idleStateID;
        public Fly flyState;

        public List<MSpeed> freecamSpeeds;

        public PLAYER_STATE targetState;
        public PLAYER_STATE currentState;

        /// <summary>
        /// Should be only called in Awake by ProgramManager
        /// Sets programMode field, which is used in Start to set UI and player state
        /// </summary>
        /// <param name="setMode"></param>
        public void SetProgramMode(PROGRAM_MODE setMode)
        {
            programMode = setMode;
        }

        public float3 GetPlayerWorldPos()
        {
            return new float3(gameOriginCell.x, 0, gameOriginCell.y) * TerrainManager.TILE_LENGTH_M + (float3)playerCharacter.transform.position;
        }
        public float3 GetWorldPos(float3 enginePos)
        {
            return new float3(gameOriginCell.x, 0, gameOriginCell.y) * TerrainManager.TILE_LENGTH_M + enginePos;
        }

        public void SetPlayerState(PLAYER_STATE state)
        {
            switch (state)
            {
                case PLAYER_STATE.NULL:
                    playerCharacter.SetActive(false);
                    playerCam.SetActive(false);
                    break;
                case PLAYER_STATE.CHARACTER:
                    playerCam.SetActive(true);
                    flyState = (Fly)malbersPlayer.State_Get(flyStateID);
                    flyState.PitchLimit = 80;
                    flyState.InertiaLerp.Value = 0f;
                    flyState.canLand.Value = true;
                    malbersPlayer.SpeedSet_Get("Fly").Speeds = freecamSpeeds;
                    flyState.FlyForceExit();
                    //malbersPlayer.State_Try(idleStateID);
                    break;
                case PLAYER_STATE.FREECAM:
                    flyState = (Fly)malbersPlayer.State_Get(flyStateID);
                    flyState.PitchLimit = 90;
                    flyState.InertiaLerp.Value = 0f;
                    flyState.canLand.Value = false;
                    malbersPlayer.SpeedSet_Get("Fly").Speeds = freecamSpeeds;
                    malbersPlayer.State_Activate(flyStateID);
                    break;
            }
            currentState = state;
        }

        public void TeleportCharacter(CreatureManager creatureManager, float3 pos)
        {
            creatureManager.transform.position = pos;
        }
        public void SpawnPlayerCharacter(float3 pos, quaternion q)
        {
            playerCharacter.transform.position = pos;
            playerCharacter.transform.rotation = q;
        }

        public bool isPlacingStatic;
        public string placementStaticId;

        public void DisableTestArena()
        {
            testPlane.SetActive(false);
        }
        public void EnableTestArena()
        {
            testPlane.SetActive(true);
        }

    }
}