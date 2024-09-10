using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UltimateTerrains;
using UltimateTerrains.Pathfinder;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

/// <summary>
/// A simple behaviour to test path finding. Press 'I' to set start point, move where you want, and then press 'I' again to
/// find path between the start point and your position.
/// </summary>
public class NavPathAgent : MonoBehaviour
{
    private UltimateTerrain terrain;
    private readonly List<GameObject> cubes = new List<GameObject>();

    private Vector3d start;
    private Vector3d end;

    public void Awake()
    {
        this.terrain = UltimateTerrain.Main;
    }

    public Vector3d Start
    {
        get { return start; }
        set { start = value; }
    }

    public Vector3d End
    {
        get { return end; }
        set { end = value; }
    }


    //
    // Summary:
    //     Is the agent currently positioned on an OffMeshLink? (Read Only)
    public bool isOnOffMeshLink { get; }
    //
    // Summary:
    //     Should the agent move across OffMeshLinks automatically?
    public bool autoTraverseOffMeshLink { get; set; }
    //
    // Summary:
    //     Should the agent brake automatically to avoid overshooting the destination point?
    public bool autoBraking { get; set; }
    //
    // Summary:
    //     Should the agent attempt to acquire a new path if the existing path becomes invalid?
    public bool autoRepath { get; set; }
    //
    // Summary:
    //     Does the agent currently have a path? (Read Only)
    public bool hasPath { get; }
    //
    // Summary:
    //     Is a path in the process of being computed but not yet ready? (Read Only)
    public bool pathPending { get; }
    //
    // Summary:
    //     Is the current path stale. (Read Only)
    public bool isPathStale { get; }
    //
    // Summary:
    //     The status of the current path (complete, partial or invalid).
    public UnityEngine.AI.NavMeshPathStatus pathStatus { get; }
    //
    // Summary:
    //     The status of the current path (complete, partial or invalid).
    //public NavMeshPathStatus pathStatus { get; }
    public Vector3 pathEndPosition { get; }
    //
    // Summary:
    //     This property holds the stop or resume condition of the NavMesh agent.
    public bool isStopped { get; set; }
    //
    // Summary:
    //     Property to get and set the current path.
    //public NavMeshPath path { get; set; }
    //
    // Summary:
    //     The next OffMeshLinkData on the current path.
    public OffMeshLinkData nextOffMeshLinkData { get; }
    //
    // Summary:
    //     Returns the owning object of the NavMesh the agent is currently placed on (Read
    //     Only).
    public Object navMeshOwner { get; }
    //
    // Summary:
    //     Specifies which NavMesh layers are passable (bitfield). Changing walkableMask
    //     will make the path stale (see isPathStale).
    public int walkableMask { get; set; }
    //
    // Summary:
    //     Specifies which NavMesh areas are passable. Changing areaMask will make the path
    //     stale (see isPathStale).
    public int areaMask { get; set; }
    //
    // Summary:
    //     Maximum movement speed when following a path.
    public float speed { get; set; }
    //
    // Summary:
    //     Maximum turning speed in (deg/s) while following a path.
    public float angularSpeed { get; set; }
    //
    // Summary:
    //     The maximum acceleration of an agent as it follows a path, given in units / sec^2.
    public float acceleration { get; set; }
    //
    // Summary:
    //     Gets or sets whether the transform position is synchronized with the simulated
    //     agent position. The default value is true.
    public bool updatePosition { get; set; }
    //
    // Summary:
    //     Should the agent update the transform orientation?
    public bool updateRotation { get; set; }
    //
    // Summary:
    //     Allows you to specify whether the agent should be aligned to the up-axis of the
    //     NavMesh or link that it is placed on.
    public bool updateUpAxis { get; set; }
    //
    // Summary:
    //     The avoidance radius for the agent.
    public float radius { get; set; }
    //
    // Summary:
    //     The height of the agent for purposes of passing under obstacles, etc.
    public float height { get; set; }
    //
    // Summary:
    //     The level of quality of avoidance.
    //public ObstacleAvoidanceType obstacleAvoidanceType { get; set; }
    //
    // Summary:
    //     The type ID for the agent.
    public int agentTypeID { get; set; }
    //
    // Summary:
    //     The current OffMeshLinkData.
    public OffMeshLinkData currentOffMeshLinkData { get; }
    //
    // Summary:
    //     Is the agent currently bound to the navmesh? (Read Only)
    public bool isOnNavMesh { get; }
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
    //
    // Summary:
    //     The avoidance priority level.
    public int avoidancePriority { get; set; }

