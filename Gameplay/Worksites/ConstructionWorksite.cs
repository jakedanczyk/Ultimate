using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public abstract class ConstructionWorksite : MonoBehaviour
    {
        public int id;//id of parent StaticPrefab
        public bool needsComponentUpdates;
        public bool supported;
        public StaticPrefab staticPrefab;
        //public UrthStatic ustatic;
        public float constructionProgress;
        public float supplyProgress;
        public float size;
        public float length;
        public float width;
        public float height;
        public float rotation;
        public float minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, minRotation, maxRotation, minSecondary, maxSecondary;
        public int minTertiary, maxTertiary;


        public bool hasSecondary;
        public bool hasTertiary;

        public CONSTRUCTION_METHOD constructionMethod;
        public float primaryVolume;
        public abstract USTATIC type { get;}

        public List<SupplyCount> supplies;

        // Start is called before the first frame update
        void Start()
        {
            staticPrefab = this.gameObject.GetComponent<StaticPrefab>();
            //ustatic = staticPrefab.ustatic;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UrthUpdate()
        {
            UpdateComponents();
            UpdateSuppliesNeeded();
        }

        public float PrimaryVolume()
        {
            return primaryVolume;
        }


        public void UpdateSupplyProgress()
        {
            UpdateSuppliesNeeded();
            foreach (int id in staticPrefab.inventory.GetIds())
            {
                UItemData itemData = staticPrefab.inventory.GetItem(id);
                for(int suppliesIdx=0; suppliesIdx<supplies.Count; suppliesIdx++)
                {
                    if (itemData.type == supplies[suppliesIdx].type)
                    {
                        SupplyCount sc = supplies[suppliesIdx];
                        sc.countActual += 1;
                        supplies[suppliesIdx] = sc;
                    }
                }
            }
        }

        public void CheckSupplyProgressOld()
        {
            ConstructionTemplateStruct ct = StaticsLibrary.Instance.ctStructDict[staticPrefab.staticType];
            ConstructionTemplateClass ctClass = StaticsLibrary.Instance.constructClassDict[staticPrefab.staticType];
            Dictionary<ITEM, int> count = new Dictionary<ITEM, int>();
            foreach (int id in staticPrefab.inventory.GetIds())
            {
                UItemData itemData = staticPrefab.inventory.GetItem(id);
                if (ctClass.suppliesDict.ContainsKey(itemData.type))
                {//if the ConstructTemplateClass supplies dict has a listing for an item of this type...
                 //add or increment the count for that type
                    if (count.ContainsKey(itemData.type))
                    {
                        count[itemData.type] = count[itemData.type] + 1;
                    }
                    else
                    {
                        count[itemData.type] = 1;
                    }
                }
            }

            float low = 1f;
            foreach (ITEM itemType in ctClass.suppliesDict.Keys)
            {
                int neededCount = ctClass.suppliesDict[itemType];
                //(int)(supplyCount.count * this.size + 0.5f);
                int haveCount = count[itemType];
                float frac = ((float)haveCount) / neededCount;
                if (frac < low)
                {
                    low = frac;
                }
            }
            supplyProgress = low;
        }

        public void Adjust()
        {
            //wipPrefab.transform.localScale = Vector3.one * data.size;
            //finalPrefab.transform.localScale = Vector3.one * data.size;
            //if (data.supplyProgress >= 1.0f)
            //{
            //    wipPrefab.SetActive(false);
            //    finalPrefab.SetActive(true);
            //}
            //else
            //{
            //    wipPrefab.SetActive(true);
            //    finalPrefab.SetActive(false);
            //}

        }

        public abstract void UpdateSuppliesNeeded();

        /*UpdateComponents updates the sizes and positions of meshes and colliders 
         *which are determined by factors such as surrounding terrain and settings such as length/width/height
         */
        public abstract void UpdateComponents();
        public abstract void UpdateComponentsFinal();


        public virtual float SetHeight(float h) { return height = h; }
        public virtual float GetHeight() { return height; }

        public virtual float SetSecondaryValue(float f) { return 0f; }
        public virtual float GetSecondaryValue() { return 0f; }
        public virtual (float,float) GetSecondaryMinMax() { return (0f, 10f); }

        public virtual int SetTertiaryValue(int n) { return 0; }
        public virtual int GetTertiaryValue() { return 0; }
        public virtual (int,int) GetTertiaryMinMax() { return (0,10); }

        //public abstract List<(string, )>
    }
}