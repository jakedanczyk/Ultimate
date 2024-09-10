using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Urth
{
    [Serializable]
    public class StaticData
    {
        public USTATIC type;
        public StaticTemplate template;
        public UMATERIAL material;
        public QUALITY quality;
        public UrthStatic item;

        public float3 pos;
        public float3 facing;
        public bool assembled;
        public float volumeComponent;
        public float3 lwhComponent;
        public float volumeAssembled;
        public float3 lwhAssembled;
        public float longestDim;

        //public List<UItemData> subItems;
        public Dictionary<string, StaticData> components;
        public float weightComponent;
        public float weightAssembled;
        public int id;
        public int ID
        {
            get { return this.id; }
            //set { this.id = value; } 
        }
        public float Weight { get { return weightAssembled; } }
        public float Length { get { return lwhAssembled.x; } }
        public float Width { get { return lwhAssembled.y; } }
        public float Height { get { return lwhAssembled.z; } }


        public List<EquippedBodyPartLayer> layers;

        public string GetName()
        {
            return quality.ToString() + " " + material.ToString() + " " + type.ToString();
        }

        public StaticData()
        {
            id = -1;
            type = 0;
            quality = 0;
            weightComponent = 0;
            weightAssembled = 0;
            lwhComponent = float3.zero;
            material = 0;
        }

        //public float CalcWeightAssembled()
        //{
        //    weightAssembled = subWeight;
        //    foreach (UItemData subItem in subItems)
        //    {
        //        subItem.CalcWeightAssembled();
        //        weightAssembled += subItem.weightAssembled;
        //    }
        //    return weightAssembled;
        //}
        //public float CalcDimsAssembled()
        //{
        //    weightAssembled = subWeight;
        //    foreach (UItemData subItem in subItems)
        //    {
        //        subItem.CalcWeightAssembled();
        //        weightAssembled += subItem.weightAssembled;
        //    }
        //    return weightAssembled;
        //}

        public void CalcProperties()
        {//calculate current properties, assembled or not
            template.CalcStaticProps(this);
        }
    }

    [Serializable]
    public class UrthStatic
    {
        public string id;
        public string name;
        public USTATIC type;
        public float size;
        public int inventoryId;
        public UInventory inventory;
        public bool isConstruction;
        public string constructionWorksiteId;
        public List<string> supportedStatics;
        public List<string> supportingStatics;

        public UrthStatic(string nid, string nname, USTATIC ntype, float nsize, bool nisCons)
        {
            id = nid;
            name = nname;
            type = ntype;
            size = nsize;
            isConstruction = nisCons;
            (inventoryId, inventory) = InventoriesManager.Instance.NextInventory();
        }

        public void SetSupported(List<string> i)
        {
            supportedStatics = i;
        }
        public void SetSupporting(List<string> i)
        {
            supportingStatics = i;
        }
    }
}