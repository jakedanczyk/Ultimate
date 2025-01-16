using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class CreatureStats
    {
        public StatInterface statInterface;
        public CreatureBody creatureBody;

        public CreatureStats(CreatureBody ibody)
        {
            creatureBody = ibody;
            
            statInterface = new StatInterface(new CreatureStatCollection());
        }
        public float GetMaxEncumbrance()
        {
            CreatureSpecies species = creatureBody.GetCreatureSpecies();
            int strength = statInterface.statCollection.GetStat(StatType.Strength).StatValue;
            float maxEncumbranceBase = species.CalcMaxEncumbrance(strength, creatureBody.data.weight - creatureBody.data.fatWeight);
            Debug.Log("Max Encumbrance " + maxEncumbranceBase);
            return maxEncumbranceBase;
        }


        public float GetGrabReachDistance()
        {
            return creatureBody.data.height * 1.5f;
        }
        public float GetToolReachDistance()
        {
            return GetGrabReachDistance() + Mathf.Max(creatureBody.creatureInventory.leftItemReach, creatureBody.creatureInventory.rightItemReach);
        }

        public float GetMaxMoI()
        {
            return 5f;
        }
        public float GetMaxMoI2H()
        {
            return 15f;
        }

        public float GetStatByName(string name)
        {
            StatType statType = (StatType)System.Enum.Parse(typeof(StatType), name);
            return statInterface.statCollection.GetStat(statType).StatValue;
        }

        #region Attributes
        public float Strength()
        {
            CreatureSpecies species = creatureBody.GetCreatureSpecies();
            int strength = statInterface.statCollection.GetStat(StatType.Strength).StatValue;
            float effectiveStrength = species.CalcEffectiveStrength(strength, creatureBody.data.weight - creatureBody.data.fatWeight);
            Debug.Log("effectiveStrength " + effectiveStrength);
            return effectiveStrength;
        }
        #endregion

        #region Task Skills
        public float Mining()
        {
            Debug.Log(statInterface);
            Debug.Log(statInterface.statCollection);
            Debug.Log(statInterface.statCollection.GetStat(StatType.Mining));
            return statInterface.statCollection.GetStat(StatType.Mining).StatValue;
        }
        public float Digging()
        {
            return statInterface.statCollection.GetStat(StatType.Digging).StatValue;
        }
        #endregion

    }

}