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
        public GameManager gameManager;
        public CreaturesLibrary creatureLibrary;
        public GameObject playerCharacter;
        public PhysicsBasedCharacterController.CharacterManager characterManager;
        public GameObject flyCamera;
        public UIDocument hudDocument;

        public VisualElement outerElement;
        public VisualElement target;
        public VisualElement labelElement;
        public Label lookAtLabel;
        public Aim aim;
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

    }
}