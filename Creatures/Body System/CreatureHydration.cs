using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class CreatureHydration : ICreatureHydration
    {
        public float water;
        public float waterRate;
        public CreatureBody body;

        public CreatureHydration(CreatureBody ib)
        {
            body = ib;
        }

        public void RemoveWater(float loss)
        {
            water = Mathf.Max(0f, water - loss);
        }

        public void AddWater(float add)
        {
            water += add;
        }

        public void UpdateHydration(float t)
        {
            water -= waterRate * t;
        }
    }
}