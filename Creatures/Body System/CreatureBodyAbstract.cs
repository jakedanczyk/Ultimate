using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public enum CREATURE_CLASS 
    { 
        ANIMAL,
        ANIMATED,
    }

    public abstract class CreatureBodyAbstract
    {
        public bool ableToSleep;

        public CREATURE_CLASS creatureClass;
        public CreatureManager manager;
        public CreatureInventory creatureInventory;
        public CreatureStatus status;
        public CreatureParts parts;
        public CreaturePerception perception;
        public CreatureTemperature temperatureModel;
        public CreatureOxygen oxygenModel;
        public CreatureWetness wetnessModel;
        public CreatureHydration hydrationModel;
        public CreatureDigestion digestionModel;
        public CreatureNutrition nutritionModel;
        public CreatureNavigation navigation;
        public CreatureMind mind;

        public StatCollection rpgStats;
        public CreatureData data;

        public double lastUpdateTime;
        public abstract void Update(double t);
    }
}
