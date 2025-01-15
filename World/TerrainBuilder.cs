using System.Collections;
using System.Collections.Generic;
using UltimateTerrains;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{

    public class TerrainSurfaceProfile
    {
        public float bottomAltitude;
        public float snow;
        public float litter;
        public float turf;
        public float peat;
        public float topsoil;
        public float subsoil;
        public float substrate;

        public TerrainSurfaceProfile(float ibottomHeight, float isnow, float ilitter, float iturf, float ipeat, float itopsoil, float isubsoil, float isubstrate)
        {
            bottomAltitude = ibottomHeight;
            snow = isnow; litter = ilitter; turf = iturf; peat = ipeat; topsoil = itopsoil; subsoil = isubsoil; substrate = isubstrate;
        }


    }

    public class TerrainBuilder : MonoBehaviour
    {
        public TerrainManager terrainManager;
        public UltimateTerrain Terrain;
        public Dictionary<int2, TerrainSurfaceProfile> surfaceProfiles;

        public static TerrainBuilder Instance { get; private set; }
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

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int3 GetFollowGridInt(Vector3 worldPosition)
        {
            Vector3i v3i = Terrain.Converter.UnityToVoxelPositionRound(worldPosition);
            int3 i3 = new int3(v3i.x, v3i.y, v3i.z);
            return i3;
        }

        public Vector3 GetFollowGrid(Vector3 worldPosition)
        {
            return (Vector3)Terrain.Converter.VoxelToUnityPosition(Terrain.Converter.UnityToVoxelPositionRound(worldPosition));

        }

        public TerrainBlock GetSurfaceTerrainBlock(int3 pos, float surfaceDepth, float surfaceAltitude)
        {
            int2 pos2d = new int2(pos.x, pos.z);
            if (!surfaceProfiles.ContainsKey(pos2d))
            {//surface profile not created yet, create it now
                //float totalDepth = (float)TerrainManager.Main.GetSoilDepth(pos);
                float turfDepth = surfaceDepth < 0.1f ? 0f : 0.1f;
                float soilDepth = surfaceDepth - turfDepth;
                float topSoilDepth = soilDepth * 0.125f;
                float subSoilDepth = soilDepth * 0.375f;
                float substrateDepth = soilDepth * 0.5f;

                surfaceProfiles[pos2d] = new TerrainSurfaceProfile(surfaceAltitude - surfaceDepth, 0f, 0f, turfDepth, 0f, topSoilDepth, subSoilDepth, substrateDepth);
                int depth = 0;
                while(depth < surfaceDepth)
                {

                    depth++;
                }
            }
            TerrainSurfaceProfile tsp = surfaceProfiles[pos2d];

            List<Voxel> currentVoxelVals = ScanVoxel(pos);

            TerrainBlock tb = new TerrainBlock(isurface:true);
            float topHeight = pos.y + 0.5f;//Mathf.Min(pos.y+1, surfaceAltitude);
            float botHeight = pos.y - 0.5f;
            float fillThickness = 0;
            float fillHeight = botHeight;
            //find first layer
            if (botHeight < tsp.bottomAltitude)
            {//bottom in bedrock, sample bottom corners for first layer
                float rockLayerThickness = tsp.bottomAltitude - botHeight;
                string voxelTypeName = Terrain.VoxelTypeSet.SerializableVoxelTypes[currentVoxelVals[0].VoxelTypeIndex].Name;
                UMATERIAL rockType = MaterialsLibrary.Instance.voxelMats[voxelTypeName];
                tb.fractions.Add(new TerrainBlockFraction(rockType, rockLayerThickness, 0f));

                fillThickness = rockLayerThickness;
                fillHeight += fillThickness;
            }
            if(fillHeight < tsp.bottomAltitude + tsp.substrate)
            {//substrate
                float substrateLayerThickness = Mathf.Min(1f - fillThickness, (tsp.bottomAltitude + tsp.substrate) - fillHeight);
                tb.fractions.Add(new TerrainBlockFraction(UMATERIAL.SUBSTRATE, substrateLayerThickness, 0f));

                fillThickness += substrateLayerThickness;
                fillHeight += substrateLayerThickness;
            }
            if(fillHeight < tsp.bottomAltitude + tsp.substrate + tsp.subsoil)
            {//subsoil
                float subsoilLayerThickness = Mathf.Min(1f - fillThickness, (tsp.bottomAltitude + tsp.substrate + tsp.subsoil) - fillHeight);
                tb.fractions.Add(new TerrainBlockFraction(UMATERIAL.SUBSOIL, subsoilLayerThickness, 0f));

                fillThickness += subsoilLayerThickness;
                fillHeight += subsoilLayerThickness;
            }
            if (fillHeight < tsp.bottomAltitude + tsp.substrate + tsp.subsoil + tsp.topsoil)
            {//topsoil
                float topsoilLayerThickness = Mathf.Min(1f - fillThickness, (tsp.bottomAltitude + tsp.substrate + tsp.subsoil + tsp.topsoil) - fillHeight);
                tb.fractions.Add(new TerrainBlockFraction(UMATERIAL.TOPSOIL, topsoilLayerThickness, 0f));

                fillThickness += topsoilLayerThickness;
                fillHeight += topsoilLayerThickness;
            }
            if (fillHeight < tsp.bottomAltitude + tsp.substrate + tsp.subsoil + tsp.topsoil + tsp.turf)
            {//turf
                float turfLayerThickness = Mathf.Min(1f - fillThickness, (tsp.bottomAltitude + tsp.substrate + tsp.subsoil + tsp.topsoil + tsp.turf) - fillHeight);
                tb.fractions.Add(new TerrainBlockFraction(UMATERIAL.TOPSOIL, turfLayerThickness, 0f));

                fillThickness += turfLayerThickness;
                fillHeight += turfLayerThickness;
            }

            return tb;


        }

        public TerrainBlock GetEmptyTerrainBlock(int3 pos)
        {
            TerrainBlock tb = new TerrainBlock(isurface: false);
            return tb;
        }

        public TerrainBlock GetDeepTerrainBlock(int3 pos)
        {
            int2 pos2d = new int2(pos.x, pos.z);

            List<Voxel> currentVoxelVals = ScanVoxel(pos);

            TerrainBlock tb = new TerrainBlock(isurface: false);
            foreach(Voxel voxel in currentVoxelVals)
            {
                string voxelTypeName = Terrain.VoxelTypeSet.SerializableVoxelTypes[currentVoxelVals[0].VoxelTypeIndex].Name;
                UMATERIAL type = MaterialsLibrary.Instance.voxelMats[voxelTypeName];
                tb.fractions.Add(new TerrainBlockFraction(type, .125f, 0f));
            }
            return tb;
        }

        public List<Voxel> ScanVoxel(int3 pos)
        {
            List<Voxel> vals = Terrain.GetVoxelValues(pos.x, pos.y, pos.z);
            return vals;
        }
        public void Dig(int3 pos)
        {
            if (Terrain.OperationsManager.IsReadyToComputeAsync)
            {
                Terrain.OperationsManager
                       .Add(AxisAlignedCube.CreateFromUnityWorld(Terrain, true, new Vector3(pos.x, pos.y, pos.z), Vector3.one, null), true)
                       .PerformAll(true);
            }
        }
    }

}