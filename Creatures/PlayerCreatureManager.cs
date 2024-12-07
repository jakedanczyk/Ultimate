using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class PlayerCreatureManager : CreatureManager
    {


        public override void Awake()
        {
            base.Awake();
            CreatureData data = new CreatureData(0, CREATURE.HUMAN, GENDER.MALE, -20, 80, .3f, 1.91f, .6f, Unity.Mathematics.float3.zero);


            CreatureBody playerBody = new CreatureBody(data);
            //playerBody.data = data;
            body = playerBody;

        }

        // Start is called before the first frame update
        private void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}