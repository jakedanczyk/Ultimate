using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Xml;
using System;

namespace Urth
{
    public enum USTATIC
    {
        FOUNDATION_LOG_PIER_LOG,
        FOUNDATION_LOG_PIER_STONE,
        FOUNDATION_PILLAR_METAL,
        FOUNDATION_PILLAR_STONE_AND_LOG,
        FOUNDATION_PILLAR_STONE_AND_METAL,
        FOUNDATION_SOLID_EARTH,
        FOUNDATION_SOLID_LOG,
        FOUNDATION_SOLID_STONE,
        WALL_EARTH,
        WALL_STONE,
        WALL_LOG,
        WALL_TIMBERS,
        WALL_FRAMED,
        WALL_WATTLE,
        WALL_WINDOW_EARTH,
        WALL_WINDOW_STONE,
        WALL_WINDOW_LOG,
        WALL_WINDOW_TIMBERS,
        WALL_WINDOW_FRAMED,
        WALL_WINDOW_WATTLE,
        WALL_DOORWAY_EARTH,
        WALL_DOORWAY_STONE,
        WALL_DOORWAY_LOG,
        WALL_DOORWAY_TIMBERS,
        WALL_DOORWAY_FRAMED,
        WALL_DOORWAY_WATTLE,
        ROOF_LOG,
        ROOF_LOG_EARTH,
        ROOF_LOG_TILES,
        ROOF_TIMBERS,
        ROOF_TIMBERS_EARTH,
        ROOF_TIMBERS_TILES,
        ROOF_FRAMED,
        ROOF_WATTLE,
        ROOF_THATCH,
        FLOOR_LOG,
        FLOOR_TIMBERS,
        FLOOR_STONE,
        FLOOR_TILES,
        DOOR_LOG,
        DOOR_TIMBERS,
        DOOR_METAL,
        WATTLE_WALL,
        WATTLE_WALL_WINDOW,
        WATTLE_WALL_DOORWAY,
        FRAMED_WALL,
        FRAMED_WALL_WINDOW,
        FRAMED_WALL_DOORWAY,
        SOLID_WALL,
        SOLID_WALL_WINDOW,
        SOLID_WALL_DOORWAY,
        PILLAR_ROUND,
        PILLAR_SQUARE,
        BEAM_ROUND,
        BEAM_SQUARE,
        FOUNDATION,
        DOOR,
        WINDOW,
        LATCH,
        FLOOR,
        ROOF,
        CHIMNEY,
        FIRE_RING,
        FIRE_PLACE,
        OVEN,
        BLOOMERY_FURNACE,
        TABLE,
        STOOL,
        CHAIR,
        COUCH,
        BED,
        PIT,
        QUERNSTONE,
        ANVIL,
        VISE,
    }

    public enum STATIC_SIZE
    {
        HALF,
        THREE_QUARTERS,
        FULL,
        THREE_HALVES,
        DOUBLE
    }

    [System.Serializable]
    public struct SupplyCount
    {//this struct is setup for display in Inspector
        public int countNeeded;
        public int countActual;
        public string itemName;
        public ITEM type; //this field should not be set manually. During initialization, these structs are read and this field is set based on the itemName string

        public SupplyCount(int v) : this()
        {
            V = v;
        }
        public SupplyCount(int icn, int ica, ITEM itype) : this()
        {
            countNeeded = icn;
            countActual = ica;
            type = itype;
        }
        public int V { get; }
    }

    [System.Serializable]
    public struct ConstructionTemplateStruct
    {//this struct is setup for display in Inspector
        public string typeName;
        public List<SupplyCount> supplies;
        public float difficulty;
        public float labor;
    }

    public class ConstructionTemplateClass
    {//at initialization, the struct data is copied turned into objects of this class//
     //there is a dictionary with ITEM keys and integer values of the number needed
        public USTATIC type;
        public Dictionary<ITEM, int> suppliesDict;

        public ConstructionTemplateClass(USTATIC itype)
        {
            type = itype;
            suppliesDict = new Dictionary<ITEM, int>();
        }
    }


    [System.Serializable]
    public class StaticsLibrary : MonoBehaviour
    {
        public static StaticsLibrary Instance { get; private set; }
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

        void Start()
        {
            SetupPrefabs();
            SetupConstructionTemplates();

            PopulateStaticTemplates();
        }

