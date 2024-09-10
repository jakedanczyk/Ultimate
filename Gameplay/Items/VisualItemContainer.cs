using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{

    public class VisualItemContainer : MonoBehaviour
    {
        public List<UItem> items;
        // Start is called before the first frame update
        void Start()
        {
            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            foreach(UItem item in items)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.parent = this.transform;
                item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }

        public void AddItem(UItem item)
        {
            items.Add(item);
        }

    }

}