using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class ConstructionPreviewSupportCollider : MonoBehaviour
    {
        public List<Collider> trigList = new List<Collider>();

        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == GameManager.Instance.BUILDABLE_LAYER)
                trigList.Add(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == GameManager.Instance.BUILDABLE_LAYER)
                trigList.Remove(other);
        }
    }

}