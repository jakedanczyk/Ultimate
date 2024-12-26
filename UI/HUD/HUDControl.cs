using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using MalbersAnimations.Utilities;


namespace Urth
{
    public class HUDControl : MonoBehaviour
    {
        public UIDocument hudDocument;
        public WorkUIControl workUIControl;
        public GameManager gameManager;
        public CreaturesLibrary creatureLibrary;
        public GameObject playerCharacter;
        public PhysicsBasedCharacterController.CharacterManager characterManager;
        public PlayerCreatureManager playerCreatureManager;
        public GameObject targetIndicator;
        public GameObject flyCamera;

        public VisualElement outerElement;
        public VisualElement target;
        public VisualElement labelElement;
        public Label lookAtLabel;
        public Aim aim;
        public RaycastHit aimHit;
        public UInputReader inputReader;

        public VisualElement constructionInfoTopElement;
        public VisualElement constructionInfoGroupElement;

        public static HUDControl Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            hudDocument = this.GetComponent<UIDocument>();

            outerElement = hudDocument.rootVisualElement.Query("HUD").First();
            labelElement = outerElement.Query("target").First();
            lookAtLabel = (Label)labelElement.Query("label").First();

            constructionInfoTopElement = hudDocument.rootVisualElement.Query("constructionInfo").First();
            constructionInfoGroupElement = constructionInfoTopElement.ElementAt(0).ElementAt(0);
        }

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.Instance;
            creatureLibrary = CreaturesLibrary.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if(aim.AimHit.transform != null)
            {
                lookAtLabel.text = aim.AimHit.transform.name;
            }
            else
            {
                lookAtLabel.text = "";
            }

            if(targetIndicator != null)
            {
                if (Physics.Raycast(aim.AimOrigin.position, aim.AimDirection, out aimHit, 40))
                {
                    Debug.DrawLine(aim.AimOrigin.position, aimHit.point, Color.green);
                    if (aimHit.transform != this.transform)
                        UpdateWorksite(aimHit);
                }
                //switch (workUIControl.currentWorksiteIndicatorType)
                //{
                //    case WORKSITE_TYPE.TERRAIN:

                //}
            }
        }

        public void SetConstructionInfo(string buildType, string buildScale, string offset)
        {
            ((Label)constructionInfoGroupElement.ElementAt(0)).text = buildType;
            ((Label)constructionInfoGroupElement.ElementAt(1)).text = buildScale;
            ((Label)constructionInfoGroupElement.ElementAt(2)).text = offset;
        }
        public void SetConstructionType(string buildType)
        {
            ((Label)constructionInfoGroupElement.ElementAt(0)).text = buildType;
        }
        public void SetConstructionOffset(string offset)
        {
            ((Label)constructionInfoGroupElement.ElementAt(2)).text = offset;
        }

        public void EnableWorkHUD()
        {
            targetIndicator = workUIControl.terrainWorksiteIndicator.gameObject;
        }

        public void DisableWorkHUD()
        {
            targetIndicator = null;
        }

        public float heightOffset;
        public Transform aimOrigin;
        private Vector3 currentPos;
        private Vector3 currentRot;
        public Transform previewTransform;
        bool snapToGrid = true;

        //public bool isBuilding;
        //public bool snapToGrid;
        //public GameObject previewObject;
        //public USTATIC selectedType;
        //public ConstructionPreview currentPreview;
        //public float placementAngle;
        //public STATIC_SIZE placementSizeEnum;
        //public float placementScale = 1f;

        public float offset = 1.0f;
        public float gridSize = 1.0f;
        public void UpdateWorksite(RaycastHit hit2)
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
            targetIndicator.transform.position = currentPos;
            targetIndicator.transform.localEulerAngles = new Vector3(0, 0, 0); //currentRot;
        }

    }
}