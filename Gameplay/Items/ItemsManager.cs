using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;


namespace Urth
{

    public class ItemsManager : MonoBehaviour
    {
        public ItemsLibrary itemsLibrary;
        public UrthTime time;
        int nextID = 0;
        public int GetNextId()
        {
            return nextID++;
        }
        public static ItemsManager Instance { get; private set; }
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
            spawnQueue = new Queue<int>();
            despawnList = new List<int>();
            population = new Dictionary<int, UItemData>(10000);
            pxCreatures = new Dictionary<int2, List<int>>(10000);
            worldSet = new HashSet<int>();
            fastSet = new HashSet<int>();
        }

        // Start is called before the first frame update
        void Start()
        {
            time = UrthTime.Instance;

            //TestPopulate();
            PopulateTile(GameManager.Instance.gameOriginCell);
        }

        float fastPeriod = 3f;
        float worldPeriod = 3000f;

        double fastLast = 0f;
        double worldLast = 0f;

        int fastIdx = 0;
        int worldIdx = 0;


        int framecount = 0;
        float neededFastSetRate = 0;
        float neededWorldSetRate = 0;

        float FRAME_TIME = 0.016f;
        void Update()
        {
            framecount++;
            if (framecount == 100)
            {
                framecount = 0;
                lastUpdateUrthTime = time.totalGameSeconds;
                //scan tiles around player to see if creatures should spawn in
                List<int> spawnIds = GetShouldSpawnList();
                foreach (int id in spawnIds)
                {
                    if (fastSet.Contains(id))
                    {
                        Debug.Log("Creature Already Spawned");
                        continue;
                    }

                    UItemData spawnItemData = population[id];
                    GameObject prefab = spawnItemData.assembled ? itemsLibrary.prefabsDict[spawnItemData.type].assembledPrefab : itemsLibrary.prefabsDict[spawnItemData.type].componentPrefab;
                    GameObject instance = Instantiate(prefab, spawnItemData.pos, Quaternion.Euler(spawnItemData.facing));
                    UItem item = instance.AddComponent<UItem>();
                    item.data = spawnItemData;
                    spawnItemData.item = item;
                    spawnedItems.Add(id, item);
                    fastSet.Add(id);
                    worldSet.Remove(id);
                }

                foreach (int id in shouldDespawnList)
                {
                    fastSet.Remove(id);
                    worldSet.Add(id);
                    spawnedItems[id].data = null;
                    Destroy(spawnedItems[id].gameObject);
                }
                shouldDespawnList = new List<int>(100);

                //set the number of BodyManager Updates processed per frame
                neededFastSetRate = fastSet.Count / fastPeriod;
                fastList = fastSet.ToList();
                neededWorldSetRate = worldSet.Count / worldPeriod;
                worldList = worldSet.ToList();
            }
            else
            {
                //int endFastIdx = (int)(neededFastSetRate * (time.totalGameSeconds - lastUpdateUrthTime) + 0.5f);
                ////(int)math.round(neededFastSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
                //for (int i = fastIdx; i < endFastIdx; i++)
                //{
                //    population[fastList[i]].Update(time.totalGameSeconds);
                //}
                //fastIdx = endFastIdx;

                //int endWorldIdx = (int)(neededWorldSetRate * (time.totalGameSeconds - lastUpdateUrthTime) + 0.5f);
                ////int worldSteps = (int)math.round(neededWorldSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
                //for (int i = worldIdx; i < endWorldIdx; i++)
                //{
                //    population[worldList[i]].Update(time.totalGameSeconds);
                //}
                //worldIdx = endWorldIdx;
            }
        }

        double lastUpdateUrthTime = 0f;

        float lastUpdateTime = 0f;

        static float SPAWN_RADIUS = 4000f;
        static float SQ_SPAWN_RADIUS = SPAWN_RADIUS * SPAWN_RADIUS;
        static float DESPAWN_RADIUS = 4200f;
        public static float SQ_DESPAWN_RADIUS = DESPAWN_RADIUS * DESPAWN_RADIUS;



