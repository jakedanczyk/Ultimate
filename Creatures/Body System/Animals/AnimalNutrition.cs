using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{
    /*An animals reserve of food nutrients:
     * -energy (calories as fat, protein, carbs)
     * -micronutrients
     */
    public class AnimalNutrition : CreatureNutrition
    {
        public MacroNutrients macros;
        public MacroNutrients macroBurnRates;
        public CreatureBody body;
        public float waterNeedRate;
        public AnimalNutrition(CreatureBody ib)
        {
            body = ib;
        }

        public void UpdateNutrition(double t)
        {
            macros.c -= (float)(macroBurnRates.c * t);
            macros.f -= (float)(macroBurnRates.f * t);
            macros.p -= (float)(macroBurnRates.p * t);
        }
    }
}