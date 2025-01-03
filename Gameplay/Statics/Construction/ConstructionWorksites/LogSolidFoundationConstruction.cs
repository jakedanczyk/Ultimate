using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class LogSolidFoundationConstruction : ConstructionWorksite
    {
        public Transform foundationTransform;
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
            float vol = length * width * height;
            //1m tall, 2m square. 4m^3 volume. Assume 75% volume fill, 3m^3
            int numLogsNeeded = (int)(Mathf.Ceil(0.75f * vol / UrthConstants.LOG_VOLUME));

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
            foundationTransform.localScale = new Vector3(length, height, width);
            supported = true;//start true until proven false
        }

        public override void UpdateComponentsFinal()
        {
            foundationTransform.localScale = new Vector3(length, height, width);
        }
    }
}
