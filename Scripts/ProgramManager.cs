using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public enum PROGRAM_MODE
    {
        NULL,//DO NOTHING
        MAIN, //start to main menu
        DEV_CHAR, //start to default char in static prefab arena
        DEV_TERRAIN, //start to default terrainGen then freecam after gen complete
    }

    public class ProgramManager : MonoBehaviour
    {
        public GameManager gameManager;
        public UIManager uiManager;

        public PROGRAM_MODE programMode;

        private void Awake()
        {
            switch (programMode)
            {
                case PROGRAM_MODE.NULL:
                    break;
                case PROGRAM_MODE.MAIN:
                    uiManager.mainMenu.gameObject.SetActive(true);
                    break;
                case PROGRAM_MODE.DEV_CHAR:
                    gameManager.EnableTestArena();
                    break;
                case PROGRAM_MODE.DEV_TERRAIN:
                    break;
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
    }

}