using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    /*Attached to each player
     * 
     */
    public class ConstructionPlayer : MonoBehaviour
    {
        public MalbersAnimations.Utilities.Aim aim;
        public RaycastHit aimHit;
        public Transform aimOrigin;
        private Vector3 currentPos;
        private Vector3 currentRot;
        public Transform previewTransform;

        public bool isBuilding;
        public bool snapToGrid;
        public GameObject previewObject;
        public USTATIC selectedType;
        public ConstructionPreview currentPreview;
        public float placementAngle;
        public STATIC_SIZE placementSizeEnum;
        public float placementScale = 1f;

        public float offset = 1.0f;
        public float gridSize = 1.0f;

        void Start()
        {
        }

        void Update()
        {
            if (isBuilding)
            {
                if (currentPreview is null)
                {
                    InstantiatePreview();
                }
                currentPreview.isActive = true;
                UpdatePreview();
            }
        }

        public void StartPreview()
        {
            isBuilding = true;
            if (currentPreview is null)
            {
                InstantiatePreview();
            }
            currentPreview.gameObject.SetActive(true);
            //if (currentPreview == null)
            //{
            //    currentPreview = ConstructionManager.Instance.previewDict[selectedType];
            //}
        }
        public void StopPreview()
        {
            if(currentPreview is not null)
            {
                currentPreview.gameObject.SetActive(false);
            }
            isBuilding = false;
        }
        void UpdatePreview()
        {
            //if (Physics.Raycast(aimOrigin.position, aimOrigin.forward, out aimHit, 40, ConstructionManager.Instance.constructionLayers))
            //{
            //    Debug.DrawLine(aimOrigin.position, aimHit.point, Color.green);
            //    if (aimHit.transform != this.transform)
            //        ShowPreview(aimHit);
            //}
            if (Physics.Raycast(aim.AimOrigin.position, aim.AimDirection, out aimHit, 40, ConstructionLibrary.Instance.constructionLayers))
            {
                Debug.DrawLine(aim.AimOrigin.position, aimHit.point, Color.green);
                if (aimHit.transform != this.transform)
                    ShowPreview(aimHit);
            }
        }

        public float adjust;
        public void ShowPreview(RaycastHit hit2)
        {
            currentPos = hit2.point;
            currentPos -= Vector3.one * offset;
            currentPos /= gridSize;
            currentPos = new Vector3(Mathf.Round(currentPos.x), currentPos.y, Mathf.Round(currentPos.z));
            currentPos *= gridSize;
            currentPos += Vector3.one * offset + Vector3.up * adjust;
            previewTransform.position = currentPos;
            previewTransform.localEulerAngles = currentRot;
            previewObject.transform.position = currentPos;
            previewObject.transform.eulerAngles = currentRot;
        }

        public void InputRotate90()
        {
            currentRot += new Vector3(0, 45, 0);
        }
        public void InputPlace()
        {
            if (currentPreview != null && currentPreview.isBuildable)
            {
                ConstructionManager.Instance.CreateConstruction(currentPreview, placementScale, currentPos, currentRot);
            }
        }
        public void ChangePreviewType(USTATIC type)
        {
            selectedType = type;
            HUDControl.Instance.SetConstructionType(selectedType.ToString());
            if(previewObject != null)
            {
                Destroy(previewObject);
            }
            InstantiatePreview();
        }
        public void InstantiatePreview()
        {
            previewObject = Instantiate(ConstructionLibrary.Instance.previewDict[selectedType].gameObject, Vector3.zero, Quaternion.identity);
            currentPreview = previewObject.GetComponent<ConstructionPreview>();
        }
        public void AdjustHeight(float adj)
        {
            adjust += adj;
            string display = (adjust > 0 ? "+" : "-") + (Mathf.Abs(adjust).ToString()) + "m";
            HUDControl.Instance.SetConstructionOffset(display);
        }
        //public void Build()
        //{
        //StaticsLibrary.Instance.prefabsDict[selectedPreview.staticType].finalPrefab
        //    PreviewObject PO = currentPreview.GetComponent<PreviewObject>();
        //    if (PO.isBuildable)
        //    {
        //        //if supplies available
        //        for (int i = 0; i < PO.supplyListItems.Count; i++)
        //        {
        //            var count = supplyInventory.inventoryContents.Where(item => item.itemName == PO.supplyListItems[i].itemName).Count();
        //            print(count);
        //            if (count < PO.supplyListNumbers[i]) { print("insufficient supplies"); return; }
        //        }
        //        //build item
        //        var newobject = Instantiate(currentObject.prefab, currentPos, Quaternion.Euler(currentRot));
        //        BuildingObject newBuildingObject = newobject.GetComponent<BuildingObject>();

        //        //move supplies into building object
        //        for (int i = 0; i < PO.supplyListItems.Count; i++)
        //        {
        //            for (int j = 0; j < PO.supplyListNumbers[i]; j++)
        //            {
        //                Item consumedItem = (supplyInventory.inventoryContents.Find(delegate (Item searchItem) { return searchItem.itemName == PO.supplyListItems[i].itemName; }));
        //                supplyInventory.RemoveItem(consumedItem);
        //                newBuildingObject.mats.Add(consumedItem);
        //                consumedItem.gameObject.SetActive(false);
        //            }
        //        }
        //        print("buildable built");
        //    }
        //}
        public void SetSize(STATIC_SIZE newsize)
        {
            placementSizeEnum = newsize;
        }

        public void PlaceConstruction()
        {

        }
    }

}