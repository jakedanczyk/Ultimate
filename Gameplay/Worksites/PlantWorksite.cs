using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class PlantWorksite : MonoBehaviour
    {
        public PlantWorksiteData data;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AttachData(PlantWorksiteData idata)
        {
            data = idata;
        }
    }
}