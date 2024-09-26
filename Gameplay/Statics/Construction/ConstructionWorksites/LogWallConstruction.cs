using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class LogWallConstruction : ConstructionWorksite
    {
        float lwScale = 0.5f;// 0.58f;
        float hScale = 1f;
        float logDiameterScaleFactor = 4f;

        //Vector3 flPierPos = new Vector3(-0.75f, 0.148f, -0.75f);
        //Vector3 frPierPos = new Vector3(0.75f, 0.148f, -0.75f);
        //Vector3 brPierPos = new Vector3(0.75f, 0.148f, 0.75f);
        //Vector3 blPierPos = new Vector3(-0.75f, 0.148f, 0.75f);

        //Vector3 frontBeamPos = new Vector3(-0.84f, 0.25f, -0.75f);
        //Vector3 leftBeamPos = new Vector3(-0.75f, 0.25f, -0.84f);
        //Vector3 backBeamPos = new Vector3(0.84f, 0.25f, 0.75f);
        //Vector3 rightBeamPos = new Vector3(0.75f, 0.25f, -0.84f);

        public float logDiameter = 0.25f;
        public int logCount = 8;
        public RaycastHit aimHit;

        public Transform wall;
        public List<Transform> logs;
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
            logVolume = length * logCsa * logCount;
            int numSupplyLogsNeeded = (int)(Mathf.Ceil(logVolume / UrthConstants.LOG_VOLUME));

            if((supplies.Count == 0))
            {
                supplies.Add(new SupplyCount(numSupplyLogsNeeded, 0, ITEM.LOG));
            }
            else if(supplies[0].countNeeded != numSupplyLogsNeeded)
            {
                supplies[0] = new SupplyCount(numSupplyLogsNeeded, 0, ITEM.LOG);
            }
        }


        public override void UpdateComponents()
        {
            wall.localScale = new Vector3(height, width, length);

            //for (int i = 0; i < lengthBeamsPreview.Count; i++)
            //{
            //    Transform beam = lengthBeamsPreview[i];
            //    beam.localScale = new Vector3(logDiameterScale, lwScale * length, logDiameterScale);
            //    Transform beamCollider = lengthBeamsPreviewCollider[i];
            //    beamCollider.localScale = new Vector3(logDiameterScale, lwScale * length, logDiameterScale);
            //}
            //for (int i = 0; i < widthBeamsPreview.Count; i++)
            //{
            //    Transform beam = widthBeamsPreview[i];
            //    beam.localScale = new Vector3(logDiameterScale, lwScale * width, logDiameterScale);
            //    Transform beamCollider = widthBeamsPreviewCollider[i];
            //    beamCollider.localScale = new Vector3(logDiameterScale, lwScale * width, logDiameterScale);
            //}
        }

        public override void UpdateComponentsFinal()
        {
            wall.localScale = new Vector3(height, width, length);
        }


        public override float SetSecondaryValue(float f) 
        {
            logDiameter = f;
            width = logDiameter;
            height = logDiameter * logCount;
            return logDiameter;
        }
        public override float GetSecondaryValue()
        {
            return logDiameter;
        }
        public override int SetTertiaryValue(int n)
        {
            logCount = n;
            for(int i = 0; i < logCount; i++)
            {
                logs[i].gameObject.SetActive(true);
            }
            for (int i = logCount; i < logs.Count; i++)
            {
                logs[i].gameObject.SetActive(true);
            }
            logDiameter = height / logCount;
            width = logDiameter;
            return logCount;
        }
        public override int GetTertiaryValue()
        {
            return logCount;
        }
    }
}
