using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class AnimalTemperature : CreatureTemperature
    {
        public float water;
        public new CreatureBody body;

        public AnimalTemperature(CreatureBody ib) : base(ib)
        {
            body = ib;
        }
    }
}