using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class InventoriesManager : MonoBehaviour
    {
        private int nextId = 0;
        public Dictionary<int, UInventory> inventories;

        public static InventoriesManager Instance { get; private set; }
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
            Initialize();
        }
        void Start()
        {
        }

        void Update()
        {

        }
        public void Initialize()
        {
            inventories = new Dictionary<int, UInventory>();
            //LoadSaveData();
        }
        public void LoadSaveData(string filepath)
        {
            List<string> fileData = new List<string>();//= ReadJson(filepath);//TODO implement
            List<(int,string)> datas = new List<(int, string)>(fileData.Count); 
            foreach((int id, string data) in datas)
            {
                LoadInventory(id, data);
            }
        }

        public int GetNextId()
        {
            return nextId++;//increments after return
        }

        public (int, UInventory) NextInventory()
        {
            UInventory inv = new UInventory();
            int id = GetNextId();
            inventories[id] = inv;
            return (id, inv);
        }
        void LoadInventory(int id, string data)
        {
            UInventory inv = new UInventory();
            inventories[id] = inv;
            //TODO split data and foreach to read items
        }
    }
}