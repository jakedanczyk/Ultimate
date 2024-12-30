using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace Urth
{
    public class BillboardPrefab
    {
        public PLANT type;
        public PlantTileBillboard billboard;
    }
    public class PlantTileMacro : PlantTile
    {
        public int2 tileKey;

        //WorldGen.TerrainManager tmInstance;
        GameManager gm;
        PlantsManager plantsManager;

        //public List<PlantData> population = new List<PlantData>(1000);
        public Dictionary<int, PlantWorksiteData> populationWorksites;
        public Dictionary<int, PlantData> population;

        private void Awake()
        {
            foreach(BillboardPrefab prefab in billboardPrefabs)
            {
                billboards[prefab.type] = prefab.billboard;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        bool populated = false;
        bool generated = false;
        // Update is called once per frame
        void Update()
        {
            if (populated && !generated)
            {
                generated = true;
                MakeBillboards();
            }
        }

        /*Procedurally populate the tile by iterating all pixels
         * and checking biome conditions, and placing plants using data
         * from the PlantLibrary
         * 
         * There is an attempt to vary density and position, the idea is to
         * 'fill in' each pixel, by having a 'grid' of sub-pixels that are 
         * marked as 'occupied' as they are covered by a plant. 
         * A plant species is selected, then a specific position is chosen
         * from among the remaining non-occupied positions,
         * then the center and surrounding sub-pixels are marked as occupied
         */
        public void Populate(TerrainManager tmInstance)
        {
            populationWorksites = new Dictionary<int, PlantWorksiteData>(1000);
            population = new Dictionary<int, PlantData>(100000);
            bool[] occupied = new bool[100];
            int tileX, tileY;
            //float adjAngle;
            //int adjX, adjY;
            float3 plantPos;
            //tmInstance = WorldGen.TerrainManager.Main;
            float2 tileXY = tileKey * TerrainManager.TILE_LENGTH_M;

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
                    System.Array.Clear(occupied, 0, occupied.Length);

                    float2 pxXY = new float2(x, y) * TerrainManager.TERRAIN_PX_LENGTH_M;
                    float3 pos = new float3(tileXY.x + pxXY.x, height, tileXY.y + pxXY.y);
                    float roll = noise.snoise(pos);
                    if (roll < 0.6f) { continue; }
                    //biome-scores is an array of tuples, the values are weight and biome type
                    NativeArray<float2> biomeScores = tmInstance.GetRegionalBiomeScores(height, temp, aridity);
                    List<float2> sortedScores = new List<float2>(biomeScores.Length);
                    foreach (float2 score in biomeScores)
                    {
                        int weightsIdx = 0;
                        while (weightsIdx < sortedScores.Count && score.x < sortedScores[weightsIdx].x) { weightsIdx++; }
                        sortedScores.Insert(weightsIdx, score);
                    }

                    float accum = 0f;
                    int sortedScoresIdx = 0;
                    while (sortedScoresIdx < sortedScores.Count - 1)
                    {
                        accum += sortedScores[sortedScoresIdx].x;
                        if (roll < accum) { break; }
                        sortedScoresIdx++;
                    }

                    BIOME biome = (BIOME)(int)(sortedScores[sortedScoresIdx].y + 0.5f);
                    if (!plantsManager.plantsLibrary.biomesHaveLargePlants[biome])
                    {
                        continue;
                    }

                    //List<FloraData> floraTypes = FloraManager.Instance.biomeFlora[biomeType];
                    //int plantIdx = (int)math.round(roll * biomeScores.Length);
                    //FloraData flora = floraTypes[plantIdx];
                    //PlantData flora = new PlantData(PLANT.DOUGLAS_FIR, roll * 80f + 15f, roll * 5f + 1f, roll * 200f + 40f, pos);
                    //population.Add(flora);
                    //TODO Add individual variation to attributes, The FloraManager dict is seeded with typical specimen
                    //TODO Account for groundwater
                    //TODO Account for slope gradient and aspect

                    float expectedFillFraction = Mathf.Min(0.75f, plantsManager.plantsLibrary.biomeTotalCoverFractions[biome]);
                    float fillFraction = 0f;
                    bool notFilled = true;
                    while (notFilled)
                    {
                        float speciesRoll = UnityEngine.Random.value;
                        float rollAccumulator = 0;
                        foreach(PLANT plantType in plantsManager.plantsLibrary.biomeSpeciesFractions[biome].Keys)
                        {
                            float speciesFractionInBiome = plantsManager.plantsLibrary.biomeSpeciesFractions[biome][plantType];
                            if (speciesRoll < speciesFractionInBiome + rollAccumulator)
                            {
                                int tries = 0;
                                while(tries < 10)
                                {
                                    tries++;
                                    tileX = (int)(UnityEngine.Random.value * 10f);
                                    tileY = (int)(UnityEngine.Random.value * 10f);
                                    if(!occupied[tileX + tileY * 10])
                                    {
                                        plantPos = pos + new float3(tileX, 0, tileY);
                                        PlantData flora = plantsManager.ProceduralPlant(plantType, biome, plantPos);
                                        population.Add(flora.ID, flora);
                                        occupied[tileX + tileY * 10] = true;//TODO occupy more tiles depending on plant size
                                        break;
                                    }
                                }
                                break;
                            }
                            rollAccumulator += speciesFractionInBiome;
                        }
                        // plantsManager.plantsLibrary.biomeSpecies[biome][speciesSelectionIdx];
                        
                        if (fillFraction > expectedFillFraction)
                        {
                            
                        }
                    }
                }
            }
            populated = true;
        }


        Dictionary<int2, PlantTileMeso> mesoTiles;
        public Dictionary<int2, PlantTileMeso> GenerateMesoTiles()
        {

            for(int x = 0; x < TerrainManager.TILE_LENGTH_KM*1000; x += PlantsManager.MESO_TILE_LENGTH_M)
            {
                for (int y = 0; y < TerrainManager.TILE_LENGTH_KM * 1000; y += PlantsManager.MESO_TILE_LENGTH_M)
                {
                    //create new meso tile
                    Vector3 pos = new Vector3(x, 0, y);
                    PlantTileMeso mesoTile = Instantiate(plantsManager.mesoTilePrefab, pos, Quaternion.identity, this.transform).GetComponent<PlantTileMeso>();
                    int2 mesoTileKey = new int2(x, y) + TerrainManager.TILE_LENGTH_KM * 1000 * tileKey;
                    mesoTiles.Add(mesoTileKey, mesoTile);
                }
            }
            //iterate through population, add to population for appropriate meso tile
            foreach(int id in population.Keys)
            {
                PlantData plantData = population[id];
                int mesoX = (int)(plantData.pos.x * PlantsManager.MESO_TILE_LENGTH_M) / PlantsManager.MESO_TILE_LENGTH_M;
                int mesoY = (int)(plantData.pos.y * PlantsManager.MESO_TILE_LENGTH_M) / PlantsManager.MESO_TILE_LENGTH_M;
                int2 mesoKey = new int2(mesoX, mesoY);
                PlantTileMeso mesoTile = mesoTiles[mesoKey];
                mesoTile.population.Add(id);
            }
            //generate billboards for each meso tile
            foreach (int2 key in mesoTiles.Keys)
            {
                mesoTiles[key].MakeBillboards();
            }
            return mesoTiles;
        }

        public void UnloadMeso()
        {
            foreach(int2 mesoTileKey in mesoTiles.Keys)
            {
                PlantTileMeso mesoTile = mesoTiles[mesoTileKey];
                Destroy(mesoTile.gameObject);
            }
        }


        public void ClearBillboards()
        {
            foreach(PLANT type in billboards.Keys)
            {
                PlantTileBillboard plantTileBillboard = billboards[type];
                plantTileBillboard.Clear();
            }
        }

        //public bool isGenerating, beenDetailed;
        //public List<GameObject> trees = new List<GameObject>();

        //public Shader shader;
        //public MeshRenderer coniferBillboard;
        //public MeshRenderer palmBillboard;
        //public MeshRenderer broadleafBillboard;
        //List<MeshFilter> meshFilters = new List<MeshFilter>();
        //Mesh mesh;
        //GameObject selectedTreeBillboard;
        //GameObject selectedTreePrefab;
        //List<Vector3> treePosList = new List<Vector3>();

        public void MakeBillboards()
        {
            //mesh = new Mesh();
            //GetComponent<MeshFilter>().mesh = mesh;
            //mesh.Clear();

            // hard coded seed so the mesh comes out the same always
            //TerrainGen terrainGen = new TerrainGen();
            //if (transform.position.y > 3000)
            //{
            //    meshRenderer.material.mainTexture = world.coniferBillboard;
            //    meshRenderer.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });
            //}
            //else
            //{
            //    meshRenderer.material.mainTexture = world.broadleafBillboard;
            //    meshRenderer.material.SetFloatArray("_Cells", new float[] { 2, 3, 0, 0 });
            //}

            //coniferBillboard.material.mainTexture = PlantsManager.Instance.coniferBillboard;
            //coniferBillboard.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });
            //palmBillboard.material.mainTexture = PlantsManager.Instance.palmBillboard;
            //palmBillboard.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });
            //broadleafBillboard.material.mainTexture = PlantsManager.Instance.broadleafBillboard;
            //broadleafBillboard.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });


            int2 tileWorldPosXY = tileKey * TerrainManager.TILE_LENGTH_M;
            float3 tileWorldPos = new float3(tileWorldPosXY.x, 0, tileWorldPosXY.y);
            int2 tileGamePosXY = (tileKey - GameManager.Instance.gameOriginCell) * TerrainManager.TILE_LENGTH_M;
            float3 tileGamePos = new float3(tileGamePosXY.x, 0, tileGamePosXY.y);

            foreach (int id in population.Keys)
            {
                PlantData plant = population[id];
                PlantTileBillboard billboard = billboards[plant.type];

                float height = plant.height;
                float width = height / plantsManager.plantsLibrary.speciesDict[plant.type].slenderness;

                if (height + width < 5f) { continue; }

                float3 meshPosf3 = plant.pos - tileWorldPos;
                Vector3 meshPos = new Vector3(meshPosf3.x, meshPosf3.y, meshPosf3.z);
                //float x = Random.Range(-128, 128);
                //float z = Random.Range(-128, 128);
                //var s = new Vector3(x, terrainGen.DirtHeight(transform.position.x + x, 0, transform.position.z + z) - transform.position.y, z);
                //treePosList.Add(s);

                // 4 vertices representing the shape of a quad
                var v0 = new Vector3(0, 0, 0);
                var v1 = new Vector3(0, height, 0);
                var v2 = new Vector3(width, height, 0);
                var v3 = new Vector3(width, 0, 0);

                billboard.vertices.Add(v1 + meshPos);
                billboard.vertices.Add(v2 + meshPos);
                billboard.vertices.Add(v3 + meshPos);

                // add local position to mesh to make it easier in the shader
                billboard.localPos.Add(new Vector2(v0.x, v0.y));
                billboard.localPos.Add(new Vector2(v1.x, v1.y));
                billboard.localPos.Add(new Vector2(v2.x, v2.y));
                billboard.localPos.Add(new Vector2(v3.x, v3.y));

                // add tex-coords
                var uv0 = new Vector2(0, 1);
                var uv1 = new Vector2(0, 0);
                var uv2 = new Vector2(1, 0);
                var uv3 = new Vector2(1, 1);

                billboard.uvs.Add(uv0);
                billboard.uvs.Add(uv1);
                billboard.uvs.Add(uv2);
                billboard.uvs.Add(uv3);

                // add random starting frame index for each billboard
                // 8*8 assumes the texture contains 8 columns and 8 rows
                var frameIndex = new Vector2(UnityEngine.Random.Range(0, 6), 0);
                billboard.frameIndices.Add(frameIndex);
                billboard.frameIndices.Add(frameIndex);
                billboard.frameIndices.Add(frameIndex);
                billboard.frameIndices.Add(frameIndex);

                // left triangle of quad
                billboard.triangles.Add(billboard.triangleIndex + 0);
                billboard.triangles.Add(billboard.triangleIndex + 1);
                billboard.triangles.Add(billboard.triangleIndex + 2);

                // right triangle of quad
                billboard.triangles.Add(billboard.triangleIndex + 2);
                billboard.triangles.Add(billboard.triangleIndex + 3);
                billboard.triangles.Add(billboard.triangleIndex + 0);

                billboard.triangleIndex += 4;
            }

            foreach(PLANT type in billboards.Keys)
            {
                billboards[type].Render();
            }
        }
    }
}