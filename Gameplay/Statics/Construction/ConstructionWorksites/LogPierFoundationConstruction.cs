using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class LogPierFoundationConstruction : ConstructionWorksite
    {
        float lwScale = 0.5f;// 0.58f;
        float hScale = 1f;
        float logDiameterScaleFactor = 4f;

        Vector3 flPierPos = new Vector3(-0.75f, 0.148f, -0.75f);
        Vector3 frPierPos = new Vector3(0.75f, 0.148f, -0.75f);
        Vector3 brPierPos = new Vector3(0.75f, 0.148f, 0.75f);
        Vector3 blPierPos = new Vector3(-0.75f, 0.148f, 0.75f);

        Vector3 frontBeamPos = new Vector3(-0.84f, 0.25f, -0.75f);
        Vector3 leftBeamPos = new Vector3(-0.75f, 0.25f, -0.84f);
        Vector3 backBeamPos = new Vector3(0.84f, 0.25f, 0.75f);
        Vector3 rightBeamPos = new Vector3(0.75f, 0.25f, -0.84f);

        public float logDiameter = 0.25f;
        public RaycastHit aimHit;
        public float pillarMaxScale = 1.2f;
        public float pillarMaxHeight = 1f;

        public List<Transform> previewPillars;
        public List<Transform> finalPillars;
        public List<bool> arePillarsSupported;
        public List<float> pillarSupportHeights;

        public List<Transform> lengthBeamsPreview;
        public List<Transform> lengthBeamsPreviewCollider;
        public List<Transform> lengthBeams;
        public List<Transform> widthBeams;
        public List<Transform> widthBeamsPreview;
        public List<Transform> widthBeamsPreviewCollider;
        
        
        public List<Transform> frontBeam;
        public List<Transform> leftBeam;
        public List<Transform> backBeam;
        public List<Transform> rightBeam;

        public List<Transform> flPier;
        public List<Transform> blPier;
        public List<Transform> brPier;
        public List<Transform> frPier;

        public List<Transform> frontLeft = new List<Transform>(3);
        public List<Transform> frontRight = new List<Transform>(3);
        public List<Transform> backLeft = new List<Transform>(3);
        public List<Transform> backRight = new List<Transform>(3);

        public List<Transform> leftSide = new List<Transform>(3);
        public List<Transform> rightSide = new List<Transform>(3);
        public List<Transform> backSide = new List<Transform>(3);
        public List<Transform> frontSide = new List<Transform>(3);


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
            else if(supplies[0].countNeeded != numLogsNeeded)
            {
                supplies[0] = new SupplyCount(numLogsNeeded, 0, ITEM.LOG);
            }
        }


        public override void UpdateComponents()
        {
            float logDiameterScale = logDiameter * logDiameterScaleFactor;

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
                float pillarHeightFraction = pierHeights[i] / pillarMaxHeight * pillarMaxScale;

                pillarT.localScale = new Vector3(logDiameterScale, pillarHeightFraction, logDiameterScale);
            }

            for (int i = 0; i < lengthBeamsPreview.Count; i++)
            {
                Transform beam = lengthBeamsPreview[i];
                beam.localScale = new Vector3(logDiameterScale, lwScale * length, logDiameterScale);
                Transform beamCollider = lengthBeamsPreviewCollider[i];
                beamCollider.localScale = new Vector3(logDiameterScale, lwScale * length, logDiameterScale);
            }
            for (int i = 0; i < widthBeamsPreview.Count; i++)
            {
                Transform beam = widthBeamsPreview[i];
                beam.localScale = new Vector3(logDiameterScale, lwScale * width, logDiameterScale);
                Transform beamCollider = widthBeamsPreviewCollider[i];
                beamCollider.localScale = new Vector3(logDiameterScale, lwScale * width, logDiameterScale);
            }

            for(int i = 0; i < frontBeam.Count; i++)
            {
                Transform t = frontBeam[i];
                t.localPosition = new Vector3(0.5f * frontBeamPos.x * width, frontBeamPos.y, 0.5f * frontBeamPos.z * length);
            }
            for (int i = 0; i < leftBeam.Count; i++)
            {
                Transform t = leftBeam[i];
                t.localPosition = new Vector3(0.5f * leftBeamPos.x * width, leftBeamPos.y, 0.5f * leftBeamPos.z * length);
            }
            for (int i = 0; i < backBeam.Count; i++)
            {
                Transform t = backBeam[i];
                t.localPosition = new Vector3(0.5f * backBeamPos.x * width, backBeamPos.y, 0.5f * backBeamPos.z * length);
            }
            for (int i = 0; i < rightBeam.Count; i++)
            {
                Transform t = rightBeam[i];
                t.localPosition = new Vector3(0.5f * rightBeamPos.x * width, rightBeamPos.y, 0.5f * rightBeamPos.z * length);
            }

            for (int i = 0; i < frPier.Count; i++)
            {
                Transform t = frPier[i];
                t.localPosition = new Vector3(0.5f * frPierPos.x * width, frPierPos.y, 0.5f * frPierPos.z * length);
            }
            for (int i = 0; i < flPier.Count; i++)
            {
                Transform t = flPier[i];
                t.localPosition = new Vector3(0.5f * flPierPos.x * width, flPierPos.y, 0.5f * flPierPos.z * length);
            }
            for (int i = 0; i < blPier.Count; i++)
            {
                Transform t = blPier[i];
                t.localPosition = new Vector3(0.5f * blPierPos.x * width, blPierPos.y, 0.5f * blPierPos.z * length);
            }
            for (int i = 0; i < brPier.Count; i++)
            {
                Transform t = brPier[i];
                t.localPosition = new Vector3(0.5f * brPierPos.x * width, brPierPos.y, 0.5f * brPierPos.z * length);
            }

        }

        public override void UpdateComponentsFinal()
        {

            float logDiameterScale = logDiameter * logDiameterScaleFactor;

            for (int i = 0; i < previewPillars.Count; i++)
            {
                float pillarHeightFraction = pierHeights[i] / pillarMaxHeight * pillarMaxScale;

                Transform previewRootT = previewPillars[i];
                Transform previewPillarT = previewRootT.childCount > 0 ? previewRootT.GetChild(0) : previewRootT;
                previewPillarT.localScale = new Vector3(logDiameterScale, pillarHeightFraction, logDiameterScale);
                
                Transform rootT = finalPillars[i];
                Transform pillarT = rootT.childCount > 0 ? rootT.GetChild(0) : rootT;
                pillarT.localScale = new Vector3(logDiameterScale, pillarHeightFraction, logDiameterScale);

            }

            for (int i = 0; i < lengthBeamsPreview.Count; i++)
            {
                Transform previewBeam = lengthBeamsPreview[i];
                previewBeam.localScale = new Vector3(logDiameterScale, lwScale * length, logDiameterScale);
                Transform beam = lengthBeams[i];
                beam.localScale = new Vector3(logDiameterScale, lwScale * length, logDiameterScale);
            }
            for (int i = 0; i < widthBeamsPreview.Count; i++)
            {
                Transform previewBeam = widthBeamsPreview[i];
                previewBeam.localScale = new Vector3(logDiameterScale, lwScale * width, logDiameterScale);
                Transform beam = widthBeams[i];
                beam.localScale = new Vector3(logDiameterScale, lwScale * width, logDiameterScale);
            }

            for (int i = 0; i < frontBeam.Count; i++)
            {
                Transform t = frontBeam[i];
                t.localPosition = new Vector3(0.5f * frontBeamPos.x * width, frontBeamPos.y, 0.5f * frontBeamPos.z * length);
            }
            for (int i = 0; i < leftBeam.Count; i++)
            {
                Transform t = leftBeam[i];
                t.localPosition = new Vector3(0.5f * leftBeamPos.x * width, leftBeamPos.y, 0.5f * leftBeamPos.z * length);
            }
            for (int i = 0; i < backBeam.Count; i++)
            {
                Transform t = backBeam[i];
                t.localPosition = new Vector3(0.5f * backBeamPos.x * width, backBeamPos.y, 0.5f * backBeamPos.z * length);
            }
            for (int i = 0; i < rightBeam.Count; i++)
            {
                Transform t = rightBeam[i];
                t.localPosition = new Vector3(0.5f * rightBeamPos.x * width, rightBeamPos.y, 0.5f * rightBeamPos.z * length);
            }

            for (int i = 0; i < frPier.Count; i++)
            {
                Transform t = frPier[i];
                t.localPosition = new Vector3(0.5f * frPierPos.x * width, frPierPos.y, 0.5f * frPierPos.z * length);
            }
            for (int i = 0; i < flPier.Count; i++)
            {
                Transform t = flPier[i];
                t.localPosition = new Vector3(0.5f * flPierPos.x * width, flPierPos.y, 0.5f * flPierPos.z * length);
            }
            for (int i = 0; i < blPier.Count; i++)
            {
                Transform t = blPier[i];
                t.localPosition = new Vector3(0.5f * blPierPos.x * width, blPierPos.y, 0.5f * blPierPos.z * length);
            }
            for (int i = 0; i < brPier.Count; i++)
            {
                Transform t = brPier[i];
                t.localPosition = new Vector3(0.5f * brPierPos.x * width, brPierPos.y, 0.5f * brPierPos.z * length);
            }

            ///*foreach pillar
            // * raycast down from top to determine distance to ground
            // * adjust scale and turn off/on sections as needed to fit distance
            // */
            //for (int i = 0; i < finalPillars.Count; i++)
            //{
            //    Transform rootT = finalPillars[i];
            //    Transform pillarT = rootT.childCount > 0 ? rootT.GetChild(0) : rootT;
            //    //if (Physics.SphereCast(rootT.position, radius, Vector3.down, out aimHit, pillarMaxHeight, ConstructionManager.Instance.constructionLayers))
            //    //if (Physics.Raycast(rootT.position, Vector3.down, out aimHit, pillarMaxHeight, ConstructionLibrary.Instance.constructionLayers))
            //    //{
            //    //    Debug.DrawLine(rootT.position, aimHit.point, Color.green);
            //    //    pierHeights[i] = aimHit.distance;
            //    //    arePillarsSupported[i] = true;
            //    //}
            //    //else
            //    //{
            //    //    Debug.DrawLine(rootT.position, rootT.position + Vector3.down * pillarMaxHeight, Color.red);
            //    //    //supported = false;
            //    //    arePillarsSupported[i] = false;
            //    //    pierHeights[i] = pillarMaxHeight;
            //    //}
            //    float pillarHeightFraction = pierHeights[i] / pillarMaxHeight * pillarMaxScale;

            //    pillarT.localScale = new Vector3(pillarT.localScale.x, pillarHeightFraction, pillarT.localScale.z);
            //}
        }


        public override float SetSecondaryValue(float f) 
        {
            logDiameter = f;
            return logDiameter;
        }
        public override float GetSecondaryValue()
        {
            return logDiameter;
        }

        public override void CopyValuesFromPreview(ConstructionWorksite previewWorksite)
        {
            Debug.Log("pierFoundation CopyValues");
            base.CopyValuesFromPreview(previewWorksite);
            LogPierFoundationConstruction preview = (LogPierFoundationConstruction)previewWorksite;
            logDiameter = preview.logDiameter;
            pierHeights = preview.pierHeights;
        }
    }
}
