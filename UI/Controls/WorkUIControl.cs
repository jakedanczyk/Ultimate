using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class WorkUIControl : MonoBehaviour
    {
        public PlayerCreatureManager playerCreatureManager;
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
            if(playerCreatureManager.currentWorksiteType != currentWorksiteIndicatorType)
            {
                switch (currentWorksiteIndicatorType)
                {
                    case WORKSITE_TYPE.TERRAIN:
                        DeactivateTerrainIndicator();
                        break;
                    case WORKSITE_TYPE.BUSH:
                        DeactivateBushIndicator();
                        break;
                }
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
    }
}