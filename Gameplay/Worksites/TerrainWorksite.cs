using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class TerrainWorksite : MonoBehaviour
    {
        public TerrainWorksiteIndicator hudCube;
        public TerrainWorksiteData data;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetIndicator(bool on)
        {
            hudCube.gameObject.SetActive(on);
        }

        public List<TerrainBlockFraction> GetExistentFractions()
        {
            List<TerrainBlockFraction> list = new List<TerrainBlockFraction>();
            foreach (TerrainBlockFraction terrainBlockFraction in data.terrainBlock.fractions)
            {
                if (terrainBlockFraction.volumeFraction > 0f)
                {
                    list.Add(terrainBlockFraction);
                }
            }
            return list;
        }
        public List<TerrainBlockFraction> GetUnfracturedFractions()
        {
            List<TerrainBlockFraction> list = new List<TerrainBlockFraction>();
            foreach(TerrainBlockFraction terrainBlockFraction in data.terrainBlock.fractions)
            {
                if(terrainBlockFraction.fracture < 1f)
                {
                    list.Add(terrainBlockFraction);
                }
            }
            return list;
        }
    }

}