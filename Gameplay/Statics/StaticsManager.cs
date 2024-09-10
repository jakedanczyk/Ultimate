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

    public class StaticsManager : MonoBehaviour
    {
        public StaticsLibrary staticsLibrary;
        public UrthTime time;
        int nextID = 0;
        public int GetNextId()
        {
            return nextID++;
        }
        public List<int> worldList;
        public List<int> regionList;
        public List<int> nearbyList;
        public List<int> fastList;


        public HashSet<int> worldSet; //all objects loaded into memory
        public HashSet<int> regionSet; //objects in billboard range
        public HashSet<int> nearbySet; //objects in low-poly range
        public HashSet<int> fastSet; //objects loaded in full detail

        public Dictionary<string, UrthStatic> constructions;
        public Dictionary<string, UrthStatic> population;
        public Dictionary<string, UrthStatic> spawnedStatics;

        public List<int> unbuiltList;
        public Dictionary<int, StaticPrefab> prefabPopulation;

        public List<int> shouldDespawnList;

        public static StaticsManager Instance { get; private set; }
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
            spawnQueue = new Queue<string>();
            despawnList = new List<int>();
            population = new Dictionary<string, UrthStatic>(10000);
            worldSet = new HashSet<int>();
            fastSet = new HashSet<int>();

            unbuiltList = new List<int>();
            prefabPopulation = new Dictionary<int, StaticPrefab>(10000);
        }

        // Start is called before the first frame update
        void Start()
        {
            time = UrthTime.Instance;

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
                //scan tiles around player to see if statics should spawn in
                List<int> spawnIds = new List<int>();// GetShouldSpawnList();
                foreach (int id in spawnIds)
                {
                    if (fastSet.Contains(id))
                    {
                        Debug.Log("Statics Already Spawned");
                        continue;
                    }

                    //StaticData spawnItemData = population[id];
                    //GameObject prefab = spawnItemData.assembled ? staticsLibrary.prefabsDict[spawnItemData.type].placement : staticsLibrary.prefabsDict[spawnItemData.type].preview;
                    //GameObject instance = Instantiate(prefab, spawnItemData.pos, Quaternion.Euler(spawnItemData.facing));
                    //Static item = instance.AddComponent<Static>();
                    //item.data = spawnItemData;
                    //spawnItemData.item = item;
                    //spawnedStatics.Add(id, item);
                    //fastSet.Add(id);
                    //worldSet.Remove(id);
                }

                foreach (int id in shouldDespawnList)
                {
                    //fastSet.Remove(id);
                    //worldSet.Add(id);
                    //spawnedStatics[id].data = null;
                    //Destroy(spawnedStatics[id].gameObject);
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


        //public List<int> GetShouldSpawnList()
        //{//TODO use 3d distance. there are cliffs
        //    List<int> ids = new List<int>(1000);
        //    float3 playerPos = GameManager.Instance.GetPlayerWorldPos();
        //    int2 playerPxIdx = 10 * new int2((int)(playerPos.x / 10f), (int)(playerPos.z / 10f));
        //    for (int y = -(int)SPAWN_RADIUS; y < SPAWN_RADIUS; y += 10)
        //    {
        //        int xr = (int)(SPAWN_RADIUS * Mathf.Cos((y) / SPAWN_RADIUS * Mathf.PI / 2));
        //        for (int x = -xr; x < xr; x += 10)
        //        {
        //            if (pxCreatures.TryGetValue(playerPxIdx + new int2(x, y), out List<int> pxIds))
        //            {
        //                ids.AddRange(pxIds);
        //            }
        //        }
        //    }
        //    return ids;
        //}


        Queue<string> spawnQueue;
        void SpawnItems()
        {
            foreach (string id in spawnQueue)
            {
                SpawnStatic(id);
            }
            spawnQueue.Clear();
        }
        void SpawnStatic(string id)
        {
            UrthStatic spawnItemData = population[id];
            //GameObject prefab = spawnItemData.assembled ? staticsLibrary.prefabsDict[spawnItemData.type].finalPrefab : staticsLibrary.prefabsDict[spawnItemData.type].wipPrefab;
            //float3 gamePosition = spawnItemData.pos - GameManager.Instance.gameWorldOffset;
            //GameObject instance = Instantiate(prefab, gamePosition, Quaternion.Euler(spawnItemData.facing));
        }
        public int SpawnNewStatic(USTATIC type, float size, Vector3 pos, Vector3 rot, bool isConstruction=false, List<int> supportedByIds=null)
        {
            var newObject = Instantiate(StaticsLibrary.Instance.prefabsDict[type], pos, Quaternion.Euler(rot));
            StaticPrefab staticPrefab = newObject.gameObject.GetComponent<StaticPrefab>();
            staticPrefab.id = GetNextId();
            prefabPopulation.Add(staticPrefab.id, staticPrefab);
            //string newId = type.ToString() + GetNextId().ToString();
            //UrthStatic newUrthStatic = new UrthStatic(newId, "", type, size, isConstruction);
            if (isConstruction)
            {
                newObject.GetComponent<StaticPrefab>().SetAsWip();
                unbuiltList.Add(staticPrefab.id);
                if(supportedByIds != null)
                {
                    staticPrefab.supportingStatics = supportedByIds;
                }
                //ConstructionManager.Instance.CreateNewConstructionWorksite(staticPrefab);
            }
            return staticPrefab.id;
        }

        public void ToggleConstructionHUD(bool show)
        {
            foreach(int id in unbuiltList)
            {
                prefabPopulation[id].gameObject.SetActive(show);
            }
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
            //GameObject go = spawnedStatics[id].gameObject;
            //Destroy(go);
        }


      


        public void AddCustomStatic(UrthStatic data)
        {
            population.Add(data.id, data);
        }
    }

}