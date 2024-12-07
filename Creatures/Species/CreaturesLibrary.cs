using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Unity.Mathematics;

namespace Urth
{

    [System.Serializable]
    public struct CreaturePrefab
    {
        public CREATURE type;
        public GameObject prefab;
    }

    [System.Serializable]
    public class CreaturesLibrary : MonoBehaviour
    {
        public static CreaturesLibrary Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public CreaturesManager creaturesManager;

        // Start is called before the  first frame update
        void Start()
        {
            prefabsDict = new Dictionary<CREATURE, GameObject>(prefabs.Count);
            foreach (CreaturePrefab creaturePrefab in prefabs)
            {
                prefabsDict[creaturePrefab.type] = creaturePrefab.prefab;
            }
            PopulateSpecies();
            //Populate();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Populate()
        {
            if (speciesDict is null)
            {
                PopulateSpecies();
                //PopulatePrefabsDict();
            }
        }

        public List<CreaturePrefab> prefabs;
        public Dictionary<CREATURE, GameObject> prefabsDict;

        public static BodyPartId headBpid = new BodyPartId(BODY_PART.HEAD);
        public static BodyPartId neckBpid = new BodyPartId(BODY_PART.NECK);
        public static BodyPartId chestBpid = new BodyPartId(BODY_PART.CHEST);
        public static BodyPartId abdomenBpid = new BodyPartId(BODY_PART.ABDOMEN);
        public static BodyPartId pelvisBpid = new BodyPartId(BODY_PART.PELVIS);
        public static BodyPartId lShoulderBpid = new BodyPartId(BODY_PART.SHOULDER, SYMMETRY.LEFT);
        public static BodyPartId rShoulderBpid = new BodyPartId(BODY_PART.SHOULDER, SYMMETRY.RIGHT);
        public static BodyPartId lUpperArmBpid = new BodyPartId(BODY_PART.UPPER_ARM, SYMMETRY.LEFT);
        public static BodyPartId rUpperArmBpid = new BodyPartId(BODY_PART.UPPER_ARM, SYMMETRY.RIGHT);
        public static BodyPartId lForearmBpid = new BodyPartId(BODY_PART.FOREARM, SYMMETRY.LEFT);
        public static BodyPartId rForearmBpid = new BodyPartId(BODY_PART.FOREARM, SYMMETRY.RIGHT);
        public static BodyPartId lHandBpid = new BodyPartId(BODY_PART.HAND, SYMMETRY.LEFT);
        public static BodyPartId rHandBpid = new BodyPartId(BODY_PART.HAND, SYMMETRY.RIGHT);

        public static BodyPartId lThighBpid = new BodyPartId(BODY_PART.THIGH, SYMMETRY.LEFT);
        public static BodyPartId rThighBpid = new BodyPartId(BODY_PART.THIGH, SYMMETRY.RIGHT);
        public static BodyPartId lKneeBpid = new BodyPartId(BODY_PART.KNEE, SYMMETRY.LEFT);
        public static BodyPartId rKneeBpid = new BodyPartId(BODY_PART.KNEE, SYMMETRY.RIGHT);
        public static BodyPartId lCalfBpid = new BodyPartId(BODY_PART.CALF, SYMMETRY.LEFT);
        public static BodyPartId rCalfBpid = new BodyPartId(BODY_PART.CALF, SYMMETRY.RIGHT);
        public static BodyPartId lAnkleBpid = new BodyPartId(BODY_PART.ANKLE, SYMMETRY.LEFT);
        public static BodyPartId rAnkleBpid = new BodyPartId(BODY_PART.ANKLE, SYMMETRY.RIGHT);
        public static BodyPartId lFootBpid = new BodyPartId(BODY_PART.FOOT, SYMMETRY.LEFT);
        public static BodyPartId rFootBpid = new BodyPartId(BODY_PART.FOOT, SYMMETRY.RIGHT);

        public static BodyPartId tailBpid = new BodyPartId(BODY_PART.TAIL);


        public static BodyPlanNode headNode = new BodyPlanNode(headBpid, new List<BODY_PART> { BODY_PART.NECK });
        public static BodyPlanNode neckNode = new BodyPlanNode(neckBpid, new List<BODY_PART> { BODY_PART.CHEST, BODY_PART.HEAD });
        public static BodyPlanNode chestNode = new BodyPlanNode(chestBpid, new List<BODY_PART> { BODY_PART.NECK, BODY_PART.SHOULDER, BODY_PART.ABDOMEN });
        public static BodyPlanNode abdomenNode = new BodyPlanNode(abdomenBpid, new List<BODY_PART> { BODY_PART.CHEST, BODY_PART.PELVIS });
        public static BodyPlanNode pelvisNode = new BodyPlanNode(pelvisBpid, new List<BODY_PART> { BODY_PART.ABDOMEN, BODY_PART.THIGH });
        public static BodyPlanNode rShoulderNode = new BodyPlanNode(rShoulderBpid, new List<BODY_PART> { BODY_PART.CHEST, BODY_PART.UPPER_ARM });
        public static BodyPlanNode lShoulderNode = new BodyPlanNode(lShoulderBpid, new List<BODY_PART> { BODY_PART.CHEST, BODY_PART.UPPER_ARM });
        public static BodyPlanNode rUpperArmNode = new BodyPlanNode(rUpperArmBpid, new List<BODY_PART> { BODY_PART.SHOULDER, BODY_PART.FOREARM });
        public static BodyPlanNode lUpperArmNode = new BodyPlanNode(lUpperArmBpid, new List<BODY_PART> { BODY_PART.SHOULDER, BODY_PART.FOREARM });
        public static BodyPlanNode rForearmNode = new BodyPlanNode(rForearmBpid, new List<BODY_PART> { BODY_PART.UPPER_ARM, BODY_PART.HAND });
        public static BodyPlanNode lForearmNode = new BodyPlanNode(lForearmBpid, new List<BODY_PART> { BODY_PART.UPPER_ARM, BODY_PART.HAND });
        public static BodyPlanNode rHandNode = new BodyPlanNode(rHandBpid, new List<BODY_PART> { BODY_PART.FOREARM });
        public static BodyPlanNode lHandNode = new BodyPlanNode(lHandBpid, new List<BODY_PART> { BODY_PART.FOREARM });
        public static BodyPlanNode rThighNode = new BodyPlanNode(rThighBpid, new List<BODY_PART> { BODY_PART.PELVIS, BODY_PART.KNEE });
        public static BodyPlanNode lThighNode = new BodyPlanNode(lThighBpid, new List<BODY_PART> { BODY_PART.PELVIS, BODY_PART.KNEE });
        public static BodyPlanNode rKneeNode = new BodyPlanNode(rKneeBpid, new List<BODY_PART> { BODY_PART.THIGH, BODY_PART.CALF });
        public static BodyPlanNode lKneeNode = new BodyPlanNode(lKneeBpid, new List<BODY_PART> { BODY_PART.THIGH, BODY_PART.CALF });
        public static BodyPlanNode rCalfNode = new BodyPlanNode(rCalfBpid, new List<BODY_PART> { BODY_PART.KNEE, BODY_PART.ANKLE });
        public static BodyPlanNode lCalfNode = new BodyPlanNode(lCalfBpid, new List<BODY_PART> { BODY_PART.KNEE, BODY_PART.ANKLE });
        public static BodyPlanNode rAnkleNode = new BodyPlanNode(rAnkleBpid, new List<BODY_PART> { BODY_PART.CALF, BODY_PART.FOOT });
        public static BodyPlanNode lAnkleNode = new BodyPlanNode(lAnkleBpid, new List<BODY_PART> { BODY_PART.CALF, BODY_PART.FOOT });
        public static BodyPlanNode rFootNode = new BodyPlanNode(rFootBpid, new List<BODY_PART> { BODY_PART.CALF });
        public static BodyPlanNode lFootNode = new BodyPlanNode(lFootBpid, new List<BODY_PART> { BODY_PART.CALF });
        public static BodyPlanNode tailNode = new BodyPlanNode(tailBpid, new List<BODY_PART> { BODY_PART.PELVIS });

        public static BodyPlan bodyPlanQuadruped = new BodyPlan(new List<BodyPlanNode> {
            headNode,
            neckNode,
            chestNode,
            abdomenNode,
            pelvisNode,
            lShoulderNode,
            rShoulderNode,
            lUpperArmNode,
            rUpperArmNode,
            lForearmNode,
            rForearmNode,
            lHandNode,
            rHandNode,
            lThighNode,
            rThighNode,
            lKneeNode,
            rKneeNode,
            lCalfNode,
            rCalfNode,
            lAnkleNode,
            rAnkleNode,
            lFootNode,
            rFootNode,
            tailNode
        });

        public static BodyPlan bodyPlanHumanoid = new BodyPlan(new List<BodyPlanNode> {
            headNode,
            neckNode,
            chestNode,
            abdomenNode,
            pelvisNode,
            lShoulderNode,
            rShoulderNode,
            lUpperArmNode,
            rUpperArmNode,
            lForearmNode,
            rForearmNode,
            lHandNode,
            rHandNode,
            lThighNode,
            rThighNode,
            lKneeNode,
            rKneeNode,
            lCalfNode,
            rCalfNode,
            lAnkleNode,
            rAnkleNode,
            lFootNode,
            rFootNode,
        });

        public Dictionary<BODY_FORM, BodyPlan> bodyPlans = new Dictionary<BODY_FORM, BodyPlan>() {
            { BODY_FORM.HUMANOID, bodyPlanHumanoid },
            { BODY_FORM.CANINE, bodyPlanQuadruped },
            { BODY_FORM.DEER, bodyPlanQuadruped },
            { BODY_FORM.RODENT, bodyPlanQuadruped },
            { BODY_FORM.THEROPOD, bodyPlanQuadruped },
        };


        XmlDocument speciesXmlFile;
        public Dictionary<CREATURE, CreatureSpecies> speciesDict;
        public Dictionary<BIOME, List<CREATURE>> biomeSpecies;
        void PopulateSpecies()
        {
            speciesXmlFile = new XmlDocument();
            speciesXmlFile.Load(System.IO.Path.Combine(Application.dataPath, "Creatures/Species/CreatureSpeciesData.xml"));
            speciesDict = new Dictionary<CREATURE, CreatureSpecies>(speciesXmlFile.DocumentElement.ChildNodes.Count);
            biomeSpecies = new Dictionary<BIOME, List<CREATURE>>(50);
            foreach (XmlNode speciesNode in speciesXmlFile.DocumentElement.ChildNodes)
            {
                XmlElement speciesElement = (XmlElement)speciesNode;
                CreatureSpecies species = new CreatureSpecies();
                species.type = (CREATURE)System.Enum.Parse(typeof(CREATURE), speciesNode.Attributes["type"].InnerText);
                //species.maxAge = float.Parse(speciesNode.Attributes["maxAge"].InnerText);
                species.maxAge = float.Parse(speciesElement["maxAge"].InnerText);
                species.maleGrowthCurve = new List<(float, float)>();
                foreach (var ageWeight in speciesElement["growthCurveMale"].InnerText.Split(':'))
                {
                    var ageWeightTuple = ageWeight.Split(',');
                    species.maleGrowthCurve.Add((float.Parse(ageWeightTuple[0]), float.Parse(ageWeightTuple[1])));
                }
                species.femaleGrowthCurve = new List<(float, float)>();
                foreach (var ageWeight in speciesElement["growthCurveFemale"].InnerText.Split(':'))
                {
                    var ageWeightTuple = ageWeight.Split(',');
                    species.femaleGrowthCurve.Add((float.Parse(ageWeightTuple[0]), float.Parse(ageWeightTuple[1])));
                }
                species.temperatureModel = (CREATURE_TEMPERATURE_MODEL)System.Enum.Parse(typeof(CREATURE_TEMPERATURE_MODEL), speciesElement["temperatureModel"].InnerText);
                species.bodyForm = (BODY_FORM)System.Enum.Parse(typeof(BODY_FORM), speciesElement["bodyForm"].InnerText);
                species.bodyplan = bodyPlans[species.bodyForm];

                species.bodySizeAdjust = new Dictionary<string, Dictionary<string, float>>();
                foreach (var partData in speciesElement["bodySizeAdjust"].InnerText.Split(':'))
                {
                    string[] partArr = partData.Split(';');
                    string part = partArr[0];
                    species.bodySizeAdjust.Add(part, new Dictionary<string, float>());
                    for (int i = 1; i < partArr.Length; i++)
                    {
                        string[] adj = partArr[i].Split(',');
                        species.bodySizeAdjust[part].Add(adj[0], float.Parse(adj[1]));
                    }
                }
                speciesDict[species.type] = species;
                foreach (XmlNode biomeNode in speciesElement["biomes"].ChildNodes)
                {

                    BIOME biome = (BIOME)System.Enum.Parse(typeof(BIOME), biomeNode.Attributes["biome-id"].InnerText);
                    float popPerSqKm = float.Parse(biomeNode["density"].InnerText);
                    species.biomeDensityDict.Add(biome, popPerSqKm);
                    if (!biomeSpecies.ContainsKey(biome))
                    {
                        biomeSpecies.Add(biome, new List<CREATURE>(100));
                    }
                    biomeSpecies[biome].Add(species.type);
                }
            }
        }
    }
}