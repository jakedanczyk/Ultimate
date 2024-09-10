using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class ConstructionPillarSet : MonoBehaviour
    {
        public bool supported;
        public float pillarMaxHeight;
        public float pillarMaxScale;
        public float pillarDiameter;
        public List<Transform> pillars;
        public List<bool> arePillarsSupported;
        public List<float> pillarSupportHeights;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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
            for (int i = 0; i < pillars.Count; i++)
            {
                Transform rootT = pillars[i];
                Transform pillarT = rootT.childCount > 0 ? rootT.GetChild(0) : rootT;
                //if (Physics.SphereCast(rootT.position, radius, Vector3.down, out aimHit, pillarMaxHeight, ConstructionManager.Instance.constructionLayers))
                if (Physics.Raycast(rootT.position, Vector3.down, out aimHit, pillarSupportHeights[i], ConstructionLibrary.Instance.constructionLayers))
                {
                    Debug.DrawLine(rootT.position, aimHit.point, Color.green);
                    arePillarsSupported[i] = true;
                }
                else
                {
                    Debug.DrawLine(rootT.position, rootT.position + Vector3.down * pillarSupportHeights[i], Color.red);
                    supported = false;
                    arePillarsSupported[i] = false;
                }
                float pillarHeightFraction = pillarSupportHeights[i] / pillarMaxHeight * pillarMaxScale;

                pillarT.localScale = new Vector3(pillarT.localScale.x, pillarHeightFraction, pillarT.localScale.z);
            }
        }
    }
}