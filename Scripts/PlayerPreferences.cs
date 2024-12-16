using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class UKeyBind
    {
        public int key;
        public UItemData item;
        //public UAction action;

        public UKeyBind(int ikey, UItemData iitem)
        {
            key = ikey;
            item = iitem;
        }
    }

    public class PlayerPreferences : MonoBehaviour
    {
        public bool isRightPrimary = true;
        public bool equipShieldsLeft = true;
        public HashSet<ITEM> offhandWeapons = new HashSet<ITEM>() {
            ITEM.BUCKLER,
            ITEM.ROUND_SHIELD,
            ITEM.KITE_SHIELD,
            ITEM.HEATER_SHIELD,
            ITEM.TOWER_SHIELD
        };
        public Dictionary<WORKSITE_TYPE, List<WORKTASK>> autoWorkOrders = new Dictionary<WORKSITE_TYPE, List<WORKTASK>>()
        {
            {WORKSITE_TYPE.TERRAIN, new List<WORKTASK>(){WORKTASK.DIG,WORKTASK.MINE} },
            {WORKSITE_TYPE.TREE, new List<WORKTASK>(){WORKTASK.FELL,WORKTASK.DELIMB} },
            {WORKSITE_TYPE.STUMP, new List<WORKTASK>(){WORKTASK.CLEAR} }
        };

        public List<UMATERIAL> preferredTerrainMaterials = new List<UMATERIAL>();

        public Dictionary<int, UKeyBind> keybinds = new Dictionary<int, UKeyBind>()
        {

        };
    
        public static PlayerPreferences Instance { get; private set; }
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

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetKeybind(int slot, UItemData itemData)
        {
            keybinds[slot] = new UKeyBind(slot, itemData);
        }
    }
}