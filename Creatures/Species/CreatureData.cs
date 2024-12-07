using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

/* CreatureData
 * Holds (mostly) fixed, non-RPG biological data
 * Such as species type, hair color, or body part size variation
 */

namespace Urth
{

    public enum CREATURE
    {
        ELF,
        HUMAN,
        DWARF,
        HALFLING,

        GOBLIN,
        ORC,

        //dogs
        ARTIC_FOX,
        FOX,
        COYOTE,
        WOLF,
        GRAY_WOLF,
        DIRE_WOLF,

        //bears
        SLOTH_BEAR,
        PANDA_BEAR,
        BLACK_BEAR,
        BROWN_BEAR,
        POLAR_BEAR,
        SHORT_FACED_BEAR,

        //cats

        //rodents
        MOUSE,
        RAT,
        SQUIRREL,
        HEDGEHOG,   
        PORCUPINE,
        MUSKRAT,
        BEAVER,

        //mustilidae
        WEASEL,
        MINK,
        MARTEN,
        FISHER,
        OTTER,
        BADGER,
        WOLVERINE,

        //other small mamals
        RACOON,



        //deer
        ROE_DEER,
        WHITETAIL_DEER,
        MULE_DEER,
        ELK,
        MOOSE,

        //llamasa
        ALPACA,
        LLAMA,
        CAMEL, //can sustain 7-10 

    
        //cows
        BUFFALO,
        BISON,
        AUROCH,

        //equines
        ZEBRA,
        WILDEBEEST,
        TAHKI_HORSE,
        HORSE,
        DRAFT_HORSE,

        //predatory dinors
        COMPSOGNATHUS,
        VELOCIRAPTOR,
        DEIONYCHUS,
        UTAHRAPTOR,
        ALLOSAURUS,
        TYRANNOSAURUS,

        //defensive dinos
        ANKLYOSAUR,
        STEGASAUR,
        TRICERATOPS,

        //saurapods
        IGUANADON,
        DIPLODACUS,
        APPATASAURUS,
        BRONTASAUR,


        //small birds

        //owls

        //raptor birds

    }

    public enum BODY_FORM
    {
        QUADRUPED,
        HUMANOID,
        DEER,
        CANINE,
        RODENT,
        THEROPOD,
        BIRD,
        FISH,
    }

    public enum GENDER
    {
        MALE,
        FEMALE,
        HERMAPHRODITE
    }

    public class BodyPlanNode
    {
        public BodyPartId bodyPartId;
        public List<BODY_PART> attached;

        public BodyPlanNode(BodyPartId ipart, List<BODY_PART> iattached = null)
        {
            bodyPartId = ipart; attached = iattached;
        }
    }

    public class BodyPlan
    {
        public List<BodyPlanNode> nodes;
        public BodyPlan(List<BodyPlanNode> inodes)
        {
            nodes = inodes;
        }
    }

    public class CreatureSpecies
    {
        public CREATURE type;
        public float density;//per sq km
        public float maxAge;
        public float genderRatio; //fraction male
        public List<(float, float)> maleGrowthCurve;
        public List<(float, float)> femaleGrowthCurve;
        public CREATURE_TEMPERATURE_MODEL temperatureModel;
        public BODY_FORM bodyForm;
        public Dictionary<string, Dictionary<string, float>> bodySizeAdjust;
        public List<(BodyPartId, float)> sweatRates;
        public List<string> tags;
        public Dictionary<BIOME, float> biomeDensityDict;
        public LinkedList<CreatureScheduleEntry> dailySchedule;
        public BodyPlan bodyplan;

        public CreatureSpecies()
        {
            biomeDensityDict = new Dictionary<BIOME, float>();
            sweatRates = new List<(BodyPartId, float)>();
        }

        float weightStrengthMult = 88.428f;
        float weightStrengthAdd = -266.49f;
        public float LeanWeightToBaseEncumbrance(float leanWeight)
        {
            return weightStrengthMult * Mathf.Log(leanWeight) + weightStrengthAdd;
        }
        float maxSizeMaxStrengthMult = 1f;
        float minSizeMaxStrengthMult = 5f/3f;
        float lowAdultLeanWeight = 50f;
        float highAdultLeanWeight = 100f;
        public float CalcMaxEncumbrance(int strength, float leanWeight)
        {
            float baseEncumbr = LeanWeightToBaseEncumbrance(leanWeight);
            float strengthMult = ((float)strength) / 20f;
            float sizeFrac = Mathf.Clamp01((leanWeight - lowAdultLeanWeight) / (highAdultLeanWeight - lowAdultLeanWeight));
            float sizeMult = Mathf.Lerp(minSizeMaxStrengthMult, maxSizeMaxStrengthMult, sizeFrac);
            return baseEncumbr * strengthMult * sizeMult;
        }
        public float CalcEffectiveStrength(int strength, float leanWeight)
        {
            float baseEncumbr = LeanWeightToBaseEncumbrance(leanWeight);
            float strengthMult = ((float)strength) / 20f;
            float sizeFrac = Mathf.Clamp01((leanWeight - lowAdultLeanWeight) / (highAdultLeanWeight - lowAdultLeanWeight));
            float sizeMult = Mathf.Lerp(minSizeMaxStrengthMult, maxSizeMaxStrengthMult, sizeFrac);
            return baseEncumbr * strengthMult * sizeMult;
        }
    }

    [System.Serializable]
    public class CreatureData
    {
        private int id;
        public int ID { 
            get { return this.id; }
            //set { this.id = value; }
        }
        public CREATURE type;
        public GENDER gender;
        public float birthTime;
        public float sizeGene;//0-1, from min to max for species

        public float weight;
        public float fatWeight;
        public float height;
        public float width;
        public float length;
        public float reach;
        public bool rightHanded;
        public float3 homepos;
        //public CreatureSpecies species;
        public CreatureData(int iid, CREATURE itype, GENDER igender, float ibirthTime, float iweight, float ilength, float iheight, float iwidth, float3 ihomepos)
        {
            id = iid; type = itype; gender = igender; birthTime = ibirthTime; weight = iweight; height = iheight; width = iwidth; length = ilength; homepos = ihomepos;
            
            reach = height;
        }
    }
}
