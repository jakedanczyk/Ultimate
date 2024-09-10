using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public enum LWH_KEY
    {
        LENGTH,
        WIDTH,
        HEIGHT
    }

    public struct UPropertyOp
    {
        public float factor;
        public string key;
    }

    public class UItemTemplateComponent
    {
        public string name;
        public ITEM type;
        //public int count;
        public float3 lwh;
        //public List<(ITEM_PROPERTY, (ITEM_PROPERTY, FUNCTION))> propRelationsList;
        //public Dictionary<ITEM_PROPERTY, (ITEM_PROPERTY, FUNCTION)> propRelationsDict;
        
        public UItemTemplateComponent(string iname, ITEM itype, float3 ilwh)
        {

        }
    }

    /* ItemTemplates are 'recipes' for items, 
     * they assist in allowing items to vary in size and texture, 
     * including sub-components such as handles, surface coating, etc
     * 
     * They set the scale, position, and material for the 1 or more meshes that make up an Item
     * 
     * They also provide a way to have a mix of generic and custom stats for items
     * For example, the Pickaxe template ensures even a poor quality pick is a much superior 
     * tool for breaking earth than a sword or wooden club
     */
    public class UItemTemplate
    {
        public string componentName;
        public ITEM type;
        public USAGE primaryUse;
        public HashSet<ITEM_TAG> tags;
        public HashSet<USAGE> altUsages;
        public Dictionary<WORKTASK, float> worktaskScores;
        public Dictionary<WORKSITE, WORKTASK> preferredTasks;

        //public List<UItemTemplateComponent> subItems;
        public Dictionary<string, UItemTemplateComponent> componentsDict;

        public Dictionary<LWH_KEY, List<(UPropertyOp, LWH_KEY)>> lwhDict;//foreach each lwh_key, a list of pairs of values. A ItemPropertyOp (that has a multiplier factor and key and for which component to look at) and a lwh_key which says which dimension of the selected component to use
        //for example, for most hafted tools, the length is set by the haft length, the tool heads "length" becomes the assemblies height, and the tool heads width becomes the assemblies width
        //for a spear, length is haft.length + head.length
        public List<UPropertyOp> volumeList;


        public UItemTemplate()
        {
            tags = new HashSet<ITEM_TAG>();
            altUsages = new HashSet<USAGE>();
            worktaskScores = new Dictionary<WORKTASK, float>();
            //subItems = new List<UItemTemplateComponent>();
            componentsDict = new Dictionary<string, UItemTemplateComponent>();
        }

        public void CalcItemProps(UItemData itemData)
        {
            float vol = 0;
            foreach(UPropertyOp volumeOp in volumeList)
            {
                if(volumeOp.key == "self")
                {
                    vol += volumeOp.factor + itemData.volumeComponent;
                }
                else
                {
                    vol += volumeOp.factor + itemData.components[volumeOp.key].volumeAssembled;
                }
            }
            foreach(LWH_KEY key in lwhDict.Keys)
            {
                int assemblyLwhIdx = 0;
                switch (key)
                {
                    case LWH_KEY.LENGTH:
                        assemblyLwhIdx = 0;
                        break;
                    case LWH_KEY.WIDTH:
                        assemblyLwhIdx = 1;
                        break;
                    case LWH_KEY.HEIGHT:
                        assemblyLwhIdx = 2;
                        break;
                }
                itemData.lwhAssembled[assemblyLwhIdx] = 0;
                foreach(var pair in lwhDict[key])
                {
                    int componentLwhIdx = 0;

                    switch(pair.Item2)
                    {
                        case LWH_KEY.LENGTH:
                            componentLwhIdx = 0;
                            break;
                        case LWH_KEY.WIDTH:
                            componentLwhIdx = 1;
                            break;
                        case LWH_KEY.HEIGHT:
                            componentLwhIdx = 2;
                            break;
                    }
                    UPropertyOp propOp = pair.Item1;
                    itemData.lwhAssembled[assemblyLwhIdx] += propOp.factor * itemData.components[propOp.key].lwhAssembled[componentLwhIdx];
                }
            }
        }
    }

}
