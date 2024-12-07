using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{
    public enum NEED
    {
        HEALING,
        BLOOD,
        OXYGEN,
        FOOD,
        WATER,
        SHELTER,
        HEAT,
        COOLING,
        THREAT
    }


    public enum NEED_LEVEL
    {
        PERFECT,
        HEALTHY,
        NORMAL,
        MINOR,
        MODERATE,
        SERIOUS,
        SEVERE,
        CRITICAL,
        MAXIMAL,
    }


    public class CreatureNeeds
    {
        CreatureBody creatureBody;
        CreatureBody animalBody;

        public CreatureNeeds(CreatureBody ibody)
        {
            creatureBody = ibody;
            switch (ibody.creatureClass)
            {
                case CREATURE_CLASS.ANIMAL:
                    animalBody = (CreatureBody)ibody;
                    needsPriority = new List<NEED> { NEED.COOLING, NEED.BLOOD, NEED.OXYGEN, NEED.THREAT, NEED.HEALING, NEED.WATER, NEED.HEAT, NEED.SHELTER, NEED.FOOD };
                    break;
                case CREATURE_CLASS.ANIMATED:
                    needsPriority = new List<NEED> { NEED.THREAT, NEED.HEALING, NEED.COOLING };
                    break;
            }

            needsLevels = new Dictionary<NEED, NEED_LEVEL>(needsPriority.Count);
            foreach (NEED need in needsPriority)
            {
                needsLevels.Add(need, NEED_LEVEL.NORMAL);
            }
        }

        public List<NEED> needsPriority;

        public Dictionary<NEED, NEED_LEVEL> needsLevels;
        public (NEED, NEED_LEVEL) CheckNeeds()
        {
            NEED worstNeed = NEED.HEALING;
            NEED_LEVEL worstLevel = NEED_LEVEL.PERFECT;
            foreach (NEED need in needsPriority)
            {
                switch (need)
                {
                    case NEED.BLOOD:
                        needsLevels[need] = CheckBlood();
                        break;
                    case NEED.OXYGEN:
                        needsLevels[need] = CheckOxygen();
                        break;
                    case NEED.HEALING:
                        needsLevels[need] = CheckHealth();
                        break;
                    case NEED.WATER:
                        needsLevels[need] = CheckWater();
                        break;
                    case NEED.HEAT:
                        needsLevels[need] = CheckHeat();
                        break;
                    case NEED.SHELTER:
                        needsLevels[need] = CheckShelter();
                        break;
                    case NEED.THREAT:
                        needsLevels[need] = CheckThreat();
                        break;
                    case NEED.FOOD:
                        needsLevels[need] = CheckFood();
                        break;
                }
                if (needsLevels[need] > worstLevel)
                {
                    worstNeed = need;
                    worstLevel = needsLevels[need];
                }
            }
            return (worstNeed, worstLevel);
        }

        public NEED_LEVEL CheckBlood()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckOxygen()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckHealth()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckWater()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckHeat()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckShelter()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckThreat()
        {
            return NEED_LEVEL.NORMAL;
        }
        public NEED_LEVEL CheckFood()
        {
            return NEED_LEVEL.NORMAL;
        }




        public void SetBody(CreatureBody body)
        {
            switch (body.creatureClass)
            {
                case CREATURE_CLASS.ANIMAL:
                    animalBody = (CreatureBody)body;
                    break;
            }
        }
    }
}