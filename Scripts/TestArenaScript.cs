using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{

    public class TestArenaScript : MonoBehaviour
    {
        public List<UItem> items;
        public void Setup()
        {
            foreach(UItem item  in items)
            {
                item.data.pos = new float3(item.transform.position) + GameManager.Instance.gameWorldOffset;
                ItemsManager.Instance.AddCustomItem(item.data);
            }
        }
    }

}