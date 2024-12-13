using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    public class PauseMenuControl : MonoBehaviour
    {

        public UIDocument pauseMenuDocument;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate()
        {
            pauseMenuDocument.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            pauseMenuDocument.gameObject.SetActive(false);
        }

        public void SetVisible()
        {
            pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void SetHidden()
        {
            pauseMenuDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }

}