using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class ItemLayer
    {

        public float condition = 1.0f;
        public List<Injury> injuries;

        public bool partial = false;
        public float thickness;
        public UMaterial mat;

        public bool HitTest(RaycastHit hit)
        {
            return true;
        }

        /*Returns portion of layer penetrated, from 0f-1.01f. 
         * >=1.0f means penetration
         */
        public float SharpPenetrationDepth(Attack attack)
        {
            return 0f;
        }
        public float BluntPenetrationDepth(Attack attack)
        {
            return 0f;
        }
        public bool BluntBounceTest(Attack attack)
        {
            return true;
        }
    }
}