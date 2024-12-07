using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{
    public enum CREATURE_TEMPERATURE_MODEL
    {
        TROPICAL_WARM_BLOOD,
        TEMPERATE_WARM_BLOOD,
        COLD_BLOOD,
    }

    public class CreatureTemperature : ICreatureTemperature
    {
        public CreatureBody body;

        public CreatureTemperature(CreatureBody ib)
        {
            body = ib;
        }

        public void UpdateTemperature(float timestep)
        {

        }
    }
}