using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{

    public interface ICreatureNavigation
    {
        float3 UpdateMovement(float t);
    }

}