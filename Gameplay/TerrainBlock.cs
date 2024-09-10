using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class TerrainBlock
    {
        public bool surfaceHorizons;
        public List<TerrainBlockFraction> fractions;

        public TerrainBlock(bool isurface)
        {
            surfaceHorizons = isurface;
            fractions = new List<TerrainBlockFraction>();
        }

        public float FilledVolume()
        {
            float fill = 0f;
            foreach (TerrainBlockFraction fraction in fractions)
            {
                fill += fraction.volumeFraction;
            }
            fill = Mathf.Min(1f, fill);//1m cubes, if more than 1, there is a bug in setting fraction volumes
            return fill;
        }

        public float SupportWeight()
        {
            return 100000f * FilledVolume();
        }

    }
}