using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    /*The primary tiles are of 10km side length
    *Distant tiles
    * At max display distance, each tile has a number of tile-wide meshes
    * (similar plants are grouped to keep the number of meshes to 2 or 3)
    * Periodically check for distant tiles to load/unload
    *More frequently, update the near-tiles
    *  there are 1-4 nearby tiles, depending on the position and render distance
    *  For nearby tiles, additional sub-tiles are created
    *  there are "mesotiles" of 2-km side length
    *  below these are "microtiles" of 200m side length
    *  
    * keep a bank of all tiles loaded
    * tiles at at distance have just full-tile LODs turned on
    * near-tiles 
    *  have distant-LODS turned off
    *  generate a full grid of meso-LODs
    *  meso-LODs within micro-tile range are turned off and replaced with micro-LODs
    *  micro-LODS within full-detail range are turned off and replace with individual plants
    */
    public class PlantsManager : MonoBehaviour
    {
        public static float DISTANT_DISPLACEMENT_TRIGGER_SQR = 500f;
        public static float DISPLACEMENT_TRIGGER_SQR = 50f;
        public static float NEAR_TILE_DIST = 1000f;
        public static float MICRO_TILE_DIST = 200f;
        public static float RENDER_DIST = 100f;
        public static int MESO_TILE_LENGTH_M = 2000;
        public static int MICRO_TILE_LENGTH_M = 200;

        public PlantsLibrary plantsLibrary;
        public UrthTime time;

        int UNLOAD_RANGE = 3;
        int nextID = 0;
        public int GetNextId()
        {
            return nextID++;
        }

        public List<int2> keysToUnloadList = new List<int2>();
        public List<int2> keysToDistantLoadList = new List<int2>();
        public HashSet<int2> keysToDistantLoadSet = new HashSet<int2>();
        public List<int2> keysToNearLoadList = new List<int2>();
        public HashSet<int2> keysToNearLoadSet = new HashSet<int2>();
        public List<int2> keysToMesoLoadList = new List<int2>();
        public HashSet<int2> keysToMesoLoadSet = new HashSet<int2>();
        public List<int2> keysToMicroLoadList = new List<int2>();
        public HashSet<int2> keysToMicroLoadSet = new HashSet<int2>();

        public HashSet<int2> loadedKeys = new HashSet<int2>();
        public HashSet<int2> loadedNearKeys = new HashSet<int2>();
        public HashSet<int2> loadedMesoKeys = new HashSet<int2>();
        public HashSet<int2> loadedMicroKeys = new HashSet<int2>();
        public Dictionary<int2, PlantTileMacro> loadedTiles = new Dictionary<int2, PlantTileMacro>() { };
        public Dictionary<int2, PlantTileMeso> loadedMesoTiles = new Dictionary<int2, PlantTileMeso>() { };
        public Dictionary<int2, PlantTileMicro> loadedMicroTiles = new Dictionary<int2, PlantTileMicro>() { };
        public Dictionary<int, Plant> loadedPlants = new Dictionary<int, Plant>() { };
        public List<PlantTileMacro> tiles = new List<PlantTileMacro>();

        public GameObject tilePrefab;
        public GameObject mesoTilePrefab;
        public GameObject microTilePrefab;
        public Texture coniferBillboard;
        public Texture broadleafBillboard;
        public Texture palmBillboard;

        public Transform playerTransform;

        GameManager gameManager;
        WorldGenManager worldGenManager;
        TerrainManager terrainManager;
        public static PlantsManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            worldGenManager = this.gameObject.GetComponent<WorldGenManager>();
            terrainManager = this.gameObject.GetComponent<TerrainManager>();
        }


        // Start is called before the first frame update
        void Start()
        {
            //GetTilesToLoad();
        }

        int c = 0;
        Vector3 playerPositionLastDistantUpdate;
        Vector3 distantDisplacement;
        Vector3 playerPositionLastUpdate;
        Vector3 displacement;
        // Update is called once per frame
        public bool managing = false;
        public void StartManaging() { managing = true; }
        public void StopManaging() { managing = false; }

        void Update()
        {
            if (managing)
            {
               
                if ((playerTransform.position - playerPositionLastUpdate).sqrMagnitude > DISPLACEMENT_TRIGGER_SQR)
                {
                    playerPositionLastDistantUpdate = playerTransform.position;
                    GetTilesToLoad();
                    GetMicroTilesToLoad();
                }
                else if (keysToDistantLoadList.Count > 0)
                {
                    LoadNextTile();
                }
                else if (keysToUnloadList.Count > 0)
                {
                    UnloadDistantTiles();
                }
                else if (keysToNearLoadList.Count > 0)
                {
                    LoadNextNearTile();
                }
                else if (keysToMesoLoadList.Count > 0)
                {
                    LoadNextMesoTile();
                }
                else if (keysToMicroLoadList.Count > 0)
                {
                    LoadNextMicroTile();
                }
            }
        }

        int TILES_MAX_LOD = 1;
        public void GetTilesToLoad()
        {
            int2 orig = GameManager.Instance.gameOriginCell;
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);
            foreach (int2 tileKey in UrthUtility.GetAreaTiles(TILES_MAX_LOD, orig))
            {
                if (!loadedKeys.Contains(tileKey) && !keysToDistantLoadSet.Contains(tileKey))
                {
                    keysToDistantLoadList.Add(tileKey);
                    keysToDistantLoadSet.Add(tileKey);
                }
                int2 tileMin = tileKey * TerrainManager.TILE_LENGTH_KM * 1000;
                int2 tileMax = tileMin + new int2(TerrainManager.TILE_LENGTH_KM * 1000, TerrainManager.TILE_LENGTH_KM * 1000);
                float dist = UrthUtility.DistanceFromRect(tileMin, tileMax, playerWorldPos2d);
                if(dist < NEAR_TILE_DIST && !loadedNearKeys.Contains(tileKey))
                {
                    keysToNearLoadList.Add(tileKey);
                    keysToNearLoadSet.Add(tileKey);
                }
            }
        }

        //find the meso tiles close enough to replace with micro tiles
        public void GetMicroTilesToLoad()
        {
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);
            int2 playerMesoTile = (int2)(playerWorldPos2d * MESO_TILE_LENGTH_M)/ MESO_TILE_LENGTH_M;
            foreach (int2 tileIdx in UrthUtility.GetAreaTiles(1, int2.zero))
            {
                int2 mesoTilePosition = playerMesoTile + (tileIdx * MESO_TILE_LENGTH_M);
                int2 tileMin = mesoTilePosition;
                int2 tileMax = tileMin + new int2(MESO_TILE_LENGTH_M , MESO_TILE_LENGTH_M);
                float dist = UrthUtility.DistanceFromRect(tileMin, tileMax, playerWorldPos2d);
                if (dist < MICRO_TILE_DIST && !loadedMesoKeys.Contains(mesoTilePosition) && !keysToMesoLoadSet.Contains(mesoTilePosition))
                {
                    keysToMesoLoadList.Add(mesoTilePosition);
                    keysToMesoLoadSet.Add(mesoTilePosition);
                }
            }
        }

        //find the micro tiles close enough to replace with GameObjects
        public void GetDetailTilesToLoad()
        {
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);
            int2 playerMicroTile = (int2)(playerWorldPos2d * MICRO_TILE_LENGTH_M) / MICRO_TILE_LENGTH_M;
            foreach (int2 tileIdx in UrthUtility.GetAreaTiles(1, int2.zero))
            {
                int2 microTilePosition = playerMicroTile + (tileIdx * MICRO_TILE_LENGTH_M);
                int2 tileMin = microTilePosition;
                int2 tileMax = tileMin + new int2(MICRO_TILE_LENGTH_M, MICRO_TILE_LENGTH_M);
                float dist = UrthUtility.DistanceFromRect(tileMin, tileMax, playerWorldPos2d);
                if (dist < RENDER_DIST && !loadedMicroKeys.Contains(microTilePosition) && !keysToMicroLoadSet.Contains(microTilePosition))
                {
                    keysToMicroLoadList.Add(microTilePosition);
                    keysToMicroLoadSet.Add(microTilePosition);
                }
            }
        }

        public void UnloadDistantTiles()
        {
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);

            int2 orig = GameManager.Instance.gameOriginCell;
            foreach (int2 tileKey in loadedKeys)
            {
                int2 tileMin = tileKey * TerrainManager.TILE_LENGTH_KM * 1000;
                int2 tileMax = tileMin + new int2(TerrainManager.TILE_LENGTH_KM * 1000, TerrainManager.TILE_LENGTH_KM * 1000);
                float dist = UrthUtility.DistanceFromRect(tileMin, tileMax, playerWorldPos2d);
                if (dist > 2 * TerrainManager.TILE_LENGTH_KM * 1000)
                {
                    PlantTileMacro tile = loadedTiles[tileKey];
                    Destroy(tile);
                    loadedKeys.Remove(tileKey);
                }
            }
        }

        public void UnloadDistantNearTiles()
        {
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);

            foreach (int2 tileKey in loadedNearKeys)
            {
                int2 tileMin = tileKey * TerrainManager.TILE_LENGTH_KM * 1000;
                int2 tileMax = tileMin + new int2(TerrainManager.TILE_LENGTH_KM * 1000, TerrainManager.TILE_LENGTH_KM * 1000);
                float dist = UrthUtility.DistanceFromRect(tileMin, tileMax, playerWorldPos2d);
                if (dist > 2 * NEAR_TILE_DIST)
                {
                    ReplaceNearTileWithDistant(tileKey);
                }
            }
        }

        public void UnloadDistantMicroTiles()
        {
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);

            foreach (int2 tileKey in loadedMicroKeys)
            {
                int2 tileMin = tileKey * MICRO_TILE_LENGTH_M;
                int2 tileMax = tileMin + new int2(MICRO_TILE_LENGTH_M, MICRO_TILE_LENGTH_M);
                float dist = UrthUtility.DistanceFromRect(tileMin, tileMax, playerWorldPos2d);
                if (dist > 2 * MICRO_TILE_DIST)
                {
                    PlantTileMicro tile = loadedMicroTiles[tileKey];
                    Destroy(tile);
                }
            }
        }

        public void ReplaceNearTileWithDistant(int2 tileKey)
        {
            loadedTiles[tileKey].UnloadMeso();
            loadedTiles[tileKey].TurnOnBillboards();
        }


        public void LowerDetailDistantTiles()
        {
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            float2 playerWorldPos2d = new float2(playerWorldPos.x, playerWorldPos.z);
            int2 playerMesoTile = (int2)(playerWorldPos2d * MESO_TILE_LENGTH_M) / MESO_TILE_LENGTH_M;
            foreach (int2 tileKey in loadedKeys)
            {
                int2 diff = playerMesoTile - tileKey;
                int l = diff.x * diff.x + diff.y * diff.y;
                if (l > NEAR_TILE_DIST)
                {
                    UnloadTile(tileKey);
                }
            }
            int2 playerMicroTile = (int2)(playerWorldPos2d * MICRO_TILE_LENGTH_M) / MICRO_TILE_LENGTH_M;
        }

        public void LoadNextTile()
        {
            int2 tileKey = keysToDistantLoadList[0];
            if (loadedKeys.Contains(tileKey))
            {
                return;
            }
            keysToDistantLoadList.RemoveAt(0);
            keysToDistantLoadSet.Remove(tileKey);
            int2 rel = tileKey - GameManager.Instance.gameOriginCell;
            rel *= TerrainManager.TILE_LENGTH_M;
            Vector3 tileGamePos = new Vector3(rel.x, 0, rel.y);
            GameObject newTileObj = Instantiate(tilePrefab, tileGamePos, Quaternion.identity);
            PlantTileMacro floraTile = newTileObj.GetComponent<PlantTileMacro>();
            floraTile.tileKey = tileKey;
            floraTile.Populate(terrainManager);
            floraTile.MakeBillboards();
            loadedKeys.Add(tileKey);
            loadedTiles[tileKey] = floraTile;
        }

        /*Fill a Tile with MesoTiles and disable full tile Billboards
        */
        public void LoadNextNearTile()
        {
            int2 tileKey = keysToNearLoadList[0];
            if (loadedNearKeys.Contains(tileKey))
            {
                return;
            }
            keysToNearLoadList.RemoveAt(0);
            keysToNearLoadSet.Remove(tileKey);
            PlantTileMacro tile = loadedTiles[tileKey];
            tile.TurnOffBillboards();
            Dictionary<int2, PlantTileMeso> newMesoTiles = tile.GenerateMesoTiles();
            foreach(int2 newMesoTileKey in newMesoTiles.Keys)
            {
                PlantTileMeso mesoTile = newMesoTiles[newMesoTileKey];
                loadedMesoKeys.Add(newMesoTileKey);
                loadedMesoTiles[newMesoTileKey] = mesoTile;
            }
            loadedNearKeys.Add(tileKey);
        }

        /*Fill a MesoTile with MicroTile and disable MesoTile Billboards
         */
        public void LoadNextMesoTile()
        {
            int2 mesoTileKey = keysToMesoLoadList[0];
            if (loadedMesoKeys.Contains(mesoTileKey))
            {
                return;
            }
            keysToMesoLoadList.RemoveAt(0);
            keysToMesoLoadSet.Remove(mesoTileKey);
            PlantTileMeso mesoTile = loadedMesoTiles[mesoTileKey];
            Dictionary<int2, PlantTileMicro> newMicroTiles = mesoTile.GenerateMicroTiles();
            foreach (int2 newMicroTileKey in newMicroTiles.Keys)
            {
                PlantTileMicro microTile = newMicroTiles[newMicroTileKey];
                loadedMicroKeys.Add(newMicroTileKey);
                loadedMicroTiles[newMicroTileKey] = microTile;
            }
            loadedMesoKeys.Add(mesoTileKey);
        }

        /*Fill a MicroTile with Plants
        */
        public void LoadNextMicroTile()
        {
            int2 microTileKey = keysToMicroLoadList[0];
            if (loadedMicroKeys.Contains(microTileKey))
            {
                return;
            }
            keysToMicroLoadList.RemoveAt(0);
            keysToMicroLoadSet.Remove(microTileKey);
            PlantTileMicro microTile = loadedMicroTiles[microTileKey];
            Dictionary<int, Plant> newPlants = microTile.GeneratePlants();
            //foreach (int newPlantId in newPlants.Keys)
            //{
            //    Plant plant = newPlants[newPlantId];
            //    loadedPlants.Add(newPlantId,plant);
            //}
            loadedMicroKeys.Add(microTileKey);
        }

        public void UnloadTile(int2 tileKey)
        {
            if (!loadedKeys.Contains(tileKey))
            {
                return;
            }
            GameObject go = loadedTiles[tileKey].gameObject;
            loadedTiles.Remove(tileKey);
            loadedKeys.Remove(tileKey);
            Destroy(loadedTiles[tileKey].gameObject);
        }


        public Dictionary<BIOME, List<PlantData>> biomeFlora = new Dictionary<BIOME, List<PlantData>>(10);
        public PlantData ProceduralPlant(PLANT speciesType, BIOME biomeType, float3 pos)
        {//this is for creating procedural perennial plants in world gen
            //They are much more "real-estate" constrained then animals, growing on and filling all available space
            //This results in age distribution curves for plants being very different from animals,
            //at least for plants large enough for player to interact with
            //age-distribution in vegetation depends heavily on fire (or other disturbances, fire dominant in nature)
            //If wildfires tend to occur, and tend to be devastating, the age curve will be like animals, skewing young
            //If wildfires don't occur and/or are weak, large old plants will dominant the landscape
            //
            //for now, implementation will simply be linear-random plus 1 year
            //TODO better plant sizing
            //TODO at end of each game year, we can check for death (from player, ecosystem sim, or old age) and generate individuals
            float ageRoll = UnityEngine.Random.value;
            PlantSpecies species = plantsLibrary.speciesDict[speciesType];

            float age = ageRoll * (species.maxAge-1f) + 1f;
            float height = UrthUtility.SampleGrowthCurve(age, species.growthCurve);
            height *= plantsLibrary.biomeSpeciesSizeAdjustments[biomeType][speciesType];
            float width = height / species.slenderness;
            PlantData plantData = new PlantData(nextID++, speciesType, height, width, -age, pos);
            return plantData;
        }

        public void AddCustomPlant(PlantData plantData)
        {

        }
    
        /* Save all data to JSON
         * Each PlantTile receives a file
         */
        public void SavePlants()
        {
            //TODO setting save location

            foreach (int2 tileKey in loadedTiles.Keys)
            {
                //TODO new file foreach tile

                foreach (int plantId in loadedTiles[tileKey].population.Keys)
                {
                    PlantData plant = loadedTiles[tileKey].population[plantId];
                    //TODO JSON details
                }
            }
        }
        public PlantData GetPlantData(PlantTag plantTag)
        {
            PlantTileMacro tile = loadedTiles[plantTag.tile];
            PlantData data = tile.population[plantTag.id];
            return data;
        }

    }
}
