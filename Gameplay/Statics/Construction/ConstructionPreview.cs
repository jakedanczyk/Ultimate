using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class ConstructionPreview : MonoBehaviour
    {
        public string staticTypeName;
        public USTATIC staticType;
        public CONSTRUCTION_METHOD constructionMethod;
        public CONSTRUCTION_SUPPORT_LAYERS supportLayer;//what kind of support is this object

        public bool isBuildable = false;
        public bool isActive = false;
        public List<Collider> collisionsList = new List<Collider>();
        public List<ConstructionPreviewCollider> clearanceColliders = new List<ConstructionPreviewCollider>();
        public List<ConstructionPreviewCollider> supportColliders = new List<ConstructionPreviewCollider>();
        public List<ConstructionPreviewPillar> pillarSupports = new List<ConstructionPreviewPillar>();

        public ConstructionWorksite constructionWorksite;
        public ConstructionPreviewPillarSet pillarSet;
        public ConstructionPreviewFloorSupport floorSupport;
        
        public ConstructionPreviewSupportCollider supportCollider;

        public Transform graphics;


        void Start()
        {

        }

        void Update()
        {
            if (isActive)
            {
                CheckBuildable();
                ChangeColor();
            }
        }

        //void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.layer == GameManager.Instance.BUILDABLE_LAYER)
        //        collisionsList.Add(other);
        //}

        //void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.layer == GameManager.Instance.BUILDABLE_LAYER)
        //        collisionsList.Remove(other);
        //}


        int clearanceCollisionsCount = 0;
        int supportsCount = 0;
        bool supported = true;//start true, set to false once proven
        public void CheckBuildable()
        {
            Debug.Log("Checking buildable");
            clearanceCollisionsCount = 0;
            foreach (ConstructionPreviewCollider collider in clearanceColliders)
            {
                clearanceCollisionsCount += collider.collisionsList.Count;
            }


            supported = true;
            if(supportColliders.Count > 0)
            {
                foreach(ConstructionPreviewCollider support in supportColliders)
                {
                    if(support.collisionsList.Count == 0)
                    {
                        supported = false;
                    }
                }
            }

            if(constructionWorksite != null && constructionWorksite.needsComponentUpdates)
            {
                Debug.Log("construction stuff");
                constructionWorksite.UpdateComponents();
                constructionWorksite.UpdateSuppliesNeeded();
                if (!constructionWorksite.supported)
                {
                    supported = false;
                }
            }
            if(pillarSet != null)
            {
                pillarSet.UpdatePillars();
                if (!pillarSet.supported)
                {
                    supported = false;
                }
            }
            if(floorSupport != null)
            {
                floorSupport.UpdateFloor();
                if (!floorSupport.supported)
                {
                    supported = false;
                }
            }

            isBuildable = true;
            if(clearanceCollisionsCount > 0)
            {
                isBuildable = false;
            }
            else if (!supported)
            {
                isBuildable = false;
            }
        }

        void CountClearanceCollisions()
        {

        }
        void CheckBuildableFoundation()
        {

        }

        Renderer renderer;
        public void ChangeColor()
        {
            if (isBuildable)
            {
                foreach (Transform child in graphics)
                {
                    if (child.TryGetComponent(out renderer))
                    {
                        renderer.material = ConstructionLibrary.Instance.previewGreen;
                    }
                    else
                    {
                        child.GetChild(0).GetComponent<Renderer>().material = ConstructionLibrary.Instance.previewGreen;
                    }
                }
            }
            else
            {
                foreach (Transform child in graphics)
                {
                    if(child.TryGetComponent(out renderer))
                    {
                        renderer.material = ConstructionLibrary.Instance.previewRed;
                    }
                    else
                    {
                        child.GetChild(0).GetComponent<Renderer>().material = ConstructionLibrary.Instance.previewRed;
                    }
                }
            }
        }
    }

}