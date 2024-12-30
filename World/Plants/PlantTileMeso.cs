using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{

    public class PlantTileMeso : PlantTile
    {
        public float3 worldPos;
        public PlantsManager plantsManager;
        public PlantTileMacro parentTile;
        public HashSet<int> population;

        public Shader shader;
        public MeshRenderer meshRenderer;

        private void Awake()
        {
            foreach(BillboardPrefab billboardPrefab in billboardPrefabs)
            {
                billboards[billboardPrefab.type] = billboardPrefab.billboard;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }


        bool generated = false;
        // Update is called once per frame
        void Update()
        {
            if(!generated)
            {
                generated = true;
                MakeBillboards();
            }
        }

        Dictionary<int2, PlantTileMicro> microTiles;
        public Dictionary<int2, PlantTileMicro> InitMicroTiles()
        {
            for (int x = 0; x < TerrainManager.TILE_LENGTH_KM * 1000; x += PlantsManager.MICRO_TILE_LENGTH_M)
            {
                for (int z = 0; z < TerrainManager.TILE_LENGTH_KM * 1000; z += PlantsManager.MICRO_TILE_LENGTH_M)
                {
                    //create new micro tile
                    float3 microPos = new float3(x, worldPos.y, z);
                    PlantTileMicro plantTileMicro = Instantiate(plantsManager.microTilePrefab, microPos, Quaternion.identity).GetComponent<PlantTileMicro>();
                    plantTileMicro.worldPos = microPos;
                    plantTileMicro.plantsManager = plantsManager;
                    plantTileMicro.parentTile = this.parentTile;
                    plantTileMicro.parentMesoTile = this;

                    microTiles[new int2(x, z)] = plantTileMicro;
                }
            }
            return microTiles;
        }

        public Dictionary<int2, PlantTileMicro> GenerateMicroTiles()
        {
            for (int x = 0; x < TerrainManager.TILE_LENGTH_KM * 1000; x += PlantsManager.MICRO_TILE_LENGTH_M)
            {
                for (int z = 0; z < TerrainManager.TILE_LENGTH_KM * 1000; z += PlantsManager.MICRO_TILE_LENGTH_M)
                {
                    //create new micro tile
                    float3 microPos = new float3(x, worldPos.y, z);
                    PlantTileMicro plantTileMicro = Instantiate(plantsManager.microTilePrefab,microPos, Quaternion.identity).GetComponent<PlantTileMicro>();
                    plantTileMicro.worldPos = microPos;
                    plantTileMicro.plantsManager = plantsManager;
                    plantTileMicro.parentTile = this.parentTile;
                    plantTileMicro.parentMesoTile = this;

                    microTiles[new int2(x, z)] = plantTileMicro;
                }
            }
            //iterate through population, add to population for appropriate micro tile
            foreach (int id in population)
            {
                PlantData plantData = parentTile.population[id];
                int microX = (int)(plantData.pos.x * PlantsManager.MICRO_TILE_LENGTH_M) / PlantsManager.MICRO_TILE_LENGTH_M;
                int microY = (int)(plantData.pos.y * PlantsManager.MICRO_TILE_LENGTH_M) / PlantsManager.MICRO_TILE_LENGTH_M;
                int2 microKey = new int2(microX, microY);
                PlantTileMicro microTile = microTiles[microKey];
                microTile.population.Add(id);
            }
            //generate billboards for each micro tile
            return microTiles;
        }

        public void MakeBillboards()
        {
            //coniferBillboard.material.mainTexture = PlantsManager.Instance.coniferBillboard;
            //coniferBillboard.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });
            //palmBillboard.material.mainTexture = PlantsManager.Instance.palmBillboard;
            //palmBillboard.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });
            //broadleafBillboard.material.mainTexture = PlantsManager.Instance.broadleafBillboard;
            //broadleafBillboard.material.SetFloatArray("_Cells", new float[] { 4, 2, 0, 0 });


            //int2 tileWorldPosXY = tileKey * TerrainManager.TILE_LENGTH_M;
            //float3 tileWorldPos = new float3(tileWorldPosXY.x, 0, tileWorldPosXY.y);
            //int2 tileGamePosXY = (tileKey - GameManager.Instance.gameOriginCell) * TerrainManager.TILE_LENGTH_M;
            //float3 tileGamePos = new float3(tileGamePosXY.x, 0, tileGamePosXY.y);

            foreach (int id in population)
            {
                PlantData plant = parentTile.population[id];
                PlantTileBillboard billboard = billboards[plant.type];

                float height = plant.height;
                float width = height / plantsManager.plantsLibrary.speciesDict[plant.type].slenderness;

                if (height + width < 5f) { continue; }

                float3 meshPosf3 = plant.pos - worldPos;
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

            foreach (PLANT type in billboards.Keys)
            {
                billboards[type].Render();
            }
            generated = true;
        }
    }
}