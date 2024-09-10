using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /// <summary>
    /// Attached to StaticPrefabs when they are constructions
    /// </summary>
    public class ConstructionPrefab : MonoBehaviour
    {
        public int id;
        public CONSTRUCTION_METHOD type;
        public float primaryVolume;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public float PrimaryVolume()
        {
            return primaryVolume;
        }
    }

}