using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public enum TASK
    {
        IDLE,
        MOVE,
        HEALING,
        ATTACK,
        DEFEND,
        FLEE,
        HIDE,
        EAT,
        DRINK,
        REST,
        SLEEP,
        PANIC, //a fallback state, if multiple needs are failing and its not clear what the creature should do
    }

    //Goals are general, they are accomplished through specific tasks
    public enum GOAL
    {
        IDLE,
        SHELTER, //reach and remain within a shelter or hiding place
        REST,
        SLEEP,
        HIBERNATE,

        MIGRATE, //Move to a new location
        PATROL, //patrol territory at moderate pace

        FORAGE, //roam territory at slow pace, stopping to eat at available non-combat food sources
        HUNT, //find and kill a prey animal
        EAT, //
        DRINK, //

        ATTACK,
        DEFEND,
        EVADE,
        FLEE,

        SOCIALIZE,
        PLAY,
        WORK,

        STOP_BLEEDING,
        HEAL,
        OXYGEN,
        WARM_UP,

        PANIC //a fallback state, if multiple needs are failing and its not clear what the creature should do
    }

    public class ThreatPerception
    {
        public CreatureBody creature;
    }

    public struct CreatureScheduleEntry
    {
        public GOAL goal;
        public int dailyScheduleIdx;
        public double startTime;
    }

    public class CreatureMind
    {
        public bool wantsToSleep;
        public TASK task;
        public GOAL goal;
        public GOAL scheduleGoal;

        public Vector3 waypoint; //should always be within range of pathfinding radius
        public Vector3 destination; //
        public Vector3 travelDestination; // long distance where reference to world map routefinding is needed

        public bool wantsJump;
        public Vector3 jumpPoint = Vector3.zero;
        public Vector3 jumpTarget = Vector3.zero;

        public CreatureBody body;
        public CreatureNeeds needs;

        public CreatureBody targetCreature;
        public List<CreatureBody> threats;
        public Vector3 combatTargetPos;
        public bool wantsGuard;
        public bool wantsHoldLeft;
        public bool wantsHoldRight;
        public bool wantsStrikeLeft;
        public bool wantsStrikeRight;

        public static HashSet<TASK> stationaryTasks = new HashSet<TASK> { TASK.IDLE, TASK.REST, TASK.DRINK, TASK.EAT, TASK.REST, TASK.SLEEP };


        public NEED worstNeed; //least well met need
        public NEED_LEVEL worstNeedLevel;
        public Dictionary<NEED, NEED_LEVEL> needLevels;

        public void UpdateMind()
        {
            UpdateSchedule();
            UpdateGoal();
        }
        public void UpdateGoal()
        {
            /*If in combat, decide to continue or not
             *Else If any needs are serious or worse, deal with them in priority order
             *Else follow schedule
             */
            if (targetCreature != null)
            {
                UpdateCombatGoal();
                return;
            }

            (worstNeed, worstNeedLevel) = needs.CheckNeeds();
            if (worstNeedLevel >= NEED_LEVEL.SERIOUS)
            {
                switch (worstNeed)
                {
                    case NEED.COOLING:
                        goal = GOAL.STOP_BLEEDING;
                        break;
                    case NEED.BLOOD:
                        goal = GOAL.STOP_BLEEDING;
                        break;
                    case NEED.OXYGEN:
                        goal = GOAL.OXYGEN;
                        break;
                    case NEED.HEALING:
                        goal = GOAL.HEAL;
                        break;
                    case NEED.WATER:
                        goal = GOAL.DRINK;
                        break;
                    case NEED.HEAT:
                        goal = GOAL.WARM_UP;
                        break;
                    case NEED.SHELTER:
                        goal = GOAL.SHELTER;
                        break;
                    case NEED.THREAT:
                        goal = GOAL.EVADE;
                        break;
                    case NEED.FOOD:
                        goal = GOAL.EAT;
                        break;
                    default:
                        Debug.Log("Missing entry in critical-needs switch table " + worstNeed);
                        break;
                }
                return;
            }

            goal = scheduleGoal;
        }

        public void UpdateCombatGoal()
        {

        }
        public void UpdateNav()
        {
            switch (task)
            {
                case TASK t when (stationaryTasks.Contains(t)):
                    waypoint = this.body.manager.transform.position;
                    destination = this.body.manager.transform.position;
                    break;
                case TASK t when (stationaryTasks.Contains(t)):
                    waypoint = this.body.manager.transform.position;
                    destination = this.body.manager.transform.position;
                    break;


            }
        }
        public CreatureScheduleEntry currentScheduleEntry;
        public LinkedList<CreatureScheduleEntry> schedule;
        public void UpdateSchedule()
        {
            if (UrthTime.Instance.totalGameSeconds > schedule.First.Value.startTime)
            {
                currentScheduleEntry = schedule.First.Value;
                schedule.RemoveFirst();
            }
            scheduleGoal = currentScheduleEntry.goal;
        }
    }
}