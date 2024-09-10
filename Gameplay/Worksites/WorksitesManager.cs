using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public class WorksitesManager : MonoBehaviour
    {
        int nextID = 0;
        public int GetNextId()
        {
            return nextID++;
        }
        public TerrainBuilder terrainBuilder;
        public TerrainManager terrainManager;
        public GameObject terrainWorksitePrefab;

        public Dictionary<int3, TerrainWorksiteData> terrainWorksitesData;
        public Dictionary<int3, TerrainWorksite> terrainWorksites;

        public Dictionary<int, ConstructionWorksite> constructionWorksites;
        public static WorksitesManager Instance { get; private set; }
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
        }

        // Start is called before the first frame update
        void Start()
        {
            terrainWorksitesData = new Dictionary<int3, TerrainWorksiteData>();
            terrainWorksites = new Dictionary<int3, TerrainWorksite>();
            constructionWorksites = new Dictionary<int, ConstructionWorksite>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public float WorktaskRating(WORKTASK worktask, CreatureStats stats, UItem tool)
        {
            switch (worktask) 
            {
                case (WORKTASK.MINE):
                    float fAttrib = stats.statInterface.statCollection.GetStat(StatType.Strength).StatValue
                        + 0.2f * stats.statInterface.statCollection.GetStat(StatType.Balance).StatValue
                        + 0.2f * stats.statInterface.statCollection.GetStat(StatType.Coordination).StatValue
                        + 0.1f * stats.statInterface.statCollection.GetStat(StatType.Agility).StatValue
                        + 0.05f * stats.statInterface.statCollection.GetStat(StatType.Toughness).StatValue
                        + 0.05f * stats.statInterface.statCollection.GetStat(StatType.Speed).StatValue
                        + 0.05f * stats.statInterface.statCollection.GetStat(StatType.Dexterity).StatValue
                        + 0.05f * stats.statInterface.statCollection.GetStat(StatType.Reflex).StatValue;
                    float fSkill = 0.8f * stats.statInterface.statCollection.GetStat(StatType.Mining).StatValue + 0.2f * stats.statInterface.statCollection.GetStat(StatType.StoneWorking).StatValue;
                    float fItem = 0f;// tool.
                    return fSkill * fAttrib * fItem;
                //case (WORKTASK.EARTHBREAKING):
                //    return 0.5f * stats.statInterface.statCollection.GetStat(StatType.Mining).StatValue + 0.5f * stats.statInterface.statCollection.GetStat(StatType.Digging).StatValue;
                case (WORKTASK.DIG):
                    return 0.1f * stats.statInterface.statCollection.GetStat(StatType.Mining).StatValue + 0.9f * stats.statInterface.statCollection.GetStat(StatType.Digging).StatValue;
                default: return 1f;
            }
        }
        public float WorktaskAttribRating(WORKTASK worktask, CreatureStats stats, UItem tool)
        {
            switch (worktask)
            {
                //case (WORKTASK.STONEBREAKING):
                //    return 0.8f * stats.statInterface.statCollection.GetStat(StatType.Mining).StatValue + 0.2f * stats.statInterface.statCollection.GetStat(StatType.StoneWorking).StatValue;
                //case (WORKTASK.EARTHBREAKING):
                //    return 0.5f * stats.statInterface.statCollection.GetStat(StatType.Mining).StatValue + 0.5f * stats.statInterface.statCollection.GetStat(StatType.Digging).StatValue;
                case (WORKTASK.DIG):
                    return 0.1f * stats.statInterface.statCollection.GetStat(StatType.Mining).StatValue + 0.9f * stats.statInterface.statCollection.GetStat(StatType.Digging).StatValue;
                default: return 1f;
            }
        }

        public TerrainWorksite GetTerrainWorksite(Vector3 pos)
        {
            int3 posInt = TerrainBuilder.Instance.GetFollowGridInt(pos);
            return GetTerrainWorksite(posInt);
        }
        public TerrainWorksite GetTerrainWorksite(int3 pos)
        {
            if (terrainWorksites.ContainsKey(pos))
            {
                return terrainWorksites[pos];
            }
            else if (terrainWorksitesData.ContainsKey(pos))
            {
                TerrainWorksiteData data = terrainWorksitesData[pos];
                TerrainWorksite newTerrainWorksite = Instantiate(terrainWorksitePrefab).GetComponent<TerrainWorksite>();
                newTerrainWorksite.transform.position = new Vector3(pos.x,pos.y,pos.z);
                newTerrainWorksite.SetIndicator(true);
                newTerrainWorksite.data = data;
                return newTerrainWorksite;
            }
            else
            {
                //determine voxel contents. if near surface, use terrain surface profile. Else use actual voxel types
                //Create worksite data.
                TerrainWorksiteData data = CreateTerrainWorksiteData(pos);
                TerrainWorksite newTerrainWorksite = Instantiate(terrainWorksitePrefab).GetComponent<TerrainWorksite>();
                newTerrainWorksite.transform.position = new Vector3(pos.x, pos.y, pos.z);
                newTerrainWorksite.SetIndicator(true);
                newTerrainWorksite.data = data;
                return newTerrainWorksite;
            }
            //TerrainWorksiteData worksiteData = terrainWorksitesData.ContainsKey(pos) ? terrainWorksitesData[pos] : new TerrainWorksiteData(GetNextId());
        }

        public TerrainWorksiteData CreateTerrainWorksiteData(int3 pos) 
        {
            float terrainSurfaceHeight = (float)terrainManager.GetTerrainHeight(pos);
            float soilDepth = (float)terrainManager.GetSoilDepth(pos);
            float depth = (float)terrainSurfaceHeight - pos.y;
            if(depth+1f < soilDepth)
            {//surface worksite, in soil horizons,
                TerrainBlock terrainBlock = TerrainBuilder.Instance.GetSurfaceTerrainBlock(pos, depth, soilDepth, terrainSurfaceHeight);
                TerrainWorksiteData worksiteData = new TerrainWorksiteData(GetNextId(), true, pos);
                worksiteData.terrainBlock = terrainBlock;
                return worksiteData;
            }
            else
            {//bedrock terrain. 
                TerrainBlock terrainBlock = TerrainBuilder.Instance.GetDeepTerrainBlock(pos);
                TerrainWorksiteData worksiteData = new TerrainWorksiteData(GetNextId(), false, pos);
                worksiteData.terrainBlock = terrainBlock;
                return worksiteData;
            }
        }

        public void DoTerrainWork(Vector3 pos)
        {
            Vector3 voxelPos = TerrainBuilder.Instance.GetFollowGrid(pos);
        }

        public TerrainWorksiteData CreateConstructionWorksiteData(int3 pos)
        {
            float terrainSurfaceHeight = (float)terrainManager.GetTerrainHeight(pos);
            float soilDepth = (float)terrainManager.GetSoilDepth(pos);
            float depth = (float)terrainSurfaceHeight - pos.y;
            if (depth + 1f < soilDepth)
            {//surface worksite, in soil horizons,
                TerrainBlock terrainBlock = TerrainBuilder.Instance.GetSurfaceTerrainBlock(pos, depth, soilDepth, terrainSurfaceHeight);
                TerrainWorksiteData worksiteData = new TerrainWorksiteData(GetNextId(), true, pos);
                worksiteData.terrainBlock = terrainBlock;
                return worksiteData;
            }
            else
            {//bedrock terrain. 
                TerrainBlock terrainBlock = TerrainBuilder.Instance.GetDeepTerrainBlock(pos);
                TerrainWorksiteData worksiteData = new TerrainWorksiteData(GetNextId(), false, pos);
                worksiteData.terrainBlock = terrainBlock;
                return worksiteData;
            }
        }
    }
}