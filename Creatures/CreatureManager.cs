﻿using System.Collections;
using System.Collections.Generic;
using UltimateTerrains;
using UnityEngine;


namespace Urth
{
    public enum WIELD_SLOT
    {
        LEFT,
        RIGHT,
        LEFT_HAND,
        RIGHT_HAND,
        LEFT_ITEM,
        RIGHT_ITEM,
        TWO_HAND,
        TWO_HAND_ITEM
    }
    public enum CREATURE_MODE
    {
        ACTIVE,
        COMBAT,
        CONSTRUCTION,
        CRAFTING,
        IDLE,
        STEALTH,
        UNCONSCIOUS,
        WORK,
    }

    public class CreatureManager : MonoBehaviour
    {
        public virtual void Awake()
        {
            offense = this.GetComponent<CreatureOffenseController>();
            offense.creatureManager = this;
            defense = this.GetComponent<CreatureDefenseController>();
            defense.creatureManager = this;

            mWeaponManager = this.GetComponent<MWeaponManagerUrth>();
        }

        void Start()
        {

        }


        int framecount = 0;
        void Update()
        {
            framecount++;
            if (framecount == 100)
            {
                Vector3 fromPlayer = this.transform.position - GameManager.Instance.playerCharacterObject.transform.position;
                if (fromPlayer.sqrMagnitude > CreaturesManager.SQ_DESPAWN_RADIUS)
                {
                    CreaturesManager.Instance.shouldDespawnList.Add(id);
                }
            }
        }

        public int id;
        public bool isSpawned = false;
        public bool combatMode = false;
        public bool stealthMode = false;
        public bool sprintMode = false;
        public CREATURE_MODE mode = CREATURE_MODE.IDLE;
        public CREATURE_MODE desiredMode = CREATURE_MODE.IDLE;
        public CreatureBody body;
        public CreatureMind mind;
        public NPCController npcControl;
        //public PlayerController playerControl;
        public UrthAnimController animController;
        public UPlayerBuilder terrainBuilder;
        public WORKTASK worktask;
        public WORKSITE_TYPE currentWorksiteType;

        public CreatureOffenseController offense;
        public CreatureDefenseController defense;
        public bool isPlayer;

        public MWeaponManagerUrth mWeaponManager;

        public void PrepareForSpawn(CreatureBody ibody)
        {
            isSpawned = true;
            body = ibody;
            body.manager = this;
            mind = body.mind;
            npcControl.AddNav();
        }

        public void ChangeMode(CREATURE_MODE newMode)
        {
            if(mode == newMode)
            {
                return;
            }
            switch (mode)
            {
                case CREATURE_MODE.COMBAT:
                    ExitCombatMode();
                    break;
                case CREATURE_MODE.ACTIVE:
                    ExitActiveMode();
                    break;
                case CREATURE_MODE.WORK:
                    ExitWorkMode();
                    break;
            }
            switch (newMode)
            {
                case CREATURE_MODE.COMBAT:
                    EnterCombatMode();
                    break;
                case CREATURE_MODE.ACTIVE:
                    EnterActiveMode();
                    break;
                case CREATURE_MODE.WORK:
                    EnterWorkMode();
                    break;
            }
        }

        public void EnterActiveMode()
        {
            mode = CREATURE_MODE.ACTIVE;
            animController.SetStance(STANCE.ACTIVE);
        }
        public void ExitActiveMode()
        {
        }
        public void EnterCombatMode()
        {
            combatMode = true;
            mode = CREATURE_MODE.COMBAT;
            RaiseWeapons();
            animController.SetStance(STANCE.COMBAT);
        }
        public void ExitCombatMode()
        {
            combatMode = false;
            LowerWeapons();
        }
        public void EnterWorkMode()
        {
            mode = CREATURE_MODE.WORK;
            animController.SetStance(STANCE.ACTIVE);
        }
        public void ExitWorkMode()
        {

        }
        public void LowerWeapons()
        {
            Debug.Log("TODO lowerWeapons");
        }

