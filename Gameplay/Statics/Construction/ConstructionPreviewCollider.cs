using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class ConstructionPreviewCollider : MonoBehaviour
    {
        public List<Collider> collisionsList = new List<Collider>();

        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == GameManager.Instance.BUILDABLE_LAYER)
                collisionsList.Add(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == GameManager.Instance.BUILDABLE_LAYER)
                collisionsList.Remove(other);
        }
    }

}