using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public enum STATIC_TAG 
    {
        FOUNDATION,
        LOG,
        PILLAR
    }
    public class StaticTemplateComponent
    {
        public string name;
        public USTATIC type;
        //public int count;
        public float3 lwh;
        //public List<(ITEM_PROPERTY, (ITEM_PROPERTY, FUNCTION))> propRelationsList;
        //public Dictionary<ITEM_PROPERTY, (ITEM_PROPERTY, FUNCTION)> propRelationsDict;

        public StaticTemplateComponent(string iname, USTATIC itype, float3 ilwh)
        {

        }
    }

    public class StaticSupplyEntry
    {
        public ITEM type;
        public int count;
    }

    [System.Serializable]
    public struct UStaticTemplate
    {
        public ITEM supplyType;
        public int count;
    }

    /* StaticTemplates are recipes for UrthStatics
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
    public class StaticTemplate
    {
        public string name;
        public USTATIC staticType;
        public float3 lwh;
        public float filledVolume;
        public USAGE primaryUse;
        public HashSet<STATIC_TAG> tags;

        //public List<UItemTemplateComponent> subItems;
        public Dictionary<string, StaticTemplateComponent> componentsDict;


        public List<StaticSupplyEntry> supplies;

        public StaticTemplate()
        {
            tags = new HashSet<STATIC_TAG>();
            //subItems = new List<UItemTemplateComponent>();
            componentsDict = new Dictionary<string, StaticTemplateComponent>();
        }

        public void CalcStaticProps(StaticData staticData)
        {

        }
    }

}
