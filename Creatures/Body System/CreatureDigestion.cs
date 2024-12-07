using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public enum DIET
    {
        HERB,
        CARN,
        OMNI
    }

    public abstract class CreatureDigestion : ICreatureDigestion
    {
        public CreatureBody body;
        public abstract void UpdateDigestion(float timestep);
    }
}