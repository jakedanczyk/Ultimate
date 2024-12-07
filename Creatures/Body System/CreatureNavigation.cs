using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


/*Handles long range course plotting, and updates position for ecosystem sim
 */
namespace Urth
{
    public class CreatureNavigation : ICreatureNavigation
    {
        public CreatureBody body;
        public CreatureNavigation(CreatureBody ib)
        {
            body = ib;
        }


        public float3 navdir; 
        public float navspeed; //
        public float3 travelDestination; // current ultimate destination

        //Used to simulate motion of distant, abstracted creatures
        public float3 UpdateMovement(float t)
        {
            navdir = ((Vector3)(travelDestination - body.status.pos)).normalized;
            float3 stepPos = body.status.pos + navdir * navspeed * t;
            stepPos.y = (float)TerrainManager.Main.GetTerrainHeight(stepPos);
            return stepPos;
        }
    }
}
