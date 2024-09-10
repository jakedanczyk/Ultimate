using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Xml;
using System;

namespace Urth
{
    public enum ATTACHMENT_METHOD
    {
        SOLID,//continuous material
        PRESS_FIT,
        WEDGED, //tool 
        CRIMP,
        WELD,
        FORGE,
        BRAZE,
        SOLDER,
        GLUE,
        CEMENT,
        MAGNET,
        VACUUM,
        BOLT,
        SCREW,
        NAIL,
        RIVET,
        SNAP
    }
    public enum FUNCTION
    {
        FIXED,
        LINEAR,
        QUADRATIC,
        CUBIC,
        INVERSE,
        SQUARE_ROOT,
        CUBE_ROOT,
        FOURTH_ROOT


    }


    [System.Serializable]
    public class ItemsLibrary : MonoBehaviour
    {
        public static ItemsLibrary Instance { get; private set; }
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


        void Start()
        {
            Load();
        }

        public void Load()
        {
            prefabsDict = new Dictionary<ITEM, ItemModelPrefab>(prefabs.Count);
            foreach (ItemModelPrefab itemModelPrefab in prefabs)
            {
                prefabsDict[itemModelPrefab.type] = itemModelPrefab;
            }
            PopulateTemplates();
        }

        public ItemsManager itemsManager;

        public List<ItemModelPrefab> prefabs;
        public Dictionary<ITEM, ItemModelPrefab> prefabsDict;

        XmlDocument templatesXmlFile;
        public Dictionary<ITEM, UItemTemplate> templatesDict;

        void PopulateTemplates()
        {
            templatesXmlFile = new XmlDocument();
            templatesXmlFile.Load(System.IO.Path.Combine(Application.dataPath, UrthConstants.ITEM_TEMPLATES_XML));
            templatesDict = new Dictionary<ITEM, UItemTemplate>(templatesXmlFile.DocumentElement.ChildNodes.Count);
            foreach (XmlNode templateNode in templatesXmlFile.DocumentElement.ChildNodes)
            {
                XmlElement templateElement = (XmlElement)templateNode;
                UItemTemplate template = new UItemTemplate();
                template.type = (ITEM)System.Enum.Parse(typeof(ITEM), templateNode.Attributes["type"].InnerText);
                template.componentName = templateNode["name"].InnerText;
                template.primaryUse = (USAGE)System.Enum.Parse(typeof(USAGE), templateNode["primaryUse"].InnerText);
                foreach (XmlNode componentNode in templateElement["usages"].ChildNodes)
                {
                    USAGE usage = (USAGE)System.Enum.Parse(typeof(USAGE), componentNode.Attributes["type"].InnerText);
                    Debug.Log(usage);
                    if(usage == USAGE.TOOL)
                    {
                        foreach (XmlNode worktaskNode in componentNode.ChildNodes[0].ChildNodes)
                        {
                            Debug.Log(worktaskNode.Attributes["type"].InnerText);
                            WORKTASK task = (WORKTASK)System.Enum.Parse(typeof(WORKTASK), worktaskNode.Attributes["type"].InnerText);
                            float score = float.Parse(worktaskNode["score"].InnerText);
                            template.worktaskScores[task] = score;
                        }
                    }
                    else if(usage == USAGE.WEAPON)
                    {

                    }
                }
                //if (templateElement["usages"].InnerText.Length > 0)
                //{
                //    foreach (var altUsageStr in templateElement["altUsages"].InnerText.Split(','))
                //    {
                //        USAGE altUsage = (USAGE)System.Enum.Parse(typeof(USAGE), altUsageStr);
                //        template.altUsages.Add(altUsage);
                //    }
                //}
                if (templateElement["tags"].InnerText.Length > 0)
                {
                    foreach (var tagStr in templateElement["tags"].InnerText.Split(','))
                    {
                        ITEM_TAG tag = (ITEM_TAG)System.Enum.Parse(typeof(ITEM_TAG), tagStr);
                        template.tags.Add(tag);
                    }
                }
                //foreach (XmlNode componentNode in templateElement["components"].ChildNodes)
                //{
                //    ITEM typeComp = (ITEM)System.Enum.Parse(typeof(ITEM), componentNode.Attributes["type"].InnerText);
                //    string nameComp = componentNode["name"].InnerText;
                //    float[] lwh = Array.ConvertAll(componentNode["lwh"].InnerText.Split(','), float.Parse);
                //    float3 lwh3 = new float3(lwh[0], lwh[1], lwh[2]);
                //    UItemTemplateComponent piece = new UItemTemplateComponent(nameComp, typeComp, lwh3);
                //    template.componentsDict[nameComp] = piece;
                //}
                //props

                //props assembled
                //foreach (XmlNode prop in templateElement["propsAssembled"].ChildNodes)
                //{
                //    foreach (var tagStr in templateElement["tags"].InnerText.Split(','))
                //    {
                //        ITEM_TAG tag = (ITEM_TAG)System.Enum.Parse(typeof(ITEM_TAG), tagStr);
                //        template.tags.Add(tag);
                //    }
                //}

                templatesDict[template.type] = template;
            }
        }

    }
}