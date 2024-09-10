using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{

    public class ConstructionTemplateComponent
    {
        public string name;
        public USTATIC type;
        //public int count;
        public float3 lwh;
        //public List<(ITEM_PROPERTY, (ITEM_PROPERTY, FUNCTION))> propRelationsList;
        //public Dictionary<ITEM_PROPERTY, (ITEM_PROPERTY, FUNCTION)> propRelationsDict;

        public ConstructionTemplateComponent(string iname, USTATIC itype, float3 ilwh)
        {

        }
    }

    public class ConstructionSupplyEntry
    {
        public ITEM type;
        public int count;
    }

    [System.Serializable]
    public struct UConstructionTemplate
    {
        public ITEM supplyType;
        public int count;
    }

    /* ConstructionTemplates are recipes for Constructions
     * they provide a baseline, allowing variance among the individual statics within the game world
     * The variation can be in both presentation and game mechanics
     * -varied size
     * -texture
     * -subcomponents also (hinges, handles, surface coating... anything)
     * 
     * They set the scale, position, and material for the 1 or more meshes that make up an UrthStatic
     * 
     * They also provide a way to detail how the material type and quality of subcomponents affect the overall quality
     * The quality of a Static can affect its load capacity, resistance to damage, or other attributes
     */
    [System.Serializable]
    public class ConstructionTemplate
    {
        public string name;
        public USTATIC staticType;
        public float3 lwh;
        public float filledVolume;
        public USAGE primaryUse;
        public HashSet<STATIC_TAG> tags;

        //public List<UItemTemplateComponent> subItems;
        public Dictionary<string, StaticTemplateComponent> componentsDict;


        //public List<StaticSupplyEntry> supplies;
        public string typeName;
        public List<float> sizeSuppliesMult;
        public List<SupplyCount> supplies;
        public List<SupplyCount> pillarSupplies;
        public float difficulty;
        public float labor;                                                                             
        public ConstructionTemplate()
        {
            tags = new HashSet<STATIC_TAG>();
            //subItems = new List<UItemTemplateComponent>();
            componentsDict = new Dictionary<string, StaticTemplateComponent>();
            sizeSuppliesMult = new List<float>(5);
        }

        public void CalcStaticProps(StaticData staticData)
        {

        }

        public List<SupplyCount> SizedSupplies(STATIC_SIZE size)
        {
            int idx = (int)size;
            float mult = sizeSuppliesMult[idx];
            //float mult = 1f;
            //switch (size)
            //{
            //    case STATIC_SIZE.HALF:
            //        mult = 0.5f;
            //        break;
            //    case STATIC_SIZE.THREE_QUARTERS:
            //        mult = 0.75f;
            //        break;
            //    case STATIC_SIZE.FULL:
            //        mult = 1f;
            //        break;
            //    case STATIC_SIZE.THREE_HALVES:
            //        mult = 1.5f;
            //        break;
            //    case STATIC_SIZE.DOUBLE:
            //        mult = 2f;
            //        break;
            //}
            List<SupplyCount> sizedSupplies = new List<SupplyCount>(supplies.Count);
            int loopIdx = 0;
            foreach(SupplyCount supplyItemCount in supplies)
            {
                sizedSupplies[loopIdx] = new SupplyCount((int)(supplyItemCount.countNeeded * mult));
            }
            return sizedSupplies;
        }
    }

}
