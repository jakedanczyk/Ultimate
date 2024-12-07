using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{
    public class AnimalDigestion : CreatureDigestion
    {
        public AnimalDigestion(CreatureBody ibody)
        {
            body = ibody;
        }

        public new CreatureBody body;
        public List<UItem> items;
        public MacroNutrients rate;
        public MacroNutrients stomachMacros;
        public MacroNutrients gutMacros;
        public float stomachWater;
        public float gutWater;
        public float waterRate;

        public override void UpdateDigestion(float timestep)
        {
            UpdateStomach(timestep);
            UpdateGut(timestep);
        }

        public void UpdateDigestion(float timestep, MacroNutrients newRate)
        {
            SetRate(newRate);
            UpdateStomach(timestep);
            UpdateGut(timestep);
        }

        public void SetRate(MacroNutrients newRate)
        {
            rate = newRate;
        }

        public void UpdateStomach(float timestep)
        {
            float digestedProtein = Mathf.Max(rate.p * timestep, stomachMacros.p);
            float digestedFat = Mathf.Max(rate.f * timestep, stomachMacros.f);
            float digestedCarbs = Mathf.Max(rate.c * timestep, stomachMacros.c);
            MacroNutrients toGut = new MacroNutrients(digestedProtein, digestedFat, digestedCarbs);
            stomachMacros = stomachMacros - toGut;
            gutMacros = gutMacros + toGut;

            float digestedWater = Mathf.Max(waterRate * timestep, stomachWater);
            stomachWater -= digestedWater;
            gutWater += digestedWater;
        }

        public void UpdateGut(float timestep)
        {
            float absorbedProtein = Mathf.Max(rate.p * timestep, gutMacros.p);
            float absorbedFat = Mathf.Max(rate.f * timestep, gutMacros.f);
            float absorbedCarbs = Mathf.Max(rate.c * timestep, gutMacros.c);
            MacroNutrients absorbedMacros = new MacroNutrients(absorbedProtein, absorbedFat, absorbedCarbs);
            gutMacros = gutMacros - absorbedMacros;
            body.nutritionModel.macros += absorbedMacros;

            float absorbedWater = Mathf.Max(waterRate * timestep, gutWater);
            gutWater -= absorbedWater;
            body.hydrationModel.water += absorbedWater;
        }
    }
}