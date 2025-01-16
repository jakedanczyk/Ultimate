using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class WorkUIControl : MonoBehaviour
    {
        List<WORKTASK> availableTasks = new List<WORKTASK>() { WORKTASK.MINE, WORKTASK.SCAN_TERRAIN };
        Dictionary<WORKTASK, int> taskOrder;
        Dictionary<int, WORKTASK> orderOfTasks;
        public PlayerCreatureManager playerCreatureManager;
        public Transform indicatorTransform;
        public WORKTASK currentWorktask;
        public WORKSITE_TYPE currentWorksiteIndicatorType;
        public GameObject terrainWorksiteIndicator;
        public GameObject plantWorksiteIndicator;

        public TaskMenuControl taskMenuControl;
        public VisualElement taskSelectionPanel;

        public WorkInfoPanelControl infoPanelControl;
        public VisualElement infoPanel;
        public Label taskLabel;

        public static WorkUIControl Instance { get; private set; }
        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            taskOrder = new Dictionary<WORKTASK, int>(availableTasks.Count);
            orderOfTasks = new Dictionary<int, WORKTASK>(availableTasks.Count);
            int c = 0;
            foreach(WORKTASK wt in availableTasks)
            {
                taskOrder.Add(wt, c);
                orderOfTasks.Add(c, wt);
                c++;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isWorking)
            {
                //if (playerCreatureManager.worksiteType != currentWorksiteIndicatorType)
                //{
                //    ActivateCurrentWorksiteIndicator();
                //}
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

        public void Initialize()
        {
            workInterface = doc.rootVisualElement.Query(UrthConstants.WORK_INTERFACE).First();

            taskSelectionPanel = workInterface.Query("TaskMenu").First();
            taskMenuControl.Link(taskSelectionPanel);
            taskMenuControl.Populate(availableTasks);

            infoPanel = workInterface.Query("InfoPanel").First();
            infoPanelControl.Link(infoPanel);
            //taskLabel = infoPanel.ElementAt(0).Query("content").First().Query("taskLabel").First() as Label;

            uiBuilt = true;
        }

        public void Enable()
        {
            if (!uiBuilt)
            {
                Initialize();
            }
            workInterface.BringToFront();
            ActivateCurrentWorksiteIndicator();
        }

        public void DisableMenus()
        {
            workInterface.style.display = DisplayStyle.None;
        }
        public void EnableMenus()
        {
            if (!uiBuilt)
            {
                Initialize();
            }
            workInterface.style.display = DisplayStyle.Flex;
        }
        

        void DeactivateTerrainIndicator()
        {
            terrainWorksiteIndicator.SetActive(false);
        }

        void ActivateTerrainIndicator()
        {
            indicatorTransform = terrainWorksiteIndicator.transform;
            terrainWorksiteIndicator.SetActive(true);
        }

        void DeactivateBushIndicator()
        {
            plantWorksiteIndicator.SetActive(false);
        }

        void ActivateBushIndicator()
        {
            indicatorTransform = plantWorksiteIndicator.transform;
            plantWorksiteIndicator.SetActive(true);
        }

        void ActivateCurrentWorksiteIndicator()
        {
            indicatorTransform.gameObject.SetActive(false);
            switch (currentWorksiteIndicatorType)
            {
                case WORKSITE_TYPE.TERRAIN:
                    ActivateTerrainIndicator();
                    break;
                case WORKSITE_TYPE.BUSH:
                    ActivateBushIndicator();
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
                    SetPreview(aimHit);
            }
        }

        public float heightOffset;
        public void SetPreview(RaycastHit hit2)
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

        public void NextWorktask()
        {
            int nc = (taskOrder[currentWorktask] + 1)% taskOrder.Count;
            SetWorktask(availableTasks[nc]);
        }
        public void PrevWorktask()
        {
            int nc = (taskOrder[currentWorktask] - 1) % taskOrder.Count;
            SetWorktask(availableTasks[nc]);
        }

        public void SetWorktask(WORKTASK newTask)
        {
            if (!taskOrder.ContainsKey(newTask))
            {
                Debug.Log("tried to set an unavailable worktask");
            }
            currentWorktask = newTask;
            playerCreatureManager.worktask = newTask;
            switch (newTask)
            {
                case WORKTASK.MINE:
                    currentWorksiteIndicatorType = WORKSITE_TYPE.TERRAIN;
                    break;
                case WORKTASK.DIG:
                    currentWorksiteIndicatorType = WORKSITE_TYPE.TERRAIN;
                    break;
            }
            ActivateCurrentWorksiteIndicator();
        }


        public new void OnItemClick(ClickEvent evt, int idx)
        {
            // Only perform this action at the target, not in a parent
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            // Assign a random new color
            var targetBox = evt.target as VisualElement;
            VisualElement parent = targetBox.parent;
            var currColor = parent.style.backgroundColor.value;
            parent.style.backgroundColor = Color.green;// new Color(currColor.r,currColor.g,currColor.b,1f);

            Debug.Log(availableTasks[idx]);
        }

        public void SetInfoPanelTerrain(TerrainWorksite tw)
        {
            Debug.Log("SetINfoPanelTerrain");
            infoPanelControl.PopulateTerrain(tw);
        }
    }
}