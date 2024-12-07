using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class AnimalWetness : CreatureWetness
    {
        public AnimalWetness(CreatureBody ib, List<(BodyPartId, float)> isweats)
        {
            body = ib;
            sweatRates = isweats;
        }
        public AnimalWetness(CreatureBody ib, CreatureSpecies species)
        {
            body = ib;
            sweatRates = species.sweatRates;
        }
        public AnimalWetness(CreatureBody ib)
        {
            body = ib;
            sweatRates = new List<(BodyPartId, float)>();
        }

        public new CreatureBody body;

        public List<(BodyPartId, float)> sweatRates;

        public void Sweat(float t)
        {
            foreach ((BodyPartId bp, float amnt) in sweatRates)
            {
                body.parts.dict[bp].waterCover += t * amnt;
                body.hydrationModel.SweatLoss(t * amnt);
            }
        }

        public void Precipitation(float t)
        {
            float precipRate = 0; //Weather.GetPrecipRate();
            foreach ((BodyPartId bp, float amnt) in sweatRates)
            {
                body.parts.dict[bp].waterCover += t * precipRate;
            }
        }

        public void Evaporation(float t)
        {
            float evapRate = 0; //Weather.GetEvapRate();
            foreach ((BodyPartId bp, float amnt) in sweatRates)
            {
                body.parts.dict[bp].waterCover += t * evapRate;
            }
        }

        public override void AddWaterAllParts(List<(float, BodyPartId)> amounts)
        {   
            foreach ((float amnt, BodyPartId bp) in amounts)
            {
                body.parts.dict[bp].waterCover += amnt;
            }
        }

        public override void AddWaterSinglePart(float amount, BodyPartId part)
        {
            body.parts.dict[part].waterCover += amount;
        }

        public void UpdateWetness(float t)
        {
            Sweat(t);
            Precipitation(t);
            Evaporation(t);
        }
    }
}
