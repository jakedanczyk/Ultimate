using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    //TODO move the different MakeBillboards into the abstract class. There are some slight differences to be resolved

    public abstract class PlantTile : MonoBehaviour
    {
        public Dictionary<PLANT, PlantTileBillboard> billboards { get; }
        public List<BillboardPrefab> billboardPrefabs;


        public void TurnOnBillboards()
        {
            foreach (PLANT type in billboards.Keys)
            {
                billboards[type].gameObject.SetActive(true);
            }
        }
        public void TurnOffBillboards()
        {
            foreach (PLANT type in billboards.Keys)
            {
                billboards[type].gameObject.SetActive(false);
            }
        }
    }

}