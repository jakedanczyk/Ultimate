using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Urth
{

    public class MapPanelControl : UIPanelControl
    {
        public VisualElement mapPanel;
        public VisualElement content;

        public static MapPanelControl Instance { get; private set; }
        public override void Awake()
        {
            base.Awake();
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            mapPanel = document.rootVisualElement.Query("MapPanel").First();
            content = mapPanel.Query("content");
        }

    }
}