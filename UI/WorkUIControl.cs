using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class WorkUIControl : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
    }
}