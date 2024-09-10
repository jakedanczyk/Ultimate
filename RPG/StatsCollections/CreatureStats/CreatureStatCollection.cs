using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /*
     * Currently just a wrapper for all creature stat collections
     */
    public class CreatureStatCollection : StatCollection
    {
        public override void ConfigureStats()
        {
            foreach(StatType attribType in UrthConstants.BODY_ATTRIBUTES)
            {
                var attrib = CreateOrGetStat<StatAttribute>(attribType);
                attrib.StatName = attribType.ToString();
                attrib.StatBaseValue = UrthConstants.ATTRIB_LVL_DEFAULT;
            }
            foreach (StatType attribType in UrthConstants.MIND_ATTRIBUTES)
            {
                var attrib = CreateOrGetStat<StatAttribute>(attribType);
                attrib.StatName = attribType.ToString();
                attrib.StatBaseValue = UrthConstants.ATTRIB_LVL_DEFAULT;
            }
        }
    }
}