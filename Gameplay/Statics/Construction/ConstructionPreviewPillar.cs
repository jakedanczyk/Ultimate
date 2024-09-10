using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Attach this to a child transform of a Construction Preview in the 
 * location of a pillar support.
 * Also drag-reference it into the preview's PillarSupports list
 * while the Preview is active it will call CheckSupport each frame
 * CheckSupport will spherecast downwards, and search for a support surface
 * 
 * This is used to allow variable heights for the pillar sub-components of some 
 * construction objects when building on uneven surface
 */
namespace Urth
{

    public class ConstructionPreviewPillar : MonoBehaviour
    {
        public float radius;
        public float maxHeight;
        public RaycastHit aimHit;
        public bool supported;
        public float supportHeight;
        public List<GameObject> sections;

        public void UpdatePillar()
        {
            if (Physics.SphereCast(this.transform.position, .1f, Vector3.down, out aimHit, maxHeight, ConstructionLibrary.Instance.constructionLayers))
            {
                supported = true;
                Debug.DrawLine(this.transform.position, aimHit.point, Color.green);
                supportHeight = aimHit.distance;
            }
            else
            {
                supported = false;
                supportHeight = maxHeight;
            }

            UpdateSections();
        }
        void UpdateSections()
        {

        }
    }

}