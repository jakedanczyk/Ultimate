using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public class CreaturesManager : MonoBehaviour
    {
        public float ECO_STEP_TIME_STD = 300f;//5 minutes is nominal eco step time
        public CreaturesLibrary creaturesLibrary;
        public UrthTime time;

        public static CreaturesManager Instance { get; private set; }
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
            population = new Dictionary<int, CreatureBody>(10000000);
            pxCreatures = new Dictionary<int2, List<int>>(10000000);
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
        float nearbyPeriod = 30f;
        float regionPeriod = 300f;
        float worldPeriod = 3000f;

        double fastLast = 0f;
        double nearbyLast = 0f;
        double regionLast = 0f;
        double worldLast = 0f;

        int fastIdx = 0;
        int nearbyIdx = 0;
        int regionIdx = 0;
        int worldIdx = 0;


        int framecount = 0;
        float neededFastSetRate = 0;
        float neededNearbySetRate = 0;
        float neededRegionSetRate = 0;
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

                    CreatureBody spawnCreatureBody = population[id];
                    GameObject spanPrefab = Instantiate(creaturesLibrary.prefabsDict[spawnCreatureBody.data.type], spawnCreatureBody.status.pos, Quaternion.Euler(spawnCreatureBody.status.facing));
                    CreatureManager spawnCreatureManager = spanPrefab.GetComponent<CreatureManager>();
                    spawnCreatureManager.body = spawnCreatureBody;
                    spawnCreatureBody.manager = spawnCreatureManager;
                    NPCController spawnNpcController = spanPrefab.GetComponent<NPCController>();
                    spawnNpcController.nav = GameManager.Instance.scene == SCENE_TYPE.MESH_ARENA ? LOCAL_NAV.NAVMESH : LOCAL_NAV.PATHFINDER;
                    spawnNpcController.SetManager(spawnCreatureManager);
                    spawnedCreatures.Add(id, spawnCreatureBody);
                    fastSet.Add(id);
                    worldSet.Remove(id);
                }

                foreach (int id in shouldDespawnList)
                {
                    fastSet.Remove(id);
                    worldSet.Add(id);
                    spawnedCreatures[id].manager = null;
                    Destroy(spawnedCreatures[id].manager.gameObject);
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
                int endFastIdx = (int)(neededFastSetRate * (time.totalGameSeconds - lastUpdateUrthTime) + 0.5f);
                //(int)math.round(neededFastSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
                for (int i = fastIdx; i < endFastIdx; i++)
                {
                    population[fastList[i]].Update(time.totalGameSeconds);
                }
                fastIdx = endFastIdx;

                int endWorldIdx = (int)(neededWorldSetRate * (time.totalGameSeconds - lastUpdateUrthTime) + 0.5f);
                //int worldSteps = (int)math.round(neededWorldSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
                for (int i = worldIdx; i < endWorldIdx; i++)
                {
                    population[worldList[i]].Update(time.totalGameSeconds);
                }
                worldIdx = endWorldIdx;
            }
        }

        double lastUpdateUrthTime = 0f;

        float lastUpdateTime = 0f;

        static float SPAWN_RADIUS = 4000f;
        static float SQ_SPAWN_RADIUS = SPAWN_RADIUS * SPAWN_RADIUS;
        static float DESPAWN_RADIUS = 4200f;
        public static float SQ_DESPAWN_RADIUS = DESPAWN_RADIUS * DESPAWN_RADIUS;

        void UpdateMovement()
        {
            float3 playerPos = GameManager.Instance.GetPlayerWorldPos();
            for (int id = 0; id < nextID; id++)
            {
                CreatureBody body = (CreatureBody)population[id];
                body.navigation.UpdateMovement((float)UrthTime.Instance.totalGameSeconds - lastUpdateTime);
                Vector3 toPlayer = (Vector3)(playerPos - body.data.homepos);
                if (toPlayer.sqrMagnitude < SQ_SPAWN_RADIUS && !body.manager)
                {
                    spawnQueue.Enqueue(id);
                }
                else if (body.manager && body.manager.isSpawned && toPlayer.sqrMagnitude > SQ_DESPAWN_RADIUS)
                {
                    despawnList.Add(id);
                }
            }
        }


        /*Transfering "donuts"
         * Each time a creature is updated, check if should transfer
         * If it should put on transferToTargetSet
         *  if moving to a higher-frequency set, add to that set immediately
         *  if moving to lower-frequency set, put on transferToTarget set
         *  remove from current set at end of its current loop
         */
        public HashSet<int> transferToFastSet;
        public HashSet<int> transferToNearbySet;
        public HashSet<int> transferToRegionSet;
        public HashSet<int> transferToWorldSet;

        public HashSet<int> removeFromFastSet;
        public HashSet<int> removeFromNearbySet;
        public HashSet<int> removeFromRegionSet;
        public HashSet<int> removeFromWorldSet;

        //new 
        public HashSet<(int, int, int)> transfer;
        public void TransferCreatures()
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
        void SpawnCreatures()
        {
            foreach (int id in spawnQueue)
            {
                SpawnCreature(id);
            }
            spawnQueue.Clear();
        }
        void SpawnCreature(int id)
        {
            CreatureBody body = population[id];
            if (body.creatureClass == CREATURE_CLASS.ANIMAL)
            {
                CreatureBody animalBody = (CreatureBody)body;
                GameObject go = Instantiate(creaturesLibrary.prefabsDict[animalBody.data.type]);
                CreatureManager creatureManager = go.GetComponent<CreatureManager>();
                creatureManager.PrepareForSpawn(animalBody);
                float3 gamePosition = animalBody.data.homepos - GameManager.Instance.gameWorldOffset;
                go.transform.position = gamePosition;
            }
        }



        List<int> despawnList;
        void DespawnCreatures()
        {
            foreach (int id in despawnList)
            {
                DespawnCreature(id);
            }
            despawnList.Clear();
        }
        void DespawnCreature(int id)
        {
            GameObject go = spawnedCreatures[id].manager.gameObject;
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
        int nextID = 0;
        public int GetNextId()
        {
            return nextID++;
        }
        public Dictionary<int, CreatureBody> population;
        public Dictionary<int2, List<int>> pxCreatures;
        public Dictionary<int, CreatureBody> spawnedCreatures;

        public List<int> shouldDespawnList;


        int POPULATE_RADIUS_KM = 10;
        public void TestPopulate()
        {
            population = new Dictionary<int, CreatureBody>(10000);
            int iid = 1; //id 0 is pc
            float genAreaSqKm = 3.14f * POPULATE_RADIUS_KM * POPULATE_RADIUS_KM;
            float3 playerWorldPos = GameManager.Instance.GetPlayerWorldPos();
            foreach (CREATURE type in creaturesLibrary.speciesDict.Keys)
            {
                float pop = genAreaSqKm * creaturesLibrary.speciesDict[type].density;
                for (int i = 0; i < pop; i++)
                {
                    float r = POPULATE_RADIUS_KM * 1000 * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
                    float theta = UnityEngine.Random.Range(0f, 1f) * 2f * Mathf.PI;
                    float2 relpos = new float2(r * Mathf.Cos(theta), r * Mathf.Sin(theta));
                    float2 worldpos = new float2(relpos.x + playerWorldPos.x, relpos.y + playerWorldPos.z);
                    int terrainIdx = GameManager.Instance.terrainManager.WorldPosToTerrainIdx((int2)worldpos);
                    float terrainHeight = GameManager.Instance.terrainManager.GetTerrainPxHeight(terrainIdx);
                    float3 pos = new float3(worldpos.x, terrainHeight, worldpos.y);
                    if (pos.y > 7000) //if position is unsuitable for animal dont add it
                    {//probably should instead spawn per terrain px
                        continue;
                    }

                    float yearsRoll = UnityEngine.Random.Range(0f, 1f);
                    int years = (int)math.round(yearsRoll * yearsRoll * creaturesLibrary.speciesDict[type].maxAge);
                    float age = years;
                    GENDER gender = UnityEngine.Random.Range(0f, 1f) > 0.5f ? GENDER.MALE : GENDER.FEMALE;
                    float weight = UrthUtility.SampleGrowthCurve(age, gender == GENDER.MALE ? creaturesLibrary.speciesDict[type].maleGrowthCurve : creaturesLibrary.speciesDict[type].femaleGrowthCurve);
                    float height = 0;
                    float width = 0;
                    float length = 0;

                    CreatureData data = new CreatureData(iid, type, gender, age, weight, height, width, length, pos);

                    CreatureBody body = new CreatureBody();
                    body.data = data;
                    population[iid] = body;

                    nextID = iid;
                    iid++;
                }
            }
        }

        public Dictionary<BIOME, List<PlantData>> biomeFlora = new Dictionary<BIOME, List<PlantData>>(10);


        public void Populate()
        {

        }

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

                    foreach (CREATURE speciesType in creaturesLibrary.biomeSpecies[biomeType])
                    {
                        CreatureSpecies species = creaturesLibrary.speciesDict[speciesType];
                        float densityPerSqKm = species.biomeDensityDict[biomeType];
                        float countPerPx = densityPerSqKm * TerrainManager.TPX_AREA_KM;
                        float r = UnityEngine.Random.value;
                        if (r < countPerPx)
                        {
                            CreatureBody creatureBody = RandomCreature(speciesType, species, pos);
                            population.Add(creatureBody.data.ID, creatureBody);
                            if (!pxCreatures.ContainsKey(tileXY + pxXY))
                            {
                                pxCreatures.Add(tileXY + pxXY, new List<int>(4));
                            }
                            pxCreatures[tileXY + pxXY].Add(creatureBody.data.ID);
                        }
                    }
                }
            }
        }
        public CreatureBody RandomCreature(CREATURE type, float3 pos)
        {
            CreatureSpecies species = creaturesLibrary.speciesDict[type];
            return RandomCreature(type, species, pos);
        }

        public CreatureBody RandomCreature(CREATURE speciesType, CreatureSpecies species, float3 pos)
        {
            GENDER gen = GENDER.FEMALE;
            if (UnityEngine.Random.value < species.genderRatio)
            {
                gen = GENDER.MALE;
            }
            float ageRoll = UnityEngine.Random.value;
            float age = ageRoll * ageRoll * ageRoll * species.maxAge;
            float weight = UrthUtility.SampleGrowthCurve(age, gen == GENDER.MALE ? creaturesLibrary.speciesDict[speciesType].maleGrowthCurve : creaturesLibrary.speciesDict[speciesType].femaleGrowthCurve);
            CreatureData creatureData = new CreatureData(nextID++, speciesType, gen, age, weight, 0f, 0f, 0f, pos);
            CreatureBody body = new CreatureBody(creatureData, species.temperatureModel,pos);
            return body;
        }

        public void AddCustomCreature(CreatureBody body)
        {
            population.Add(body.ID, body);
            fastSet.Add(body.ID);
        }


        void OldUpdate()
        {
            time = UrthTime.Instance;
            if (framecount % 60 == 0)
            {
                framecount = 0;

                float fastTimeRemaining = Mathf.Min(FRAME_TIME, fastPeriod - (float)(time.totalGameSeconds - fastLast));
                neededFastSetRate = (fastSet.Count - fastIdx) / fastTimeRemaining;
                float nearbyTimeRemaining = Mathf.Min(FRAME_TIME, nearbyPeriod - (float)(time.totalGameSeconds - nearbyLast));
                neededNearbySetRate = (nearbySet.Count - nearbyIdx) / nearbyTimeRemaining;
                float regionTimeRemaining = Mathf.Min(FRAME_TIME, regionPeriod - (float)(time.totalGameSeconds - regionLast));
                neededRegionSetRate = (regionSet.Count - regionIdx) / regionTimeRemaining;
                float worldTimeRemaining = Mathf.Min(FRAME_TIME, worldPeriod - (float)(time.totalGameSeconds - worldLast));
                neededWorldSetRate = (worldSet.Count - worldIdx) / worldTimeRemaining;
            }

            if (fastIdx >= fastSet.Count)
            {//completed loop throuh fast set, perform transfers
                fastIdx = 0;
                foreach (int id in removeFromFastSet)
                {
                    fastSet.Remove(id);
                }
                foreach (int id in transferToFastSet)
                {
                    fastSet.Add(id);
                }
                fastList = fastSet.ToList();
            }
            int fastSteps = (int)math.round(neededFastSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
            for (int i = fastIdx; i < fastSteps; i++)
            {
                population[fastList[i]].Update(time.totalGameSeconds);
            }


            if (nearbyIdx >= nearbySet.Count)
            {//completed loop throuh nearby set, perform transfers
                nearbyIdx = 0;
                foreach (int id in removeFromNearbySet)
                {
                    nearbySet.Remove(id);
                }
                foreach (int id in transferToNearbySet)
                {
                    nearbySet.Add(id);
                }
                nearbyList = nearbySet.ToList();
            }
            int nearbySteps = (int)math.round(neededNearbySetRate * time.TimeRate() * FRAME_TIME + 0.5f);
            for (int i = nearbyIdx; i < nearbySteps; i++)
            {
                population[nearbyList[i]].Update(time.totalGameSeconds);
            }


            if (regionIdx >= regionSet.Count)
            {//completed loop throuh region set, perform transfers
                regionIdx = 0;
                foreach (int id in removeFromRegionSet)
                {
                    regionSet.Remove(id);
                }
                foreach (int id in transferToRegionSet)
                {
                    regionSet.Add(id);
                }
                regionList = regionSet.ToList();
            }
            int regionSteps = (int)math.round(neededRegionSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
            for (int i = regionIdx; i < regionSteps; i++)
            {
                population[regionList[i]].Update(time.totalGameSeconds);
            }


            if (worldIdx >= worldSet.Count)
            {//completed loop throuh world set, perform transfers
                worldIdx = 0;
                foreach (int id in removeFromWorldSet)
                {
                    worldSet.Remove(id);
                }
                foreach (int id in transferToWorldSet)
                {
                    worldSet.Add(id);
                }
                worldList = worldSet.ToList();
            }
            int worldSteps = (int)math.round(neededWorldSetRate * time.TimeRate() * FRAME_TIME + 0.5f);
            for (int i = worldIdx; i < worldSteps; i++)
            {
                population[worldList[i]].Update(time.totalGameSeconds);
            }
        }
    }

}