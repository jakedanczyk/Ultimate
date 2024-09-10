using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    [Serializable]
    public class UrthConstruction
    {
        public string id;
        public string name;
        public USTATIC type;
        public float size;
        public int inventoryId;
        public UInventory inventory;
        public bool isConstruction;
        public string constructionWorksiteId;
        public List<string> supportedConstructions;//TODO need to have non-foundation Constructions listen for new foundations placed below them
        public List<string> supportingConstructions;

        public UrthConstruction(string nid, string nname, USTATIC ntype, float nsize, bool nisCons)
        {
            id = nid;
            name = nname;
            type = ntype;
            size = nsize;
            isConstruction = nisCons;
            (inventoryId, inventory) = InventoriesManager.Instance.NextInventory();
        }
    }

}