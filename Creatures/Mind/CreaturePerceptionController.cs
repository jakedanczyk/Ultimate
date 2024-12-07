using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public class Perception
    {
        public float3 lastLocation;
        public float3 lastVector; //objects travel direction in m/s
    }

    public class PerceivedCreature : Perception
    {
        public CreatureManager manager;
    }

    public class PerceivedItem : Perception
    {
        public UItem item;
    }




    public class CreaturePerceptionStats
    {
        public float hearingSensitivity; //dB
        public float periVisionResolution; //degrees of angle
        public float binoVisionResolution; //degrees of angle
        public float periphVisionAngle;
        public float binoVisionAngle;

        public CreaturePerceptionStats(float ihs, float ipvr, float ibvr, float ipva, float ibva)
        {
            hearingSensitivity = ihs; periVisionResolution = ipvr; binoVisionResolution = ibvr; periphVisionAngle = ipva; binoVisionAngle = ibva;
        }
    }




    /*Simulate perception
     * Each creature has a list of potential objects of interest
     * Periodically it checks if those are resolvable (by size and distance), 
     * then raycasts to check for obstacles
     * then determines what it can see/hear
     * 
     * Keep track of last known positions of targets
     * 
     * 
     * Vision can work by allowing each creature to have a list of objects of interest
     * Checking by size and distance if that object is resolvable
     * Then raycasting to check for sight obstacles
     * 
     * Or... broadcasting. Sources can "broadcast" themselves to eyes/ears within range
     */

    public class CreaturePerception
    {
        public CreatureBody creatureBody;
        public CreaturePerceptionStats stats;
        public List<CreatureOrgan> eyes;
        public List<CreatureOrgan> ears;
        public List<CreatureOrgan> noses;
    }

    /*Mono class for in-engine simulation of perception
     */
    public class CreaturePerceptionController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            time = UrthTime.Instance;
        }

        float lastRefresh;
        float creatureScanRate; //number of checks per frame needed to complete scan within scanperiod
        int creatureScanCount = 0;
        int creatureScanFramesCount = 0;
        RaycastHit hit;
        // Update is called once per frame
        void FixedUpdate()
        {
            if (creatureScanFramesCount > scanPeriodFrames + 1)
            {
                UpdateDetectableCreatures();
                creatureScanFramesCount = 0;
                creatureScanCount = 0;
                creatureScanRate = detectableCreatures.Count / scanPeriodFrames;
            }
            if (detectableCreatures.Count > 0)
            {
                int creatureScansNeeded = (int)(creatureScanCount + creatureScanRate + 0.5f);
                for (int i = creatureScanCount; i < creatureScansNeeded; i++)
                {
                    CreatureManager creature = detectableCreatures[i];
                    Vector3 vec = Vector3.up + creature.transform.position - this.transform.position;
                    if (Physics.Raycast(visionTransform.position, vec, out hit, detectionRange, GameManager.Instance.VISION_MASK))
                    {
                        if (hit.collider.TryGetComponent(out CreatureManager hitCm))
                        { // raycast hit the creature and not something else. creature is "visible"
                            if (perceivedCreaturesDict.TryGetValue(hitCm.id, out PerceivedCreature perceivedCreature))
                            {
                                perceivedCreature.lastLocation = hit.collider.transform.position;
                                if (hit.collider.TryGetComponent(out Rigidbody rb))
                                {
                                    perceivedCreature.lastVector = rb.velocity;
                                }
                            }
                        }
                    }
                }
            }
        }

        UrthTime time;
        public Transform visionTransform;
        public Transform hearingTransform;
        public CreatureManager creatureManager;
        public CreaturePerception creaturePerception;

        public HashSet<int> detectableCreatureIdsSet;
        public List<CreatureManager> detectableCreatures;
        public Dictionary<int, PerceivedCreature> perceivedCreaturesDict;
        //public List<CreaturePerception> detectedCreatures;
        public HashSet<int> detectedCreatureIdsSet;
        Collider[] detectionCols = new Collider[1000];
        public void UpdateDetectableCreatures()
        {
            int count = Physics.OverlapSphereNonAlloc(this.transform.position, detectionRange, detectionCols, GameManager.Instance.CREATURE_MASK);
            detectableCreatures = new List<CreatureManager>(count);
            detectableCreatureIdsSet = new HashSet<int>();
            foreach (Collider col in detectionCols)
            {
                if (col.TryGetComponent(out CreatureManager cm))
                {
                    if (detectableCreatureIdsSet.Add(cm.id))
                    {
                        detectableCreatures.Add(cm);
                    }
                }
            }
        }

        float scanPeriodSeconds;
        float scanPeriodFrames;
        public void UpdateScanPeriod()
        {
            scanPeriodSeconds = 1f;
            scanPeriodFrames = scanPeriodSeconds * 50f;
        }

        float detectionRange;
        public void UpdateDetectionRange()
        {
            detectionRange = 200f;
        }

    }
}