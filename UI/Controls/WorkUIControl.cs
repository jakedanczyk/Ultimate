using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class WorkUIControl : MonoBehaviour
    {
        public PlayerCreatureManager playerCreatureManager;
        public Transform indicatorTransform;
        public WORKSITE_TYPE currentWorksiteIndicatorType;
        public GameObject terrainWorksiteIndicator;
        public GameObject plantWorksiteIndicator;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (isWorking)
            {
                if (playerCreatureManager.currentWorksiteType != currentWorksiteIndicatorType)
                {
                    ActivateCurrentWorksiteIndicator();
                }
                UpdatePreview();
            }
        }

        public UIDocument doc;
        public VisualElement workInterface;
        bool uiBuilt = false;

        public ListView listView;

        public void Disable()
        {
            workInterface.style.display = DisplayStyle.None;
        }

        public void Enable()
        {
            if (!uiBuilt)
            {
                Initialize();
            }
            else
            {
                workInterface.style.display = DisplayStyle.Flex;
                ActivateCurrentWorksiteIndicator();
            }
        }

        public void Initialize()
        {
            workInterface = doc.rootVisualElement.Query(UrthConstants.CONSTRUCTION_PLANNING_INTERFACE).First();
            workInterface.style.display = DisplayStyle.Flex;

            uiBuilt = true;
        }

        void DeactivateTerrainIndicator()
        {
            terrainWorksiteIndicator.SetActive(false);
        }

        void ActivateTerrainIndicator()
        {
            terrainWorksiteIndicator.SetActive(true);
        }

        void DeactivateBushIndicator()
        {
            plantWorksiteIndicator.SetActive(false);
        }

        void ActivateBushIndicator()
        {
            plantWorksiteIndicator.SetActive(true);
        }

        void ActivateCurrentWorksiteIndicator()
        {
            switch (currentWorksiteIndicatorType)
            {
                case WORKSITE_TYPE.TERRAIN:
                    ActivateTerrainIndicator();
                    DeactivateBushIndicator();
                    break;
                case WORKSITE_TYPE.BUSH:
                    ActivateBushIndicator();
                    DeactivateTerrainIndicator();
                    break;
            }
        }


        public MalbersAnimations.Utilities.Aim aim;
        public RaycastHit aimHit;
        public Transform aimOrigin;
        private Vector3 currentPos;
        private Vector3 currentRot;
        public Transform previewTransform;

        public bool isWorking;
        public bool snapToGrid;
        public GameObject previewObject;
        public float placementAngle;
        public STATIC_SIZE placementSizeEnum;
        public float placementScale = 1f;

        public float offset = 1.0f;
        public float gridSize = 1.0f;

      
        void UpdatePreview()
        {
            //if (Physics.Raycast(aimOrigin.position, aimOrigin.forward, out aimHit, 40, ConstructionManager.Instance.constructionLayers))
            //{
            //    Debug.DrawLine(aimOrigin.position, aimHit.point, Color.green);
            //    if (aimHit.transform != this.transform)
            //        ShowPreview(aimHit);
            //}
            if (Physics.Raycast(aim.AimOrigin.position, aim.AimDirection, out aimHit, 40))
            {
                Debug.DrawLine(aim.AimOrigin.position, aimHit.point, Color.green);
                if (aimHit.transform != this.transform)
                    ShowPreview(aimHit);
            }
        }

        public float heightOffset;
        public void ShowPreview(RaycastHit hit2)
        {
            currentPos = hit2.point;
            if (snapToGrid)
            {
                currentPos -= Vector3.one * offset;
                currentPos /= gridSize;
                currentPos = new Vector3(Mathf.Round(currentPos.x), currentPos.y, Mathf.Round(currentPos.z));
                currentPos *= gridSize;
                currentPos += Vector3.one * offset;
            }
            currentPos += Vector3.up * heightOffset;
            indicatorTransform.position = currentPos;
            previewTransform.localEulerAngles = new Vector3(0, 0, 0); //currentRot;
            previewObject.transform.position = currentPos;
            previewObject.transform.eulerAngles = new Vector3(0, 0, 0); //currentRot;
        }


        public void AdjustHeight(float adj)
        {
            float curr = ConstructionSettingsPanelControl.Instance.AdjustHeightOffset(adj);
            heightOffset = curr;
            string display = (curr > 0 ? "+" : "-") + (Mathf.Abs(curr).ToString()) + "m";
            HUDControl.Instance.SetConstructionOffset(display);
        }
       
    }
}