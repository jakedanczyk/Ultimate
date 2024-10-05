using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class ConstructionManager : MonoBehaviour
    {
        public ConstructionLibrary constructionLibrary;
        public UrthTime time;
        int nextID = 0;
        public int GetNextId()
        {
            return nextID++;
        }

        Queue<string> spawnQueue;
        List<int> despawnList;
        public Dictionary<string, UrthConstruction> population;
        public Dictionary<string, UrthConstruction> spawnedStatics;

        public HashSet<int> worldSet;
        public HashSet<int> fastSet;
        public static ConstructionManager Instance { get; private set; }
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
            population = new Dictionary<string, UrthConstruction>(10000);
            worldSet = new HashSet<int>();
            fastSet = new HashSet<int>();
        }

        void Start()
        {
            time = UrthTime.Instance;

        }

        public void CreateConstruction(ConstructionPreview preview, Vector3 pos, List<int> supportedByIds = null)
        {
            int staticId = StaticsManager.Instance.SpawnNewStatic(preview.staticType, preview.constructionWorksite.size, pos, new Vector3(0, preview.constructionWorksite.rotation, 0), isConstruction: true, supportedByIds);
            StaticPrefab staticPrefab = StaticsManager.Instance.prefabPopulation[staticId];
            ConstructionWorksite constructionWorksite = staticPrefab.gameObject.GetComponent<ConstructionWorksite>();
            constructionWorksite.CopyValuesFromPreview(preview.constructionWorksite);
            switch (staticPrefab.staticType)
            {
                case USTATIC.FOUNDATION_LOG_PIER_LOG:
                    //ConstructionPreviewPillarSet constructionPreviewPillarSet = staticPrefab.GetComponent<ConstructionPreviewPillarSet>();
                    //CopyPreviewPillarSet(preview.pillarSet, constructionPreviewPillarSet);
                    //constructionPreviewPillarSet.UpdatePillars();
                    //ConstructionPillarSet constructionPillarSet = staticPrefab.GetComponent<ConstructionPillarSet>();
                    //CopyPillarSet(preview.pillarSet, constructionPillarSet);
                    //constructionPillarSet.UpdatePillars();

                    LogPierFoundationConstruction logPierFoundationConstruction = staticPrefab.gameObject.GetComponent(typeof(LogPierFoundationConstruction)) as LogPierFoundationConstruction;
                    //LogConstruction logConstructionWorksite = staticPrefab.gameObject.GetComponent(typeof(LogConstruction)) as LogConstruction;
                    //logConstructionWorksite.logs = new List<(float, float, float)>();
                    //float logDiameter = 0.25f * size;
                    //float beamLengths = 2f * size;
                    //logConstructionWorksite.logs.Add((logDiameter, beamLengths, 4f));
                    //logConstructionWorksite.logs.Add((logDiameter, constructionPillarSet.pillars[0].transform.localScale.y, 1f));
                    //logConstructionWorksite.logs.Add((logDiameter, constructionPillarSet.pillars[1].transform.localScale.y, 1f));
                    //logConstructionWorksite.logs.Add((logDiameter, constructionPillarSet.pillars[2].transform.localScale.y, 1f));
                    //logConstructionWorksite.logs.Add((logDiameter, constructionPillarSet.pillars[3].transform.localScale.y, 1f));
                    //int numLogsNeeded = (int)(Mathf.Ceil(logConstructionWorksite.PrimaryVolume() / UrthConstants.LOG_VOLUME));
                    //logConstructionWorksite.supplyCount = numLogsNeeded;
                    //logConstructionWorksite.supplies.Add(new SupplyCount(numLogsNeeded, 0, ITEM.LOG));
                    //Debug.Log(logConstructionWorksite.supplyCount);
                    
                    //LogPierFoundationConstruction constructionWorksite = StaticsManager.Instance.prefabPopulation[staticId].gameObject.AddComponent(typeof(ConstructionWorksite)) as ConstructionWorksite;
                    logPierFoundationConstruction.staticPrefab = staticPrefab;
                    logPierFoundationConstruction.staticPrefab.inventory = new UInventory();
                    logPierFoundationConstruction.pierHeights = ((LogPierFoundationConstruction)(constructionLibrary.constructionPlayer.currentPreview.constructionWorksite)).pierHeights;
                    logPierFoundationConstruction.UpdateSuppliesNeeded();
                    logPierFoundationConstruction.UpdateComponentsFinal();
                    break;
            }
            constructionWorksite.UpdateSuppliesNeeded();
            constructionWorksite.UpdateComponentsFinal();
        }

        public void CreateNewConstructionWorksite(StaticPrefab staticPrefab)
        {
            ConstructionWorksite constructionWorksite = staticPrefab.gameObject.AddComponent(typeof(ConstructionWorksite)) as ConstructionWorksite;
            constructionWorksite.staticPrefab = staticPrefab; ;
        }

        void CopyPreviewPillarSet(ConstructionPreviewPillarSet fromPillarSet, ConstructionPreviewPillarSet toPillarSet)
        {
            toPillarSet.supported = fromPillarSet.supported;
            toPillarSet.pillarMinLength = fromPillarSet.pillarMinLength;
            toPillarSet.pillarMaxHeight = fromPillarSet.pillarMaxHeight;
            toPillarSet.pillarMaxScale = fromPillarSet.pillarMaxScale;
            toPillarSet.arePillarsSupported = new List<bool>(fromPillarSet.arePillarsSupported);
            toPillarSet.pillarSupportHeights = new List<float>(fromPillarSet.pillarSupportHeights);
            toPillarSet.radius = fromPillarSet.radius;
        }
        void CopyPillarSet(ConstructionPreviewPillarSet fromPillarSet, ConstructionPillarSet toPillarSet)
        {
            toPillarSet.supported = fromPillarSet.supported;
            //toPillarSet.pillarMinLength = fromPillarSet.pillarMinLength;
            toPillarSet.pillarMaxHeight = fromPillarSet.pillarMaxHeight;
            toPillarSet.pillarMaxScale = fromPillarSet.pillarMaxScale;
            toPillarSet.arePillarsSupported = new List<bool>(fromPillarSet.arePillarsSupported);
            toPillarSet.pillarSupportHeights = new List<float>(fromPillarSet.pillarSupportHeights);
            toPillarSet.radius = fromPillarSet.radius;
        }
    }

}