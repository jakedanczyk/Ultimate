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



        public float PrimaryVolume()
        {
            return primaryVolume;
        }


        public void CheckSupplyProgress()
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

        public abstract void UpdateComponents();
        public abstract void UpdateComponentsFinal();
    }
}