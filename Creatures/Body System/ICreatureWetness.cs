using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /* Track the exterior wetness of creature bodies
     * Track per bodypart
     * There are a number of reasons water can be added or removed
     * For now rely on generic Add/RemoveWater methods
     * 
     * -Temperature model will remove water due to evaporation
     */
    public interface ICreatureWetness
    {
        void AddWaterAllParts(List<(float, BodyPartId)> amounts);
        void AddWaterSinglePart(float amount, BodyPartId part);
    }
}