using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class AnimalOxygen : CreatureOxygen
    {
        public AnimalOxygen(CreatureBody ib)
        {
            body = ib;
        }

        public CreatureBody body;
        public float oxygen;
        public float heartbpm; //TODO model heart rate
        public override void UpdateOxygen(float t)
        {
            if (body.status.CanBreathe())
            {
                oxygen += Mathf.Min(OxygenDemand(), MaxOxygenSupplyRate());
            }
        }

        //the maximum oxygen supply an animal could extract from its current environment
        //actual burn rate will be lower if needed
        public float MaxOxygenSupplyRate()
        {
            return body.status.weight;
        }

        //actual current demand. 
        public float OxygenDemand()
        {
            return body.status.bmr;
        }
    }
}