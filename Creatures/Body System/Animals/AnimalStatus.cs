using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public class AnimalStatus : CreatureStatus
    {
        public bool isBleeding;
        public bool isSleeping;
        public new CreatureBody body;

        public float bmr;
        public float blood;
        public float bloodDeath;
        public float bloodConscious;
        public float oxygen;
        public float oxygenDeath;
        public float oxygenConscious;


        public AnimalStatus(CreatureBody ib) : base(ib)
        {
            pos = float3.zero;
            body = ib;
        }
        public AnimalStatus(CreatureBody ib, float3 ipos) : base(ib)
        {
            pos = ipos;
            body = ib;
        }

        public bool CanBreathe()
        {
            if (body.parts.dict[body.parts.breathingPart].isSubmerged)
            {
                return false;
            }
            else if (body.manager.transform.position.y > 8000)
            {
                return false;
            }
            return true;
        }

        /*Can't be alive if...
         * too little blood
         * too little oxygen
         * essential bodyparts destroyed
         */
        public new bool CheckCanBeAlive()
        {
            if (blood < bloodDeath)
            {
                return false;
            }

            if (oxygen < oxygenDeath)
            {
                return false;
            }

            foreach (BodyPart bp in body.parts.GetParts())
            {
                if (bp.essential && bp.integrity <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        public new bool CheckCanBeConcious()
        {
            if (blood < bloodConscious)
            {
                return false;
            }

            if (oxygen < oxygenConscious)
            {
                return false;
            }

            foreach (BodyPart bp in body.parts.GetParts())
            {
                if (bp.essential && bp.integrity <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        public new bool CheckCanMoveLimbs()
        {
            if (blood < bloodConscious)
            {
                return false;
            }

            if (oxygen < oxygenConscious)
            {
                return false;
            }

            foreach (BodyPart bp in body.parts.GetParts())
            {
                if (bp.essential && bp.integrity <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        public new bool CheckCanMove()
        {
            if (blood < bloodConscious)
            {
                return false;
            }

            if (oxygen < oxygenConscious)
            {
                return false;
            }

            foreach (BodyPart bp in body.parts.GetParts())
            {
                if (bp.essential && bp.integrity <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateStatus()
        {
            isExist = CheckCanExist();
            isAlive = CheckCanBeAlive();
            isConscious = CheckCanBeConcious() && !isSleeping;
            isLimbMobile = CheckCanMoveLimbs();
            isMobile = CheckCanMove();
        }
    }
}