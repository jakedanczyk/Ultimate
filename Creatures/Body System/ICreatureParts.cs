using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /* Creatures have one or more parts. Parts have statuses and layers
     */
    public interface ICreatureParts
    {
        List<BodyPart> GetParts();
    }
}