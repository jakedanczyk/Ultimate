using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    [System.Serializable]
    public class CreatureCollider
    {
        public Collider collider;
        public BODY_PART part;
    }


    /* CreatureCollisionModel maps colliders to bodyparts (and aspects of bodyparts)
     */
    public class CreatureCollisionModel : MonoBehaviour
    {
        public int num = 0;
        public List<CreatureCollider> parts = new List<CreatureCollider>(10);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}