        /*Transfering "donuts"
         * Each time a creature is updated, check if should transfer
         * If it should put on transferToTargetSet
         *  if moving to a higher-frequency set, add to that set immediately
         *  if moving to lower-frequency set, put on transferToTarget set
         *  remove from current set at end of its current loop
         */
        public HashSet<int> transferToFastSet;
        //public HashSet<int> transferToNearbySet;
        //public HashSet<int> transferToRegionSet;
        public HashSet<int> transferToWorldSet;

        public HashSet<int> removeFromFastSet;
        //public HashSet<int> removeFromNearbySet;
        //public HashSet<int> removeFromRegionSet;
        public HashSet<int> removeFromWorldSet;

        //new 
        public HashSet<(int, int, int)> transfer;
        public void TransferItem()
        {
            foreach (var tup in transfer)
            {
                int id = tup.Item1;
                int src = tup.Item2;
                int dst = tup.Item3;
                switch (src)
                {
                    case -1:
                        //no source, skip
                        break;
                    case 0:
                        fastSet.Remove(id);
                        break;
                    case 1:
                        nearbySet.Remove(id);
                        break;
                    case 2:
                        regionSet.Remove(id);
                        break;
                    case 3:
                        worldSet.Remove(id);
                        break;
                }
                switch (dst)
                {
                    case -1:
                        //no destination, skip
                        break;
                    case 0:
                        fastSet.Add(id);
                        break;
                    case 1:
                        nearbySet.Add(id);
                        break;
                    case 2:
                        regionSet.Add(id);
                        break;
                    case 3:
                        worldSet.Add(id);
                        break;
                }
            }
            transfer.Clear();
            transfer = new HashSet<(int, int, int)>();
        }


        public List<int> GetShouldSpawnList()
        {//TODO use 3d distance. there are cliffs
            List<int> ids = new List<int>(1000);
            float3 playerPos = GameManager.Instance.GetPlayerWorldPos();
            int2 playerPxIdx = 10 * new int2((int)(playerPos.x / 10f), (int)(playerPos.z / 10f));
            for (int y = -(int)SPAWN_RADIUS; y < SPAWN_RADIUS; y += 10)
            {
                int xr = (int)(SPAWN_RADIUS * Mathf.Cos((y) / SPAWN_RADIUS * Mathf.PI / 2));
                for (int x = -xr; x < xr; x += 10)
                {
                    if (pxCreatures.TryGetValue(playerPxIdx + new int2(x, y), out List<int> pxIds))
                    {
                        ids.AddRange(pxIds);
                    }
                }
            }
            return ids;
        }


        Queue<int> spawnQueue;
        void SpawnItems()
        {
            foreach (int id in spawnQueue)
            {
                SpawnItem(id);
            }
            spawnQueue.Clear();
        }
        void SpawnItem(int id)
        {
            UItemData spawnItemData = population[id];
            GameObject prefab = spawnItemData.assembled ? itemsLibrary.prefabsDict[spawnItemData.type].assembledPrefab : itemsLibrary.prefabsDict[spawnItemData.type].componentPrefab;
            float3 gamePosition = spawnItemData.pos - GameManager.Instance.gameWorldOffset;
            GameObject instance = Instantiate(prefab, gamePosition, Quaternion.Euler(spawnItemData.facing));
        }


        List<int> despawnList;
        void DespawnCreatures()
        {
            foreach (int id in despawnList)
            {
                DespawnItem(id);
            }
            despawnList.Clear();
        }
        void DespawnItem(int id)
        {
            GameObject go = spawnedItems[id].gameObject;
            Destroy(go);
        }


        public List<int> worldList;
        public List<int> regionList;
        public List<int> nearbyList;
        public List<int> fastList;


        public HashSet<int> worldSet;
        public HashSet<int> regionSet;
        public HashSet<int> nearbySet;
        public HashSet<int> fastSet;

        public Dictionary<int, UItemData> population;
        public Dictionary<int2, List<int>> pxCreatures;
        public Dictionary<int, UItem> spawnedItems;

