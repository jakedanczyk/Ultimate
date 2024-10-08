using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class ItemModelPrefab : MonoBehaviour
    {
        public string typeS;
        public ITEM type;
        public bool typeSet;

        public void Awake()
        {
            SetType();
        }

        public GameObject componentPrefab;
        public GameObject assembledPrefab;
        public GameObject weaponPrefab;

        public UMA.CharacterSystem.UMAWardrobeRecipe recip;
        public UMA.UMATextRecipe textRecip;

        public void SetType()
        {
            type = (ITEM)System.Enum.Parse(typeof(ITEM), typeS);
            typeSet = true;
        }
    }
}
