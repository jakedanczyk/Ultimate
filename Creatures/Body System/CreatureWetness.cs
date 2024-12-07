using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public abstract class CreatureWetness : ICreatureWetness
    {
        public CreatureBody body;
        public abstract void AddWaterAllParts(List<(float, BodyPartId)> amounts);
        public abstract void AddWaterSinglePart(float amount, BodyPartId part);
    }

}
