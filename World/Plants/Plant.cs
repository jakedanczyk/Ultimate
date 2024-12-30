using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Urth
{
    public enum PLANT
    {
        MOUNTAIN_JUNIPER,
        COMMON_JUNIPER,
        CREEPING_JUNIPER,

        RED_CEDAR,

        LARCH,
        ALPINE_LARCH,
        TAMARACK,

        FIR,
        GRAND_FIR,
        DOUGLAS_FIR,

        MOUNTAIN_SPRUCE,
        WHITE_SPRUCE,

        BRISTLECONE_PINE,
        LIMBER_PINE,
        LODGEPOLE_PINE,
        GREY_PINE,
        PONDEROSA_PINE,
        STONE_PINE,
        WHITE_PINE, //western white pine
        WHITEBARK_PINE,

        HEMLOCK,
        MOUNTAIN_HEMLOCK,

        YEW,
        MOUNTAIN_YEW,

        PAPER_BIRCH,

        ASPEN,
        COTTONWOOD,
        POPLAR,
        BALSAM_POPLAR,

        ELM,
        BEECH,
        OAK,

        MAHOGANY,

        SAGE,
        SAGEBRUSH,

        SERVICEBERRY,
        HUCKLEBERRY,
        ELDERBERRY,
        THORN,

        PALM,

        EUCALYPTUS,
        RAINBOW_EUCALYPTUS,
    }

    public class PlantSpecies
    {
        public PLANT type;
        public float maxAge;
        public float maxHeight;
        public List<(float, float)> growthCurve;
        public List<string> tags;
        public Dictionary<BIOME, float> biomeDensityDict;
        public Dictionary<BIOME, float> biomeSizeDict;
        public float slenderness; //height divided by width
        public PlantSpecies()
        {
            biomeDensityDict = new Dictionary<BIOME, float>();
            biomeSizeDict = new Dictionary<BIOME, float>();
        }
    }

    public class PlantPatchData
    {//reduce data usage for smaller plants by grouping them in patches
        //they will use the same size with small random variation
        public PLANT type;
        public float sproutTime;
        public List<float3> positions;
        public PlantPatchData(PLANT it, float ia, List<float3> ip)
        {
            type = it; sproutTime = ia; positions = ip;
        }
    }

    public class PlantData
    {
        private int id;
        public int ID
        {
            get { return this.id; }
            //set { this.id = value; } 
        }
        public PLANT type;
        public float height;
        public float width;
        public float sproutTime;
        public float3 pos;
        public PlantData(int iid, PLANT it, float iheight, float iw, float isproutTime, float3 ip)
        {
            id = iid; type = it; height = iheight; width = iw; sproutTime = isproutTime; pos = ip;
        }
    }


    public class Plant : MonoBehaviour
    {
        //public PlantsManager plantsManager;
        public PlantData data;
        public PlantSpecies species;
        public PlantWorksite worksite;
        // Start is called before the first frame update
        void Start()
        {
            species = PlantsManager.Instance.plantsLibrary.speciesDict[data.type];
            this.transform.localScale = data.height / PlantsManager.Instance.plantsLibrary.speciesDict[data.type].maxHeight * Vector3.one;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}