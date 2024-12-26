using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class TerrainWorksiteIndicator : MonoBehaviour
    {
        public Vector3 pos = Vector3.zero;
        public Vector3 lhw = Vector3.one;
        public List<UltimateTerrains.Voxel> voxelValues;
        public UltimateTerrainsEditor.ReticleForEditor reticle;
        public UltimateTerrainsEditor.ReticleLinesForEditor reticleLines;

        public void Initialize(Vector3 pos, Vector3 lhw)
        {
            reticle.Initialize();
            reticle.SetPositionAndSize(pos, lhw);

            reticleLines.Initialize();
            reticleLines.SetCornerAndEdges(Vector3.zero, Vector3.left, Vector3.up, Vector3.forward);
        }

        public void Start()
        {
            Initialize(this.pos, this.lhw);
        }
        public void Update()
        {
            //reticle.SetPositionAndSize(pos, lhw);
            //reticleLines.SetCornerAndEdges(Vector3.zero, Vector3.left, Vector3.up, Vector3.forward);
        }
    }
}