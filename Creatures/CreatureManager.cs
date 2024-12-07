﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{
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
                Vector3 fromPlayer = this.transform.position - GameManager.Instance.playerCharacter.transform.position;
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
        public CreatureBody body;
        public CreatureMind mind;
        public NPCController npcControl;
        //public PlayerController playerControl;
        public UrthAnimController animController;
        public UPlayerBuilder terrainBuilder;

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
            switch (newMode)
            {
                case CREATURE_MODE.COMBAT:
                    EnterCombatMode();
                    break;
                case CREATURE_MODE.ACTIVE:
                    EnterActiveMode();
                    break;
            }
        }

        public void EnterActiveMode()
        {
            mode = CREATURE_MODE.ACTIVE;
            if (combatMode)
            {
                combatMode = false;
                LowerWeapons();
            }
            animController.SetStance(STANCE.ACTIVE);
        }

        public void EnterCombatMode()
        {
            combatMode = true;
            mode = CREATURE_MODE.COMBAT;
            RaiseWeapons();
            animController.SetStance(STANCE.COMBAT);
        }

        public void LowerWeapons()
        {

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
            SetIdleLeft();
        }
        public void StopWorkingRight()
        {
            working = false;
            SetIdleRight();
        }
        public void TryDoAimedWorkRight()
        {
            TryDoAimedWork(body.creatureInventory.rightItem.item);
        }
        public void TryDoAimedWorkLeft()
        {
            TryDoAimedWork(body.creatureInventory.leftItem.item);
        }
        public void TryDoAimedWork(UItem item)
        {
            Debug.Log(mWeaponManager.animal.Aimer.AimHit.transform.name);
            switch (mWeaponManager.animal.Aimer.AimHit.transform.tag)
            {
                case ("Terrain"):
                    DoTerrainWork(item);
                    break;
            }
        }
        public void DoTerrainWork(UItem item)
        {
            //If targeted terrain worksite doesn't exist yet, create it
            //Determine the type of work being done
            //Do the work
            TerrainWorksite terrainWorksite = WorksitesManager.Instance.GetTerrainWorksite(mWeaponManager.animal.Aimer.AimHit.point);

            WORKTASK task = item.data.template.preferredTasks[WORKSITE.TERRAIN];
            switch (task)
            {
                case (WORKTASK.MINE):
                    UrthInteractions.MineTerrainBlock(this, item, terrainWorksite);
                    break;
                case (WORKTASK.DIG):
                    UrthInteractions.DigTerrainBlock(this, item, terrainWorksite);
                    break;
            }
        }

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
    }
}