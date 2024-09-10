using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{
    public enum CONSTRUCTION_SUPPORT_LAYERS
    {
        GROUND,
        FOUNDATION,
        SUPPORT,
        EXTENSION,
        FURNITURE
    }
    public enum CONSTRUCTION_METHOD
    {
        SIMPLE,
        LOG,
        LUMBER,
        STONE
    }
    public class ConstructionLibrary : MonoBehaviour
    {
        public Material previewRed;
        public Material previewGreen;
        public List<ConstructionPreview> previewList;
        public Dictionary<USTATIC, ConstructionPreview> previewDict;
        public LayerMask constructionLayers;
        public List<ConstructionTemplate> templateList;
        public Dictionary<USTATIC, ConstructionTemplate> templateDict;

        public ConstructionPlayer constructionPlayer;

        public Dictionary<STATIC_SIZE, float> sizes = new Dictionary<STATIC_SIZE, float>
        {
            { STATIC_SIZE.DOUBLE, 2f },
            { STATIC_SIZE.FULL, 1f },
            { STATIC_SIZE.HALF, 0.5f },
        };


        public static ConstructionLibrary Instance { get; private set; }
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
        }
        private void Start()
        {
            PopulatePreviewDict();
        }

        void PopulatePreviewDict()
        {
            previewDict = new Dictionary<USTATIC, ConstructionPreview>(previewList.Count);
            foreach (ConstructionPreview pv in previewList)
            {
                if (System.Enum.TryParse(pv.staticTypeName, out USTATIC type))
                {
                    previewDict[type] = pv;
                    pv.staticType = type;
                }
                else
                {
                    Debug.LogWarning(string.Format("ConstructionPreview name {0} not found in STATIC enum", pv.staticTypeName));
                }
            }
        }


    }
}