        public void RaiseWeapons()
        {

            CreatureInventory inventory = body.creatureInventory;
            //if(inventory.rightItem != null)
            //{
            //    GameObject rightWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.rightItem.type].weaponPrefab);
            //    //mWeaponManager.RightEquip_Fast(rightWeaponInstance);
            //    //mWeaponManager.RightHolster_SetActive(rightWeaponInstance);
            //    //mWeaponManager.RightHolster_SetWeapon(rightWeaponInstance);
            //    mWeaponManager.Equip_External(rightWeaponInstance);
            //}
            if (inventory.leftItem != null)
            {
                GameObject leftWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.leftItem.type].weaponPrefab);
                //mWeaponManager.LeftEquip_Fast(leftWeaponInstance);
                //mWeaponManager.LeftHolster_SetWeapon(leftWeaponInstance);
                mWeaponManager.LeftEquip_External(leftWeaponInstance);
            }
            //mWeaponManager.Raise_RightWeapon();
            //mWeaponManager.Raise_LeftWeapon();
        }

        public void RaiseRightWeapon()
        {
            Debug.Log("RaiseRight");
            CreatureInventory inventory = body.creatureInventory;
            if (inventory.rightItem != null)
            {
                if (mWeaponManager.RightWeapon != null)
                {
                    mWeaponManager.Store_RightWeapon();
                }
                else
                {
                    GameObject rightWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.rightItem.type].weaponPrefab);
                    mWeaponManager.RightEquip_External(rightWeaponInstance);
                }
            }
        }
        public void RaiseLeftWeapon()
        {
            Debug.Log("RaiseLeft");
            CreatureInventory inventory = body.creatureInventory;
            if (inventory.leftItem != null)
            {
                if (mWeaponManager.LeftWeapon != null)
                {
                    mWeaponManager.Store_LeftWeapon();
                }
                else
                {
                    GameObject leftWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.leftItem.type].weaponPrefab);
                    mWeaponManager.LeftEquip_External(leftWeaponInstance);
                }
            }
        }

        public void DrawWeapons()
        {

            CreatureInventory inventory = body.creatureInventory;
            //if(inventory.rightItem != null)
            //{
            //    GameObject rightWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.rightItem.type].weaponPrefab);
            //    //mWeaponManager.RightEquip_Fast(rightWeaponInstance);
            //    //mWeaponManager.RightHolster_SetActive(rightWeaponInstance);
            //    //mWeaponManager.RightHolster_SetWeapon(rightWeaponInstance);
            //    mWeaponManager.Equip_External(rightWeaponInstance);
            //}
            if (inventory.leftItem != null)
            {
                GameObject leftWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.leftItem.type].weaponPrefab);
                //mWeaponManager.LeftEquip_Fast(leftWeaponInstance);
                //mWeaponManager.LeftHolster_SetWeapon(leftWeaponInstance);
                mWeaponManager.LeftEquip_External(leftWeaponInstance);
            }
            //mWeaponManager.Draw_RightWeapon();
            //mWeaponManager.Draw_LeftWeapon();
        }

        public void DrawRightWeapon()
        {
            Debug.Log("DrawRight");
            CreatureInventory inventory = body.creatureInventory;
            if (inventory.rightItem != null)
            {
                if(mWeaponManager.RightWeapon != null)
                {
                    mWeaponManager.Store_RightWeapon();
                }
                else
                {
                    GameObject rightWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.rightItem.type].weaponPrefab);
                    mWeaponManager.RightEquip_External(rightWeaponInstance);
                }
            }
        }
        public void DrawLeftWeapon()
        {
            Debug.Log("DrawLeft");
            CreatureInventory inventory = body.creatureInventory;
            if (inventory.leftItem != null)
            {
                if (mWeaponManager.LeftWeapon != null)
                {
                    mWeaponManager.Store_LeftWeapon();
                }
                else
                {
                    GameObject leftWeaponInstance = Instantiate(ItemsLibrary.Instance.prefabsDict[inventory.leftItem.type].weaponPrefab);
                    mWeaponManager.LeftEquip_External(leftWeaponInstance);
                }
            }
        }

        public bool working;
        public bool aimedWorking;
        public double workStartTime;
        public void StartWorkingLeft()
        {
            if (working)
            {
                return;
            }
            aimedWorking = true;
            WorkSwingLeft();
        }
        public void WorkSwingLeft()
        {
            workStartTime = UrthTime.Instance.totalGameSeconds;
            working = true;
            int idx = body.creatureInventory.GetLeftToolIdx();
            animController.StartLeftWorkSwing(idx);
        }
        public void StartWorkingRight()
        {
            if (working)
            {
                return;
            }
            aimedWorking = true;
            WorkSwingRight();
        }
        public void WorkSwingRight()
        {
            workStartTime = UrthTime.Instance.totalGameSeconds;
            working = true;
            int idx = body.creatureInventory.GetRightToolIdx();
            animController.StartRightWorkSwing(idx);
        }
        public void StopWorkingLeft()
        {
            working = false;
            aimedWorking = false;
            SetIdleLeft();
        }
        public void StopWorkingRight()
        {
            working = false;
            aimedWorking = false;
            SetIdleRight();
        }
        //public void TryDoAimedWorkRight()
        //{
        //    if(body.creatureInventory.rightItem != null)
        //    {
        //        TryDoAimedWork(body.creatureInventory.rightItem.item);
        //    }
        //    else
        //    {
        //        TryDoAimedWork(null);
        //    }
        //}
        //public void TryDoAimedWorkLeft()
        //{
        //    if (body.creatureInventory.leftItem != null)
        //    {
        //        TryDoAimedWork(body.creatureInventory.leftItem.item);
        //    }
        //    else
        //    {
        //        TryDoAimedWork(null);
        //    }
        //}
        public void TryDoAimedWork(WIELD_SLOT slot)
        {
            Debug.Log("Aimed work: " + mWeaponManager.animal.Aimer.AimHit.transform.name);
            switch (worktask)
            {
                case (WORKTASK.MINE):
                    MineTerrainBlock(slot);
                    break;
                case (WORKTASK.DIG):
                    DigTerrainBlock(slot);
                    break;
                case (WORKTASK.SCAN_TERRAIN):
                    ScanTerrainBlock();
                    break;
            }
        }
        public UltimateTerrain Terrain;
        public void ScanTerrainBlock()
        {
            TerrainWorksite worksite = WorksitesManager.Instance.GetTerrainWorksite(mWeaponManager.animal.Aimer.AimHit.point);
            List<Voxel> currentVoxelVals = TerrainBuilder.Instance.ScanVoxel(worksite.data.pos);
            foreach(Voxel vox in currentVoxelVals)
            {
                string voxelTypeName = Terrain.VoxelTypeSet.SerializableVoxelTypes[currentVoxelVals[0].VoxelTypeIndex].Name;

                Debug.Log(voxelTypeName);
                Debug.Log(vox.Value);
            }
            //WorkUIControl.Instance.SetInfoPanelTerrain(worksite);
            //UrthInteractions.MineTerrainBlock(this, slot, worksite);
        }
        public virtual void MineTerrainBlock(WIELD_SLOT slot)
        {
            TerrainWorksite worksite = WorksitesManager.Instance.GetTerrainWorksite(mWeaponManager.animal.Aimer.AimHit.point);
            UrthInteractions.SimpleMineTerrainBlock(this, slot, worksite);
        }
        public virtual void DigTerrainBlock(WIELD_SLOT slot)
        {
            TerrainWorksite worksite = WorksitesManager.Instance.GetTerrainWorksite(mWeaponManager.animal.Aimer.AimHit.point);
            TerrainBuilder.Instance.Dig(worksite.data.pos);
            //UrthInteractions.DigTerrainBlock(this, slot, worksite);
        }
        public void StartAutoWork()
        {

        }
        public void ProgressAutoWork()
        {

        }
        public UrthResponse TryStartHarvestPlant(PlantTag plantTag)
        {
            PlantData plantData = PlantsManager.Instance.GetPlantData(plantTag);
            //query PlantsManager (or Library? or Plant?) for harvest data

            return new UrthResponse(true);
        }
        //public void DoTerrainWork(WIELD_SLOT slot, TerrainWorksite worksite)
        //{
        //    //If targeted terrain worksite doesn't exist yet, create it
        //    //Determine the type of work being done
        //    //Do the work
        //    worktask
        //    WORKTASK task = item.data.template.preferredTasks[WORKSITE_TYPE.TERRAIN];
        //    switch (task)
        //    {
        //        case (WORKTASK.MINE):
        //            UrthInteractions.MineTerrainBlock(this, item, worksite);
        //            break;
        //        case (WORKTASK.DIG):
        //            UrthInteractions.DigTerrainBlock(this, item, worksite);
        //            break;
        //    }
        //}

        public void SetIdleLeft()
        {
            if (combatMode)
            {
                int idx = body.creatureInventory.GetRightWeaponIdx();
                animController.SetLeftCombatIdle(idx);
            }
            else
            {
                animController.DisableLeft();
            }
        }
        public void SetIdleRight()
        {
            if (combatMode)
            {
                int idx = body.creatureInventory.GetLeftWeaponIdx();
                animController.SetRightCombatIdle(idx);
            }
            else
            {
                animController.DisableRight();
            }
        }

        public void TryDropItem(UItemData itemData)
        {
            Vector3 pos = transform.position + transform.forward + transform.up;
            itemData.pos = new Unity.Mathematics.float3(pos) + GameManager.Instance.gameWorldOffset;
            ItemsManager.Instance.AddSpawnItemQueue(itemData.id);
            body.creatureInventory.RemoveItem(itemData);
        }

        public UItemData GetLeftItem()
        {
            return body.creatureInventory.leftItem;
        }
        public UItemData GetRightItem()
        {
            return body.creatureInventory.rightItem;
        }
    }
}