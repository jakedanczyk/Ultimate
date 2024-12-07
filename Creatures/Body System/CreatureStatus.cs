using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/* CreatureStatus
 * Holds varying, non-RPG data
 * Such as position, posture, and status effects
 */
namespace Urth
{
    public enum GAME_STATUS
    {
        DISTANT,//rest of continent.
        NEAR, //not implemented but idea is to have donut of more frequent updates around perimiter of loaded area
        RENDERED, //Visible but moving by lerping, no character controller
        ACTIVE,//character controller
    }
    public enum POSTURE
    {
        SUPINE,
        PRONE,
        SIT,
        KNEES,
        CROUCH,
        STAND,
        SWIM,
        FLY
    }

    public enum GAIT
    {
        IDLE,
        SLOWEST,//creeping walk
        SLOWER,//slow walk
        SLOW,//casual walk
        SEMI_SLOW,//walk
        MODERATE,//power walk
        SEMI_FAST,//jog
        FAST,//run
        FASTER,//fast run
        FASTEST//sprint
    }

    public enum GAIT_NAMES
    {
        BELLY_CRAWL,
        CRAWL,
        SCRAMBLE,
        LOW_CROUCH,
        CROUCH,
        CROUCHED_SPRINT,
        SLOW_WALK,
        WALK,
        STRIDE,
        JOG,
        RUN,
        SPRINT,
        SWIM,
        FLY
    }


    public enum STATUS
    {

    }

    public class CreatureStatus : ICreatureStatus
    {
        public CreatureBody body;

        public bool isExist;
        public bool isAlive;
        public bool isConscious;
        public bool isLimbMobile;
        public bool isMobile;
        public float weight;

        public float3 pos;
        public float3 facing;
        public float3 dimensions;

        public POSTURE posture;
        public GAIT gait;
        public GAME_STATUS gameStatus;

        public Dictionary<STATUS, bool> statusFlags;

        public CreatureStatus(CreatureBody ib)
        {
            body = ib;
        }
        public virtual void UpdateNeeds()
        {
        }
        public bool CheckCanExist() { return true; }
        public bool CheckCanBeAlive() { return true; }
        public bool CheckCanBeConcious() { return true; }
        public bool CheckCanMoveLimbs() { return true; }
        public bool CheckCanMove() { return true; }
        public void UpdatePos(float t)
        {
            switch (gameStatus)
            {
                case GAME_STATUS.ACTIVE:
                    pos = GameManager.Instance.GetWorldPos(body.manager.transform.position);
                    break;
                case GAME_STATUS.DISTANT:
                    body.navigation.UpdateMovement(t);
                    GameManager.Instance.GetWorldPos(body.manager.transform.position);
                    break;

            }
        }
    }
}