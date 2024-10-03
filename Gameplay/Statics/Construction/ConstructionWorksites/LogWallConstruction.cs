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
        public float maxDiameter = 1.0f;
        public float minDiameter = 0.1f;
        public int activeLogCount = 8;
        public RaycastHit aimHit;

        public Transform colliderTransorm;
        public Transform logsTransform;
        public List<Transform> logs;

        public ConstructionPreviewCollider leftSupport;
        public ConstructionPreviewCollider centerLeftSupport;
        public ConstructionPreviewCollider centerSupport;
        public ConstructionPreviewCollider centerRightSupport;
        public ConstructionPreviewCollider rightSupport;

        public override USTATIC type
        {
            get { return USTATIC.WALL_LOG; }
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
            logVolume = length * logCsa * activeLogCount;
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
            logsTransform.localScale = new Vector3(logDiameter * logDiameterScaleFactor, length * lwScale, logDiameter * logDiameterScaleFactor);
            ScaleColliderHeight();
            supported = false;
            if (leftSupport.collisionsList.Count > 0)
            {
                if (rightSupport.collisionsList.Count > 0)
                {
                    supported = true;
                    //if (centerLeftSupport.collisionsList.Count > 0 || centerSupport.collisionsList.Count > 0 || centerRightSupport.collisionsList.Count > 0)
                    //{
                    //    //super-supported?
                    //}
                }
                else if (centerRightSupport.collisionsList.Count > 0)
                {
                    supported = true;
                    //if (centerLeftSupport.collisionsList.Count > 0 || centerSupport.collisionsList.Count > 0)
                    //{
                    //    //super-supported?
                    //}
                }
            }
            else if(centerLeftSupport.collisionsList.Count > 0)
            {
                if (rightSupport.collisionsList.Count > 0)
                {
                    supported = true;
                    //if (centerSupport.collisionsList.Count > 0 || centerRightSupport.collisionsList.Count > 0)
                    //{
                    //    //super-supported?
                    //}
                }
                else if (centerRightSupport.collisionsList.Count > 0)
                {
                    supported = true;
                    //if (centerSupport.collisionsList.Count > 0)
                    //{
                    //    //super-supported?
                    //}
                }
            }
            //colliderTransorm.localScale = new Vector3(height, width, length);

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
            logsTransform.localScale = new Vector3(logDiameter * logDiameterScaleFactor, length * lwScale, logDiameter * logDiameterScaleFactor);
            ScaleColliderHeight();

            //colliderTransorm.localScale = new Vector3(height, width, length);
            //logsTransform.localScale = new Vector3(height, width, length);
        }

        public override float SetHeight(float h)
        {
            return height;
            //height = h;
            //float logD = height / logCount;
            //while(logD > maxDiameter)
            //{
            //    logCount += 1;
            //    logD = height / logCount;
            //}
            //while(logD < minDiameter)
            //{
            //    logCount -= 1;
            //    logD = height / logCount;
            //}


            //logDiameter = height / logCount;

            //logDiameter = f;
            //width = logDiameter;
            //height = logDiameter * logCount;
            //return logDiameter;
        }
        public override float GetHeight()
        {
            return logDiameter;
        }


        public override (float,float) GetSecondaryMinMax()
        {
            return (minDiameter, maxDiameter);
        }
        public override float SetSecondaryValue(float f) 
        {
            logDiameter = f;
            width = logDiameter;
            height = logDiameter * activeLogCount;
            return logDiameter;
        }
        public override float GetSecondaryValue()
        {
            return logDiameter;
        }
        public override (int, int) GetTertiaryMinMax()
        {
            return (1, logs.Count);
        }
        public override int SetTertiaryValue(int n)
        {
            activeLogCount = n;
            height = activeLogCount * logDiameter;
            SetActiveLogs();
            return activeLogCount;
        }
        public override int GetTertiaryValue()
        {
            return activeLogCount;
        }

        void SetActiveLogs()
        {
            for (int i = 0; i < activeLogCount; i++)
            {
                logs[i].gameObject.SetActive(true);
            }
            for (int i = activeLogCount; i < logs.Count; i++)
            {
                logs[i].gameObject.SetActive(true);
            }
        }

        void ScaleColliderHeight()
        {
            float f = activeLogCount / ((float)logs.Count);
            colliderTransorm.localScale = new Vector3(f, 1, 1);
        }
    }
}
