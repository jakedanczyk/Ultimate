using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class LogFloorConstruction : ConstructionWorksite
    {
        public ConstructionPreviewCollider[] supportColliderGrid = new ConstructionPreviewCollider[9];

        public override USTATIC type
        {
            get { return USTATIC.FLOOR_LOG; }
        }

        public override void UpdateSuppliesNeeded()
        {
            if (supplies == null)
            {
                supplies = new List<SupplyCount>(1);
            }

            //float scaleF = size * size * size;
            ////10cm thick, so 10cm diameter (5cm radius) logs covering a 2x2m area
            ////each log = 0.0157 m^3 = 0.05m * 0.05m * PI * 2m
            ////20 logs side by side to cover 2m, 20 * 0.0157 = 0.314159 m^3
            //int numLogsNeeded = (int)(Mathf.Ceil(0.314159f * scaleF / UrthConstants.LOG_VOLUME));

            float vol = length * width * height;
            int numLogsNeeded = (int)(Mathf.Ceil(vol / UrthConstants.LOG_VOLUME));

            if ((supplies.Count == 0))
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
            transform.localScale = new Vector3(length, height, width);
            CheckSupport();
        }

        public override void UpdateComponentsFinal()
        {
            transform.localScale = new Vector3(length, height, width);
        }

        public bool CheckSupport()
        {
            int count = 0;
            foreach(ConstructionPreviewCollider collider in supportColliderGrid)
            {
                if(collider.collisionsList.Count > 0)
                {
                    count++;
                }
            }
            supported = count >= 6;
            return supported;
        }
    }
}
