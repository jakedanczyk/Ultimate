using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Unity.Mathematics;

namespace Urth
{

    [System.Serializable]
    public struct PlantPrefab
    {
        public PLANT type;
        public GameObject prefab;
    }

    [System.Serializable]
    public class PlantsLibrary : MonoBehaviour
    {
        public static PlantsLibrary Instance { get; private set; }
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

        public PlantsManager manager;

        // Start is called before the  first frame update
        void Start()
        {
            prefabsDict = new Dictionary<PLANT, GameObject>(prefabs.Count);
            foreach (PlantPrefab plantPrefab in prefabs)
            {
                prefabsDict[plantPrefab.type] = plantPrefab.prefab;
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

        public List<PlantPrefab> prefabs;
        public Dictionary<PLANT, GameObject> prefabsDict;


        XmlDocument speciesXmlFile;
        public Dictionary<PLANT, PlantSpecies> speciesDict;
        public Dictionary<BIOME, List<PLANT>> biomeSpecies;
        public Dictionary<BIOME, Dictionary<PLANT, float>> biomeSpeciesDensities;
        public Dictionary<BIOME, Dictionary<PLANT, float>> biomeSpeciesSizeAdjustments;
        public Dictionary<BIOME, Dictionary<PLANT, float>> biomeSpeciesFractions;
        public Dictionary<BIOME, float> biomeTotalCoverFractions;
        public Dictionary<BIOME, bool> biomesHaveLargePlants;
        void PopulateSpecies()
        {
            speciesXmlFile = new XmlDocument();
            speciesXmlFile.Load(System.IO.Path.Combine(Application.dataPath, "World/Plants/PlantSpeciesData.xml"));
            speciesDict = new Dictionary<PLANT, PlantSpecies>(speciesXmlFile.DocumentElement.ChildNodes.Count);
            biomeSpecies = new Dictionary<BIOME, List<PLANT>>(50);
            biomeSpeciesDensities = new Dictionary<BIOME, Dictionary<PLANT, float>>(50);
            biomeSpeciesSizeAdjustments = new Dictionary<BIOME, Dictionary<PLANT, float>>(50);
            biomeSpeciesFractions = new Dictionary<BIOME, Dictionary<PLANT, float>>(50);
            biomeTotalCoverFractions = new Dictionary<BIOME, float>(50);
            biomesHaveLargePlants = new Dictionary<BIOME, bool>(50);
            //read species XML
            foreach (XmlNode speciesNode in speciesXmlFile.DocumentElement.ChildNodes)
            {
                XmlElement speciesElement = (XmlElement)speciesNode;
                PlantSpecies species = new PlantSpecies();
                species.type = (PLANT)System.Enum.Parse(typeof(PLANT), speciesNode.Attributes["type"].InnerText);
                //species.maxAge = float.Parse(speciesNode.Attributes["maxAge"].InnerText);
                species.maxAge = float.Parse(speciesElement["maxAge"].InnerText);
                species.growthCurve = new List<(float, float)>();
                foreach (var ageWeight in speciesElement["growthCurve"].InnerText.Split(':'))
                {
                    var s = ageWeight.Substring(1, ageWeight.Length - 2);
                    var ageWeightTuple = s.Split(',');
                    species.growthCurve.Add((float.Parse(ageWeightTuple[0]), float.Parse(ageWeightTuple[1])));
                }
                //TODO before implementing "old age shrinkage", update this
                species.maxHeight = species.growthCurve[species.growthCurve.Count - 1].Item2;

                speciesDict[species.type] = species;
                foreach (XmlNode biomeNode in speciesElement["biomes"].ChildNodes)
                {
                    BIOME biome = (BIOME)System.Enum.Parse(typeof(BIOME), biomeNode.Attributes["biome-id"].InnerText);
                    float popPerSqKm = float.Parse(biomeNode["density"].InnerText);
                    species.biomeDensityDict.Add(biome, popPerSqKm);
                    float adultSizeInBiome = float.Parse(biomeNode["adultSizeInBiome"].InnerText);
                    species.biomeSizeDict.Add(biome, adultSizeInBiome);
                    if (!biomeSpecies.ContainsKey(biome))
                    {
                        biomeSpecies.Add(biome, new List<PLANT>(100));
                        biomeSpeciesDensities.Add(biome, new Dictionary<PLANT, float>(100));
                        biomeSpeciesSizeAdjustments.Add(biome, new Dictionary<PLANT, float>(100));
                    }
                    biomeSpecies[biome].Add(species.type);
                    biomeSpeciesDensities[biome][species.type] = popPerSqKm;
                    biomeSpeciesSizeAdjustments[biome][species.type] = adultSizeInBiome;
                }
            }
            //calculate density of plant cover in different biomes
            foreach (BIOME biome in biomeSpecies.Keys)
            {
                biomesHaveLargePlants[biome] = biomeSpeciesSizeAdjustments[biome].Count > 0;
                float totalSpaceFilledInBiome = 0;

                //find total area covered by plants per sq km
                foreach (PLANT plantInBiome in biomeSpecies[biome])
                {
                    float adultSizeInBiome = biomeSpeciesSizeAdjustments[biome][plantInBiome];
                    float densityInBiome = biomeSpeciesDensities[biome][plantInBiome];
                    float spaceFilled = (adultSizeInBiome * densityInBiome) * 1E-06f;//divide by 1 million because plant sizes in sq m
                    totalSpaceFilledInBiome += spaceFilled;
                }
                biomeTotalCoverFractions[biome] = totalSpaceFilledInBiome * 1E-06f;

                //find fraction of area covered by each plant
                foreach (PLANT plantInBiome in biomeSpecies[biome])
                {
                    float adultSizeInBiome = biomeSpeciesSizeAdjustments[biome][plantInBiome];
                    float densityInBiome = biomeSpeciesDensities[biome][plantInBiome];
                    float spaceFilledBySpecies = (adultSizeInBiome * densityInBiome) * 1E-06f;//divide by 1 million because plant sizes in sq m
                    float fraction = spaceFilledBySpecies / totalSpaceFilledInBiome;
                    if (!biomeSpeciesFractions.ContainsKey(biome))
                    {
                        biomeSpeciesFractions.Add(biome, new Dictionary<PLANT, float>(100));
                    }
                    biomeSpeciesFractions[biome][plantInBiome] = fraction;
                }
            }
        }
    }
}
