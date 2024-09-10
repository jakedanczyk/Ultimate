using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /*Support for flat floors or similar objects
     * 3x3 grid of supports
     * Supported if any 4 supports are supported
     */
    public class ConstructionPreviewFloorSupport : MonoBehaviour
    {
        public bool supported;
        public ConstructionPreviewCollider[] colliders = new ConstructionPreviewCollider[9];

        int supportCount;
        public void UpdateFloor()
        {
            supportCount = 0;
            foreach(ConstructionPreviewCollider collider in colliders)
            {
                if(collider.collisionsList.Count > 0)
                {
                    supportCount += 1;
                }
            }
            supported = supportCount >= 4 ? true : false;
        }
    }
}