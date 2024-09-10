using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class TerrainBlockFraction
    {
        public UMATERIAL mat;
        public float volumeFraction;
        public float fracture;

        public TerrainBlockFraction(UMATERIAL imate, float ivol, float ifrac)
        {
            mat = imate;
            volumeFraction = ivol;
            fracture = ifrac;
        }
        public float Resistance()
        {
            UMaterial uMaterial = MaterialsLibrary.Instance.matsDict[mat];
            return uMaterial.density * uMaterial.hardness * uMaterial.toughness * (1f - fracture);
        }
    }
}