    public float pathfinderStep = 1f;
    public float pathfinderMaxSlope = 0.5f;
    public bool pathfinderGroundOnly = true;

    public PathFinder.Result path;
    public SearchNode nextNode;

    public void UpdateNode()
    {
        nextNode = nextNode.Next;
    }

    public void UpdatePath()
    {
        StartCoroutine(this.PathCoroutine(pathfinderStep, pathfinderMaxSlope, pathfinderGroundOnly));
    }


    public IEnumerator PathCoroutine(double step = 1.0, double maxSlope = 0.7, bool aboveGroundOnly = true)
    {
        // Just make sure start point and end point are above the ground
        if (aboveGroundOnly)
        {
            EnsureStartEndPointsAreAboveTheGround(step, maxSlope);
        }



        //PathFinder.Result path;
        if (aboveGroundOnly)
        {
            path = terrain.FindPathAsync(start, end, step, maxSlope);
        }
        else
        {
            path = terrain.FindPathInAirAsync(start, end, step);
        }

        // Wait for the result
        while (!path.Done)
        {
            yield return null;
        }


        if (path.Found)
        {
            nextNode = path.FirstNode;
        }
    }
    public IEnumerator DebugPathCoroutine(double step = 1.0, double maxSlope = 0.7, bool aboveGroundOnly = true)
    {
        // Just make sure start point and end point are above the ground
        if (aboveGroundOnly)
        {
            EnsureStartEndPointsAreAboveTheGround(step, maxSlope);
        }

        Debug.Log(string.Format("Searching path from {0} to {1}...", start, end));

        var watch = Stopwatch.StartNew();

        //PathFinder.Result path;
        if (aboveGroundOnly)
        {
            path = terrain.FindPathAsync(start, end, step, maxSlope);
        }
        else
        {
            path = terrain.FindPathInAirAsync(start, end, step);
        }

        // Wait for the result
        while (!path.Done)
        {
            yield return null;
        }

        watch.Stop();

        if (path.Found)
        {
            Debug.Log(string.Format("PATH FOUND (in less than {0}ms)", watch.ElapsedMilliseconds));
            
            nextNode = path.FirstNode;
            var currentNode = path.FirstNode;
            while (currentNode != null)
            {
                var cube = CreateCubeAtNode(currentNode);
                cubes.Add(cube);
                currentNode = currentNode.Next;
            }
        }
        else
        {
            Debug.Log(string.Format("PATH NOT FOUND (in less than {0}ms)", watch.ElapsedMilliseconds));
        }
    }

    private GameObject CreateCubeAtNode(SearchNode currentNode)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = terrain.Converter.VoxelToUnityPosition(currentNode.Position);
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        cube.GetComponent<Collider>().enabled = false;
        cube.GetComponent<Renderer>().material.color = Color.yellow;
        cube.layer = 2;
        return cube;
    }

    private void EnsureStartEndPointsAreAboveTheGround(double step, double maxSlope)
    {
        while (start.y > -200 && start.y < 200)
        {
            start.y -= step * maxSlope;
            var voxel = terrain.GetVoxelAt(start);
            if (voxel.IsInside)
            {
                start.y += step * maxSlope;
                break;
            }
        }

        while (end.y > -200 && end.y < 200)
        {
            end.y -= step * maxSlope;
            var voxel = terrain.GetVoxelAt(end);
            if (voxel.IsInside)
            {
                end.y += step * maxSlope;
                break;
            }
        }
    }


    public void ClearCubes()
    {
        foreach (var cube in cubes)
        {
            Object.Destroy(cube);
        }

        cubes.Clear();
    }

    //
    // Summary:
    //     Completes the movement on the current OffMeshLink.
    public void CompleteOffMeshLink() 
    { 
    }

    //
    // Summary:
    //     Sets or updates the destination thus triggering the calculation for a new path.
    //
    // Parameters:
    //   target:
    //     The target point to navigate to.
    //
    // Returns:
    //     True if the destination was requested successfully, otherwise false.
    public bool SetDestination(Vector3 target)
    {
        destination = target;
        start = (Vector3d)this.transform.position;
        end = (Vector3d)destination;
        UpdatePath();
        return true;
    }
}

