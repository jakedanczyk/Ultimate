using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /*The representation of a Static. 
     * If the Static is under construction, a translucent WIP object with no collider is shown
     * 
     */
    public class StaticPrefab : MonoBehaviour
    {
        public string staticTypeName;
        public USTATIC staticType;
        public GameObject wipPrefab;
        public GameObject finalPrefab;
        //public UrthStatic ustatic;
        public int id;
        public string customName;
        public List<(UMATERIAL, float)> materialFractions = new List<(UMATERIAL, float)>();//((mat, %)
        public QUALITY designQuality = QUALITY.RUINED;
        public QUALITY buildQuality = QUALITY.RUINED;
        public float size;
        public int inventoryId;
        public UInventory inventory;
        public List<int> supportedStatics;
        public List<int> supportingStatics;

        //public ConstructionPillarSet pillarSet;
        //public ConstructionVolumeSet volumeSet;

        public ConstructionWorksite constructionWorksite;
        public bool isConstruction;
        public string constructionWorksiteId;
        public float designProgress;
        public float buildProgress;

        // Start is called before the first frame update
        void Start()
        {
            //if(ustatic != null)
            //{//use data to decide what to display

            //}
        }

        // Update is called once per frame
        void Update()
        {

        }

        //public void AssignData(UrthStatic newdata)
        //{
        //    ustatic = newdata;
        //}

        public void SetAsWip()
        {
            finalPrefab.SetActive(false);
            wipPrefab.SetActive(true);
        }

        public void SetAsFinal()
        {
            finalPrefab.SetActive(true);
            wipPrefab.SetActive(false);
        }

        public UMATERIAL PrimaryMaterial()
        {
            return materialFractions[0].Item1;
        }
    }

}