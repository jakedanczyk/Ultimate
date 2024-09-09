using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class TerrainWorksiteHUD : MonoBehaviour
    {
        public Vector3 pos = Vector3.zero;
        public Vector3 lhw = Vector3.one;
        public List<UltimateTerrains.Voxel> voxelValues;
        public UltimateTerrainsEditor.ReticleForEditor reticle;

        public void Initialize(Vector3 pos, Vector3 lhw)
        {
            reticle.Initialize();
            reticle.SetPositionAndSize(pos, lhw);
        }

        public void Start()
        {
            Initialize(this.pos, this.lhw);
        }
        public void Update()
        {
            reticle.SetPositionAndSize(pos, lhw);
        }
    }
}