using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations.Utilities;
using UltimateTerrains;

namespace Urth
{

    public class UrthControl : MonoBehaviour
    {
        public PlayerCreatureManager playerCreatureManager;
        public UInputReader inputReader;
        bool inputReset = false;
        public Aim aim;
        public GameObject hudCube;

        private void Awake()
        {
            playerCreatureManager = this.transform.parent.GetComponent<PlayerCreatureManager>();
            inputReader = this.transform.parent.GetComponent<UInputReader>();
            aim = this.transform.parent.GetComponent<Aim>();
        }

        private void Start()
        {
        }

        RaycastHit scan;
        void Update()
        {
            Vector3 toHit = aim.AimHit.point - aim.AimOrigin.position;
            if(toHit.magnitude < playerCreatureManager.body.stats.GetToolReachDistance())
            {
                hudCube.SetActive(true);
                Vector3 pos = aim.AimHit.point;
                pos = new Vector3(((int)((pos.x)*4)) / 4f, ((int)((pos.y) * 4)) / 4f, ((int)((pos.z) * 4) / 4f));
                hudCube.transform.position = pos;
            }
            else
            {
                hudCube.SetActive(false);
            }
            
            //if (inputReader.Interact)
            //{
            //    if (!inputReset && aim.AimHit.transform != null && aim.AimHit.distance < 16f * playerCreatureManager.body.stats.GetGrabReachDistance())
            //    {
            //        Debug.Log(aim.AimHit.transform.tag);
            //        switch (aim.AimHit.transform.tag)
            //        {
            //            case UrthConstants.ITEM_TAG:
            //                TryPickupItem(aim.AimHit.transform.GetComponent<UItem>());
            //                break;
            //            case UrthConstants.WORKSITE_TAG:
            //                TerrainWorksiteHUD vw = aim.AimHit.transform.GetComponent<TerrainWorksiteHUD>();
            //                foreach (Voxel vox in vw.voxelValues)
            //                {
            //                    Debug.Log(vox);
            //                }
            //                break;
            //            case UrthConstants.CONSTRUCTION_TAG:
            //                GameUIControl.Instance.OpenConstructionMenu();
            //                break;
            //        }
            //    }
            //}
            //else
            //{
            //    inputReset = false;
            //}
        }

        public void ChangeGameMode()
        {

        }


        public void Interact()
        {
            Debug.Log("interact");
            if (aim.AimHit.transform != null && aim.AimHit.distance < 16f * playerCreatureManager.body.stats.GetGrabReachDistance())
            {
                Debug.Log(aim.AimHit.transform.tag);
                switch (aim.AimHit.transform.tag)
                {
                    case UrthConstants.ITEM_TAG:
                        TryPickupItem(aim.AimHit.transform.GetComponent<UItem>());
                        break;
                    case UrthConstants.WORKSITE_TAG:
                        TerrainWorksiteHUD vw = aim.AimHit.transform.GetComponent<TerrainWorksiteHUD>();
                        foreach (Voxel vox in vw.voxelValues)
                        {
                            Debug.Log(vox);
                        }
                        break;
                    case UrthConstants.CONSTRUCTION_TAG:
                        StaticPrefab staticPrefab = aim.AimHit.transform.GetComponentInParent<StaticPrefab>();
                        Debug.Log($"Interact Construction {staticPrefab}");
                        GameUIControl.Instance.OpenConstructionMenu(staticPrefab);
                        break;
                }
            }
        }

        public void TryPickupItem(UItem item)
        {
            UrthResponse response = playerCreatureManager.body.creatureInventory.TryPickupItem(item.data);
            if(!response.success)
            {//TODO log message for failed pickups
                inputReset = true;
                MessageLogControl.Instance.NewMessage(response.msg);
            }
        }

        public void LeftDown()
        {
            if (playerCreatureManager.combatMode && !playerCreatureManager.working)
            {
                //start charging left hand
                //playerCreatureManager.offense
            }
            else
            {
                playerCreatureManager.StartWorkingLeft();
            }
        }

        public void RightDown()
        {
            Debug.Log("RightDOwn");
            if (playerCreatureManager.combatMode && !playerCreatureManager.working)
            {
                //start charging right hand
                //playerCreatureManager.offense
            }
            else
            {
                playerCreatureManager.StartWorkingRight();
            }
        }

        public void LeftUp()
        {
            if (playerCreatureManager.combatMode && !playerCreatureManager.working)
            {
                //playerCreatureManager.Lef
            }
            else
            {
                playerCreatureManager.StopWorkingLeft();
            }
        }
        public void RightUp()
        {
            if (playerCreatureManager.combatMode && !playerCreatureManager.working)
            {
                //playerCreatureManager.Lef
            }
            else
            {
                playerCreatureManager.StopWorkingRight();
            }
        }

        public void SetCharacterStance(string newStanceString)
        {
            CREATURE_MODE newStance = (CREATURE_MODE)System.Enum.Parse(typeof(CREATURE_MODE), newStanceString);
            playerCreatureManager.ChangeStance(newStance);
        }

        public UPlayerBuilder uPlayerBuilder;
        public void ScanVoxel()
        {
            uPlayerBuilder.ScanVoxel();
        }
        public void DigVoxel()
        {
            uPlayerBuilder.Dig();
        }

        public void Wheel(Vector2 input)
        {
            Debug.Log("WHeel");
            Debug.Log(input);
        }

    }

}