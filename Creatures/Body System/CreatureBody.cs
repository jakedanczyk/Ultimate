using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Urth
{
    public class AnimalStats
    {
        public float bmr;

        public AnimalStats(float ibmr)
        {
            bmr = ibmr;
        }
    }

    public class CreatureBody : CreatureBodyAbstract
    {
        public CREATURE type;
        public int ID
        {
            get { return this.data.ID; }
        }
        public CreatureBody()
        {
            stats = new CreatureStats(this);
            mind = new CreatureMind();
            creatureInventory = new CreatureInventory(this);
            status = new AnimalStatus(this);
            parts = new AnimalParts(this);
            digestionModel = new AnimalDigestion(this);
            oxygenModel = new AnimalOxygen(this);
            wetnessModel = new AnimalWetness(this);
            hydrationModel = new AnimalHydration(this);
            nutritionModel = new AnimalNutrition(this);
            temperatureModel = new TropicalWarmBloodTemperature(this);
            navigation = new CreatureNavigation(this);
        }
        public CreatureBody(CreatureData idata)
        {
            data = idata;
            stats = new CreatureStats(this);
            mind = new CreatureMind();
            creatureInventory = new CreatureInventory(this);
            status = new AnimalStatus(this);
            parts = new AnimalParts(this);
            digestionModel = new AnimalDigestion(this);
            oxygenModel = new AnimalOxygen(this);
            wetnessModel = new AnimalWetness(this);
            hydrationModel = new AnimalHydration(this);
            nutritionModel = new AnimalNutrition(this);
            navigation = new CreatureNavigation(this);
            temperatureModel = new TropicalWarmBloodTemperature(this);
        }
        public CreatureBody(CreatureData idata, CreatureSpecies species)
        {
            data = idata;
            stats = new CreatureStats(this);
            mind = new CreatureMind();
            creatureInventory = new CreatureInventory(this);
            status = new AnimalStatus(this,idata.homepos);
            digestionModel = new AnimalDigestion(this);
            oxygenModel = new AnimalOxygen(this);
            wetnessModel = new AnimalWetness(this, species);
            hydrationModel = new AnimalHydration(this);
            nutritionModel = new AnimalNutrition(this);
            navigation = new CreatureNavigation(this);
            parts = new AnimalParts(this, species);
            switch (species.temperatureModel)
            {
                case (CREATURE_TEMPERATURE_MODEL.COLD_BLOOD):
                    temperatureModel = new ColdBloodTemperature(this);
                    break;
                case (CREATURE_TEMPERATURE_MODEL.TROPICAL_WARM_BLOOD):
                    temperatureModel = new TropicalWarmBloodTemperature(this);
                    break;
                case (CREATURE_TEMPERATURE_MODEL.TEMPERATE_WARM_BLOOD):
                    temperatureModel = new TemperateWarmBloodTemperature(this);
                    break;
                default:
                    temperatureModel = new ColdBloodTemperature(this);
                    break;
            }
        }
        public CreatureBody(CreatureData idata, CREATURE_TEMPERATURE_MODEL tempModel, float3 pos)
        {
            data = idata;
            stats = new CreatureStats(this);
            creatureInventory = new CreatureInventory(this);
            status = new AnimalStatus(this,pos);
            parts = new AnimalParts(this);
            digestionModel = new AnimalDigestion(this);
            oxygenModel = new AnimalOxygen(this);
            wetnessModel = new AnimalWetness(this);
            hydrationModel = new AnimalHydration(this);
            nutritionModel = new AnimalNutrition(this);
            navigation = new CreatureNavigation(this);
            switch (tempModel)
            {
                case (CREATURE_TEMPERATURE_MODEL.COLD_BLOOD):
                    temperatureModel = new ColdBloodTemperature(this);
                    break;
                case (CREATURE_TEMPERATURE_MODEL.TROPICAL_WARM_BLOOD):
                    temperatureModel = new TropicalWarmBloodTemperature(this);
                    break;
                case (CREATURE_TEMPERATURE_MODEL.TEMPERATE_WARM_BLOOD):
                    temperatureModel = new TemperateWarmBloodTemperature(this);
                    break;
                default:
                    temperatureModel = new ColdBloodTemperature(this);
                    break;
            }
        }
        public CreatureBody(CreatureStats istats, CreatureInventory iinv, CreatureTemperature itemperature, AnimalStatus istatus, AnimalDigestion idigest, AnimalParts iparts, AnimalOxygen ioxygen, AnimalWetness iwetness, AnimalHydration ihydration, AnimalNutrition inutrition)
        {
            creatureInventory = iinv;
            temperatureModel = itemperature;

            stats = istats;
            status = istatus;
            digestionModel = idigest;
            parts = iparts;
            oxygenModel = ioxygen;
            wetnessModel = iwetness;
            hydrationModel = ihydration;
            nutritionModel = inutrition;
        }

        public CreatureStats stats;
        public new AnimalParts parts;
        public new AnimalStatus status;
        public new AnimalDigestion digestionModel;
        public new AnimalOxygen oxygenModel;
        public new AnimalWetness wetnessModel;
        public new AnimalHydration hydrationModel;
        public new AnimalNutrition nutritionModel;

        public override void Update(double now)
        {
            float t = (float)(now - lastUpdateTime);
            lastUpdateTime = now;

            UpdateBlood(t);
            UpdateDigestion(t);
            UpdateOxygen(t);
            UpdateHydration(t);
            UpdateNutrition(t);
            UpdateWetness(t);
            UpdateTemperature(t);
            //UpdateFatigue();
            //UpdateSleep();

            UpdateStatus();
            UpdateMind();
        }


        public void UpdateBlood(double t)
        {
            float loss = 0;
            if (status.isBleeding)
            {
                foreach (BodyPart bodyPart in parts.GetParts())
                {
                    if (bodyPart.isInjured)
                    {
                        foreach (IntegralBodyPartLayer layer in bodyPart.bleedingLayers)
                        {
                            foreach (Injury injury in layer.injuries)
                            {
                                loss += (float)(injury.bleedRate * t);
                                //TODO implement slowing bleedrate as blood pressure drains
                            }
                        }
                    }
                }
            }
        }
        public void UpdateDigestion(float t)
        {
            digestionModel.UpdateDigestion(t);
        }

        public void UpdateOxygen(float t)
        {
            oxygenModel.UpdateOxygen(t);
        }

        public void UpdateHydration(float t)
        {
            hydrationModel.UpdateHydration(t);
        }

        public void UpdateNutrition(float t)
        {
            nutritionModel.UpdateNutrition(t);
        }

        public void UpdateWetness(float t)
        {
            wetnessModel.UpdateWetness(t);
        }

        public void UpdateTemperature(float t)
        {
            temperatureModel.UpdateTemperature(t);
        }

        public void UpdateStatus()
        {
            status.UpdateStatus();
        }
        public void UpdateMind()
        {
            mind.UpdateMind();
        }

        public CreatureSpecies GetCreatureSpecies()
        {
            return CreaturesLibrary.Instance.speciesDict[this.type];
        }
    }
}