        public SitesManager sitesManager;
        public List<ConstructionTemplateStruct> constTemplateList;
        public List<StaticSupplyList> supplies;
        //public List<UStaticTemplate> utemplates;
        public List<StaticTemplate> templates;
        public List<StaticPrefab> prefabsList;
        public Dictionary<USTATIC, ConstructionTemplateStruct> ctStructDict;
        public Dictionary<USTATIC, ConstructionTemplateClass> constructClassDict;
        public Dictionary<USTATIC, StaticPrefab> prefabsDict;
        //public Dictionary<STATIC, StaticBaseData> baseData = new Dictionary<STATIC, StaticBaseData>()
        //{
        //    { STATIC.LOG_FOUNDATION, new StaticBaseData(0.25f, ITEM.WOOD)},
        //    { STATIC.LOG_WALL, new StaticBaseData(0.25f, ITEM.WOOD) },
        //    { STATIC.LOG_WALL_DOORWAY, new StaticBaseData(0.10f, ITEM.WOOD) },
        //    { STATIC.LOG_DOOR, new StaticBaseData(0.05f, ITEM.WOOD) },
        //};

        void SetupPrefabs()
        {
            prefabsDict = new Dictionary<USTATIC, StaticPrefab>(prefabsList.Count);
            foreach (StaticPrefab staticPrefab in prefabsList)
            {
                if (System.Enum.TryParse(staticPrefab.staticTypeName, out USTATIC stype))
                {
                    staticPrefab.staticType = stype;
                    prefabsDict[stype] = staticPrefab;
                }
            }
        }

        void SetupConstructionTemplates()
        {
            constructClassDict = new Dictionary<USTATIC, ConstructionTemplateClass>(constTemplateList.Count);
            ctStructDict = new Dictionary<USTATIC, ConstructionTemplateStruct>(constTemplateList.Count);
            foreach (ConstructionTemplateStruct constTemplateStruct in constTemplateList)
            {//iterate the constructs list and put struct data into class dictionary
                if (System.Enum.TryParse(constTemplateStruct.typeName, out USTATIC stype))
                {
                    ConstructionTemplateClass ctClass = new ConstructionTemplateClass(stype);
                    int i = 0;
                    for (i = 0; i < constTemplateStruct.supplies.Count; i++)
                    {//iterate the list of supplies for this construct
                        SupplyCount sc = constTemplateStruct.supplies[i];
                        if (System.Enum.TryParse(sc.itemName, out ITEM itype))
                        {
                            sc.type = itype;//the ctStruct are setup manually with a string name, the ITEM type field is not set manually, but rather here
                            ctClass.suppliesDict[itype] = sc.countNeeded;//set the count in the dict in the ctClass
                        }
                        constTemplateStruct.supplies[i] = sc;//update sc entry in the ctStruct
                    }
                    constructClassDict[stype] = ctClass; //update ctClass dict
                    ctStructDict[stype] = constTemplateStruct; //update ctStruct dict
                }
            }
        }

        XmlDocument templatesXmlFile;
        public Dictionary<USTATIC, StaticTemplate> templatesDict;
        void PopulateStaticTemplates()
        {
            templatesXmlFile = new XmlDocument();
            templatesXmlFile.Load(System.IO.Path.Combine(Application.dataPath, UrthConstants.STATIC_TEMPLATES_XML));
            templatesDict = new Dictionary<USTATIC, StaticTemplate>(templatesXmlFile.DocumentElement.ChildNodes.Count);
            foreach (XmlNode templateNode in templatesXmlFile.DocumentElement.ChildNodes)
            {
                XmlElement templateElement = (XmlElement)templateNode;
                StaticTemplate template = new StaticTemplate();
                template.staticType = (USTATIC)System.Enum.Parse(typeof(USTATIC), templateNode.Attributes["type"].InnerText);
                template.name = templateNode["name"].InnerText;
                if (templateElement["tags"].InnerText.Length > 0)
                {
                    foreach (var tagStr in templateElement["tags"].InnerText.Split(','))
                    {
                        STATIC_TAG tag = (STATIC_TAG)System.Enum.Parse(typeof(STATIC_TAG), tagStr.ToUpper());
                        template.tags.Add(tag);
                    }
                }
               
                templatesDict[template.staticType] = template;
            }
        }

    }
}