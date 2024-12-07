using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class AnimalHydration : CreatureHydration
    {
        public new CreatureBody body;

        public AnimalHydration(CreatureBody ib) : base(ib)
        {
            body = ib;
        }

        public void SweatLoss(float loss)
        {
            this.RemoveWater(loss);
        }

        public new void UpdateHydration(float t)
        {
            ConsumeWaterForMetabolism(t);
        }
        public void ConsumeWaterForMetabolism(float t)
        {
            float waterRate = body.nutritionModel.waterNeedRate;
            water -= waterRate * t;
        }
    }
}