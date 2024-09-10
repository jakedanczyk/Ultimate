using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    [System.Serializable]
    public struct PillarSize
    {
        public GameObject go;
        public float length;
        public float minLength;
        public float maxLength;
        public (float, float) minSizeScale;
    }

    public class ConstructionPreviewPillarSet : MonoBehaviour
    {
        public bool supported;
        public float pillarMinLength;
        public float pillarMaxHeight;
        //public float pillarMinScale;
        public float pillarMaxScale;

        public List<Transform> pillars;
        public List<bool> arePillarsSupported;
        public List<float> pillarSupportHeights;


        public float radius = 0.1f;
        public RaycastHit aimHit;

        public void UpdatePillars()
        {
            /*foreach pillar
             * raycast down from top to determine distance to ground
             * adjust scale and turn off/on sections as needed to fit distance
             * 
             */
            supported = true;//start true until proven false
            for(int i = 0; i < pillars.Count; i++)
            {
                Transform rootT = pillars[i];
                Transform pillarT = rootT.childCount > 0 ? rootT.GetChild(0) : rootT;
                //if (Physics.SphereCast(rootT.position, radius, Vector3.down, out aimHit, pillarMaxHeight, ConstructionManager.Instance.constructionLayers))
                if (Physics.Raycast(rootT.position, Vector3.down, out aimHit, pillarMaxHeight, ConstructionLibrary.Instance.constructionLayers))
                {
                    Debug.DrawLine(rootT.position, aimHit.point, Color.green);
                    pillarSupportHeights[i] = aimHit.distance;
                    arePillarsSupported[i] = true;
                }
                else
                {
                    Debug.DrawLine(rootT.position, rootT.position + Vector3.down * pillarMaxHeight, Color.red);
                    supported = false;
                    arePillarsSupported[i] = false;
                    pillarSupportHeights[i] = pillarMaxHeight;
                }
                float pillarHeightFraction = pillarSupportHeights[i] / pillarMaxHeight * pillarMaxScale;

                pillarT.localScale = new Vector3(pillarT.localScale.x, pillarHeightFraction, pillarT.localScale.z);
            }
        }
    }
}