        public List<int> shouldDespawnList;



        public void PopulateTile(int2 tileKey)
        {
            TerrainManager tmInstance = TerrainManager.Main;
            //tmInstance = TerrainManager.Main;
            int2 tileXY = tileKey * TerrainManager.TILE_LENGTH_M;

            for (int x = 0; x < TerrainManager.TILE_LENGTH_PX; x++)
            {
                for (int y = 0; y < TerrainManager.TILE_LENGTH_PX; y++)
                {
                    int pxIdx = tmInstance.TilePosToTerrainIdx(new int2(x, y), tileKey);

                    float height = tmInstance.heights[pxIdx];
                    float gradient = tmInstance.gradients[pxIdx];
                    float aspect = tmInstance.aspects[pxIdx];
                    float temp = tmInstance.temps[pxIdx];
                    float precip = tmInstance.precips[pxIdx];
                    float soil = tmInstance.soilDepths[pxIdx];
                    float fracturedBedrock = tmInstance.fracturedBedrockDepths[pxIdx];
                    float gwDepth = tmInstance.groundwaterDepths[pxIdx];

                    float pet = TerrainManager.GetPET(temp);
                    float aridity = pet / precip;
                    float gwThickness = (soil + fracturedBedrock) - gwDepth;

                    if (temp < TerrainManager.BIOME_TEMP_ICE || (precip < 100 && (gwDepth > 10 || gwThickness < 2)) || (soil + fracturedBedrock < 0.01f))
                    {//Barren. Frozen, no water supply, or bare rock
                        continue;
                    }

                    int2 pxXY = new int2(x, y) * TerrainManager.TERRAIN_PX_LENGTH_M;
                    float3 pos = new float3(tileXY.x + pxXY.x, height, tileXY.y + pxXY.y);
                    float roll = noise.snoise(pos);
                    //if (roll < 0.6f) { continue; }
                    //biome-scores is an array of tuples, the values are weight and biome type
                    NativeArray<float2> biomeScores = tmInstance.GetRegionalBiomeScores(height, temp, aridity);
                    List<float2> sortedScores = new List<float2>(biomeScores.Length);
                    foreach (float2 biome in biomeScores)
                    { //TODO don't think this is correct
                        int weightsIdx = 0;
                        while (weightsIdx < sortedScores.Count && biome.x < sortedScores[weightsIdx].x) { weightsIdx++; }
                        sortedScores.Insert(weightsIdx, biome);
                    }

                    float accum = 0f;
                    int idx = 0;
                    while (idx < sortedScores.Count - 1)
                    {
                        accum += sortedScores[idx].x;
                        if (roll < accum) { break; }
                        idx++;
                    }

                    BIOME biomeType = (BIOME)(int)(sortedScores[idx].y + 0.5f);

                    //    foreach (CREATURE speciesType in itemsLibrary.biomeSpecies[biomeType])
                    //    {
                    //        CreatureSpecies species = itemsLibrary.speciesDict[speciesType];
                    //        float densityPerSqKm = species.biomeDensityDict[biomeType];
                    //        float countPerPx = densityPerSqKm * TerrainManager.TPX_AREA_KM;
                    //        float r = UnityEngine.Random.value;
                    //        if (r < countPerPx)
                    //        {
                    //            CreatureBody creatureBody = RandomCreature(speciesType, species, pos);
                    //            population.Add(creatureBody.data.ID, creatureBody);
                    //            if (!pxCreatures.ContainsKey(tileXY + pxXY))
                    //            {
                    //                pxCreatures.Add(tileXY + pxXY, new List<int>(4));
                    //            }
                    //            pxCreatures[tileXY + pxXY].Add(creatureBody.data.ID);
                    //        }
                    //    }
                }
            }
        }

        //public CreatureBody RandomItem(ITEM speciesType, UItemTemplate template, float3 pos)
        //{

        //}

        public void AddCustomItem(UItemData data)
        {
            population.Add(data.ID, data);
        }
    }

}