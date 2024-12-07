using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class TemperateWarmBloodTemperature : CreatureTemperature
    {
        public TemperateWarmBloodTemperature(CreatureBody ib) : base(ib)
        {
            body = ib;
        }
        public new void UpdateTemperature(float timestep)
        {
            foreach (BodyPart bp in body.parts.GetParts())
            {

            }
        }
    }
}