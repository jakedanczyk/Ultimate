using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class LogPierFoundationConstruction : ConstructionWorksite
    {
        public float logDiameter = 0.1f;
        public RaycastHit aimHit;
        public float pillarMaxScale = 1.2f;
        public float pillarMaxHeight = 1f;
        public List<Transform> previewPillars;
        public List<Transform> finalPillars;
        public List<bool> arePillarsSupported;
        public List<float> pillarSupportHeights;

        public float[] pierHeights = new float[4];
        public override USTATIC type
        {
            get { return USTATIC.FOUNDATION_LOG_PIER_LOG; }
        }

        public float logVolume = 0f;
        public override void UpdateSuppliesNeeded()
        {
            if(supplies == null)
            {
                supplies = new List<SupplyCount>(1);
            }
            //float logDiameter = 0.25f * size;
            float logRadius = logDiameter * 0.5f;
            float logCsa = logRadius * logRadius * Mathf.PI;
            float beamLength = 2f * length * width;
            float beamVolume = beamLength * logCsa;
            float pierVolume = 0f;
            for (int i = 0; i < 4; i++)
            {
                pierVolume += logCsa * pierHeights[i];
            }
            logVolume = pierVolume + beamVolume;
            int numLogsNeeded = (int)(Mathf.Ceil(logVolume / UrthConstants.LOG_VOLUME));

            if((supplies.Count == 0))
            {
                supplies.Add(new SupplyCount(numLogsNeeded, 0, ITEM.LOG));
            }
            else
            {
                supplies[0] = new SupplyCount(numLogsNeeded, 0, ITEM.LOG);
            }
        }


        public override void UpdateComponents()
        {
            /*foreach pillar
             * raycast down from top to determine distance to ground
             * adjust scale and turn off/on sections as needed to fit distance
             */
            supported = true;//start true until proven false
            for (int i = 0; i < previewPillars.Count; i++)
            {
                Transform rootT = previewPillars[i];
                Transform pillarT = rootT.childCount > 0 ? rootT.GetChild(0) : rootT;
                //if (Physics.SphereCast(rootT.position, radius, Vector3.down, out aimHit, pillarMaxHeight, ConstructionManager.Instance.constructionLayers))
                if (Physics.Raycast(rootT.position, Vector3.down, out aimHit, pillarMaxHeight, ConstructionLibrary.Instance.constructionLayers))
                {
                    Debug.DrawLine(rootT.position, aimHit.point, Color.green);
                    pierHeights[i] = aimHit.distance;
                    arePillarsSupported[i] = true;
                }
                else
                {
                    Debug.DrawLine(rootT.position, rootT.position + Vector3.down * pillarMaxHeight, Color.red);
                    supported = false;
                    arePillarsSupported[i] = false;
                    pierHeights[i] = pillarMaxHeight;
                }
                Debug.Log(pierHeights[i]);
                float pillarHeightFraction = pierHeights[i] / pillarMaxHeight * pillarMaxScale;

                pillarT.localScale = new Vector3(pillarT.localScale.x, pillarHeightFraction, pillarT.localScale.z);
            }
        }

        public override void UpdateComponentsFinal()
        {
            /*foreach pillar
             * raycast down from top to determine distance to ground
             * adjust scale and turn off/on sections as needed to fit distance
             */
            for (int i = 0; i < finalPillars.Count; i++)
            {
                Transform rootT = finalPillars[i];
                Transform pillarT = rootT.childCount > 0 ? rootT.GetChild(0) : rootT;
                //if (Physics.SphereCast(rootT.position, radius, Vector3.down, out aimHit, pillarMaxHeight, ConstructionManager.Instance.constructionLayers))
                //if (Physics.Raycast(rootT.position, Vector3.down, out aimHit, pillarMaxHeight, ConstructionLibrary.Instance.constructionLayers))
                //{
                //    Debug.DrawLine(rootT.position, aimHit.point, Color.green);
                //    pierHeights[i] = aimHit.distance;
                //    arePillarsSupported[i] = true;
                //}
                //else
                //{
                //    Debug.DrawLine(rootT.position, rootT.position + Vector3.down * pillarMaxHeight, Color.red);
                //    //supported = false;
                //    arePillarsSupported[i] = false;
                //    pierHeights[i] = pillarMaxHeight;
                //}
                float pillarHeightFraction = pierHeights[i] / pillarMaxHeight * pillarMaxScale;

                pillarT.localScale = new Vector3(pillarT.localScale.x, pillarHeightFraction, pillarT.localScale.z);
            }
        }
    }
}
