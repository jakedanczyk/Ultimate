using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsBasedCharacterController;
using UnityEngine.AI;

namespace Urth
{

    public enum LOCAL_NAV
    {
        NULL,
        NAVMESH,
        PATHFINDER
    }

    /* Controls an NPC (activities, motion, combat, etc.)
     * 
     * Queries CreatureManager to make decisions
     * Commands NPCInputReader
     */

    public class NPCController : MonoBehaviour
    {
        public CreatureManager manager;
        public CreatureMind mind;
        public CreatureBody body;
        public NPCInputReader inputReader;

        public LOCAL_NAV nav;
        public NavMeshAgent navMeshAgent;
        public PathTester pathTester;
        public NavPathAgent navPathAgent;

        public float sqrNavRadius = 5f;

        //
        // Summary:
        //     The relative vertical displacement of the owning GameObject.
        public float baseOffset { get; set; }
        //
        // Summary:
        //     The distance between the agent's position and the destination on the current
        //     path. (Read Only)
        public float remainingDistance { get; }
        //
        // Summary:
        //     The desired velocity of the agent including any potential contribution from avoidance.
        //     (Read Only)
        public Vector3 desiredVelocity { get; }
        //
        // Summary:
        //     Get the current steering target along the path. (Read Only)
        public Vector3 steeringTarget { get; }
        //
        // Summary:
        //     Gets or sets the simulation position of the navmesh agent.
        public Vector3 nextPosition { get; set; }
        //
        // Summary:
        //     Access the current velocity of the NavMeshAgent component, or set a velocity
        //     to control the agent manually.
        public Vector3 velocity { get; set; }
        //
        // Summary:
        //     Stop within this distance from the target position.
        public float stoppingDistance { get; set; }
        //
        // Summary:
        //     Gets or attempts to set the destination of the agent in world-space units.
        public Vector3 destination { get; set; }

        Vector3 nextWaypoint;
        Vector3 toNextWaypoint;
        Vector3 goalWaypoint;

        private void Awake()
        {
            manager = GetComponent<CreatureManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            mind = manager.mind;
            body = manager.body;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateNav();
            //We are using Animal Controller for now
            //Vector3 togo = nextWaypoint - this.transform.position;
            //inputReader.OnLook(togo.normalized);
            //inputReader.OnMove(togo);
            //inputReader.OnJump(mind.wantsJump, mind.jumpPoint, mind.jumpTarget);
            //inputReader.OnSetPosture(body.status.posture);
            //inputReader.OnSetGait(body.status.gait);
            //inputReader.OnGuard(mind.wantsGuard);
            //if (mind.wantsHoldLeft)
            //{
            //    inputReader.OnLeftHold();
            //}
            //else if (mind.wantsStrikeLeft)
            //{
            //    inputReader.OnLeftStrike();
            //}
            //if (mind.wantsHoldRight)
            //{
            //    inputReader.OnRightHold();
            //}
            //else if (mind.wantsStrikeLeft)
            //{
            //    inputReader.OnRightStrike();
            //}
        }

        public void AddNav()
        {
            nav = GameManager.Instance.scene == SCENE_TYPE.MESH_ARENA ? LOCAL_NAV.NAVMESH : LOCAL_NAV.PATHFINDER;
            switch (nav)
            {
                case LOCAL_NAV.NAVMESH:
                    navMeshAgent = this.gameObject.AddComponent<NavMeshAgent>();
                    break;
                case LOCAL_NAV.PATHFINDER:
                    navPathAgent = this.gameObject.AddComponent<NavPathAgent>();
                    break;
                default:
                    break;
            }
        }

        int idx = 0;
        void UpdateNav()
        {
            switch (nav)
            {
                case (LOCAL_NAV.NAVMESH):
                    nextWaypoint = navMeshAgent.destination;
                    break;
                case LOCAL_NAV.PATHFINDER:
                    //UltimateTerrains.Pathfinder.SearchNode nextNode
                    if (navPathAgent.nextNode == null)
                    {
                        idx++;
                        if (idx == 10)
                        {
                            idx = 0;
                            navPathAgent.Start = (UltimateTerrains.Vector3d)this.transform.position;
                            navPathAgent.End = (UltimateTerrains.Vector3d)(this.mind.destination);
                            navPathAgent.UpdatePath();
                        }
                    }
                    else
                    {
                        nextWaypoint = (Vector3)navPathAgent.nextNode.Position;
                        toNextWaypoint = nextWaypoint - this.transform.position;
                        if (toNextWaypoint.sqrMagnitude < sqrNavRadius)
                        {
                            if (navPathAgent.nextNode.Next != null)
                            {
                                navPathAgent.nextNode = navPathAgent.nextNode.Next;
                            }
                            else
                            {
                                navPathAgent.Start = (UltimateTerrains.Vector3d)this.transform.position;
                                navPathAgent.End = (UltimateTerrains.Vector3d)(this.transform.position + Vector3.forward * 100f);
                                navPathAgent.UpdatePath();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void SetManager(CreatureManager imanager)
        {
            manager = imanager;
            mind = manager.mind;
            body = manager.body;
        }
    }
}
