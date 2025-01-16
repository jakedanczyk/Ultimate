using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Urth
{
    public enum ITEM_TAG
    {
        WIELDABLE,
        WEARABLE,
    }
    public enum QUALITY
    {
        RUINED, //completely useless
        AWFUL, // nearly useless, only worth using for critical situation
        SHODDY, // much harder to use
        POOR, // hard to use
        PLAIN, //  gets the job done
        DECENT, // above average
        FINE, //good
        SUPERIOR, //very good
        EXCELLENT, //extremely good
        MASTERFUL, //the best of mortal works
        INCREDIBLE, //is it made by a human?
        MYTHIC, //Surely made by a god...
    }

    public enum USAGE
    {
        NONE,
        DISPLAY, //to be looked at
        FURNITURE, //stationary object with various tasks
        CLOTHING,//to be worn
        TOOL,//handheld tools
        WEAPON,//handheld weapons
        MACHINE,//
        RESOURCE,//food, water, potions, etc.
        STRUCTURE,
    }

    public enum ITEM_PROPERTY
    {
        NONE,
        NAME,
        WEIGHT,
        VOLUME,
        LENGTH,
        WIDTH,
        HEIGHT,
        QUALITY,
        SLOT
    }

    public enum ITEM_CLASS
    {
        GENERIC,
        WEAPON,
        SHIELD,
        TOOL,
    }

    public enum ITEM
    {
        PILE, //generic group of stuff that is clumped. Sand, leaves, bodies, etc.

        GRANULAR,//granular materials
        CHIP,//woodchips, stone chips, etc.
        EARTH, //earthen stuff. 

        ROCK,//a single solid stone, of any size (whatever the inventory can fit)
        NUGGET,//chunk of native metal

        LOG, //A generic log 0.5m in length and 0.25m in diameter. Volume of 0.0245436926
             //For natural woods, weight will vary (by green or dry, and by species) from 5 kg for dry feather-balsa, to more than 25kg for green hardwoods
        STICK,//A generic stick, 0.5m length 2.5cm diameter. Volume 0.0002453 m3, weight 0.1 kg to 0.5 kg
        TWIG,//A generic twig, 0.25m length by 2.5mm, volume 1.227 cm3, weight 0.25g to 1.25g
        WOOD,//a generic piece of wood of any volume
        TREE,//a felled tree. Usually they won't become items, but could be picked up if small enough
        TRUNK, //a whole or section of a tree trunk
        BARK,
        ROOTS,
        BRANCH,
        //TWIG, //small diameter end pieces of branches
        LEAF,
        POLE, //round timber
        BEAM, //squared timber

        HAFT, //generic haft for tools and weapons, can vary in size

        PICK,
        DOUBLE_PICK,
        MATTOCK,//also called adze
        DOUBLE_MATTOCK,
        PICK_MATTOCK,
        CUTTER_MATTOCK,//light-axe mattock combo
        AXE_MATTOCK,//full-axe mattock combo

        SPADE,//Digging shovel
        SHOVEL,//Scooping shovel
        BROOM,

        HAND_AXE,
        PALSTAVE_AXE, //light weight head using tang attachment instead of through hole
        CARPENTERS_AXE,
        FORESTRY_AXE,//general purpose tool axe
        FELLING_AXE,
        DOUBLE_BIT_FELLING_AXE,//heavier but twice the edge durability
        SPLITTING_AXE,
        BATTLE_AXE,//1 handed battle axe
        GREAT_AXE,//2 handed battle axe

        //shortblades
        KNIFE,//small blade
        DAGGER,//large, double edged knife
        SEAX,//large, single edged knife
        WAKIZASHI,
        GLADIUS,
        XIPHOS,
        BACKSWORD,
        SPATHA,//straight sword, minmal guard, rounded tip
        SABER,
        MESSER,
        FALCHION,
        KATANA,
        SCIMITAR,

        SMALLSWORD,
        RAPIER,
        ESTOC,

        ARMING_SWORD,//straight sword, cross guard, medium-point tip
        BASTARD_SWORD,
        LONGSWORD,
        CLAYMORE,
        GREATSWORD,

        //Sword
        SWORD,

        SHORT_SPEAR,
        SPEAR,
        STAFF,
        POLEAXE,
        HALBERD,
        PIKE,

        //Shields
        BUCKLER,
        ROUND_SHIELD,
        KITE_SHIELD,
        HEATER_SHIELD,
        TOWER_SHIELD,



        TROUSERS,

    }

    [Serializable]
    public class UItemData
    {
        public ITEM type;
        public UItemTemplate template;
        public UMATERIAL material;
        public QUALITY quality;
        public UItem item;

        public float3 pos;
        public float3 facing;
        public bool assembled;
        public float volumeComponent;
        public float3 lwhComponent;
        public float volumeAssembled;
        public float3 lwhAssembled;
        public float longestDim;

        //public List<UItemData> subItems;
        public Dictionary<string, UItemData> components;
        public float weightComponent;
        public float weightAssembled;
        public int id;
        public int ID
        {
            get { return this.id; }
            //set { this.id = value; } 
        }
        public float Weight { get { return weightAssembled; } }
        public float Length { get { return lwhAssembled.x; } }
        public float Width { get { return lwhAssembled.y; } }
        public float Height { get { return lwhAssembled.z; } }


        public List<EquippedBodyPartLayer> layers;

        public string GetName()
        {
            return quality.ToString() + " " + material.ToString() + " " + type.ToString();
        }

        public UItemData()
        {
            id = -1;
            type = 0;
            quality = 0;
            weightComponent = 0;
            weightAssembled = 0;
            lwhComponent = float3.zero;
            material = 0;
        }

        //public float CalcWeightAssembled()
        //{
        //    weightAssembled = subWeight;
        //    foreach (UItemData subItem in subItems)
        //    {
        //        subItem.CalcWeightAssembled();
        //        weightAssembled += subItem.weightAssembled;
        //    }
        //    return weightAssembled;
        //}
        //public float CalcDimsAssembled()
        //{
        //    weightAssembled = subWeight;
        //    foreach (UItemData subItem in subItems)
        //    {
        //        subItem.CalcWeightAssembled();
        //        weightAssembled += subItem.weightAssembled;
        //    }
        //    return weightAssembled;
        //}

        public void CalcProperties()
        {//calculate current properties, assembled or not
            template.CalcItemProps(this);
        }
    }

    public class UItem : MonoBehaviour
    {
        public Weapon weapon;
        public UItemData data;
        public void Awake()
        {
            data.item = this;
        }
        public void Start()
        {
            if(data.ID == -1)
            {
                data.id = ItemsManager.Instance.GetNextId();
            }
        }
    }



    #region Inspector

    #if UNITY_EDITOR
    [CustomEditor(typeof(UItem))]
    class UItemEditor : Editor
    {
        GUIStyle StyleGreen => MalbersAnimations.MTools.Style(new Color(0f, 1f, 0.5f, 0.3f));

        UItem item;

        private SerializedProperty data, type, quality, weightComponent, weightAssembled, totalWeight, id, lwh, material;


        //public List<EquippedBodyPartLayer> layers;

        public string typeString;
        Listened<string> textWithEvent = new Listened<string>(value =>
        {
            try
            {
                Debug.Log(value);
                ITEM type = (ITEM)System.Enum.Parse(typeof(ITEM), value);
            }
            catch (Exception)
            {

                throw;
            }
        });



        private void OnEnable()
        {
            item = (UItem)target;

            FindProperties();
            if (item.data == null)
            {
                item.data = new UItemData();
            }
        }

        private void FindProperties()
        {
            data = serializedObject.FindProperty("data");
            if (data != null)
            {
                type = data.FindPropertyRelative("type");
                quality = data.FindPropertyRelative("quality");
                weightComponent = data.FindPropertyRelative("weightComponent");
                weightAssembled = data.FindPropertyRelative("weightAssembled");
                id = data.FindPropertyRelative("id");
                lwh = data.FindPropertyRelative("lwh");
                material = data.FindPropertyRelative("material");
            }
        }
        public override void OnInspectorGUI()
        {
            //textWithEvent.Value = EditorGUILayout.TextField(textWithEvent.Value);

            GUI.changed = false;
            typeString = EditorGUILayout.TextField(typeString);
            if (GUI.changed && typeString.Length > 0)
            {
                try
                {
                    ITEM type = (ITEM)System.Enum.Parse(typeof(ITEM), typeString);
                    item.data.type = type;
                }
                catch (Exception)
                {

                }
            }

            serializedObject.Update();
            MalbersAnimations.MalbersEditor.DrawDescription("Urth Item ");

            //UItem uitem = (UItem)target;
            //if(uitem.data == null)
            //{
            //    var ST = new GUIStyle(EditorStyles.miniButtonMid) { fontStyle = FontStyle.Bold };

            //    if (GUILayout.Button(new GUIContent("Add Combat Layers",
            //        "There's no [Combat] layers on the current Animator. " +
            //        "This will add all the Animator Parameters and States needed for the using weapons while Riding"), ST))
            //    {
            //        AddItemData(uitem);
            //    }
            //    //serializedObject.Update();
            //    //FindProperties();
            //}
            //if (data == null)
            //{
            //    serializedObject.Update();
            //    FindProperties();
            //}
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                //EditorGUILayout.PropertyField(data);
                EditorGUILayout.DelayedTextField("");
                EditorGUILayout.PropertyField(type);
                EditorGUILayout.PropertyField(material);
                EditorGUILayout.PropertyField(quality);
                EditorGUILayout.PropertyField(weightComponent);
                EditorGUILayout.PropertyField(weightAssembled);
                EditorGUILayout.PropertyField(id);
            }

            serializedObject.ApplyModifiedProperties();

        }


        void AddItemData(UItem uitem)
        {
            uitem.data = new UItemData();
        }

        //public override void OnInspectorGUI()
        //{
        //    serializedObject.Update();
        //    MalbersEditor.DrawDescription("System for using weapons while a character is grounded & riding");

        //    //if (!Application.isPlaying)
        //    //    AddLayer();

        //    if (helpboxStyle == null)
        //    {
        //        helpboxStyle = new GUIStyle(MTools.StyleGray)
        //        {
        //            fontSize = 12,
        //            fontStyle = FontStyle.Bold,
        //            alignment = TextAnchor.MiddleLeft,
        //            stretchWidth = true
        //        };
        //        helpboxStyle.normal.textColor = EditorStyles.boldLabel.normal.textColor;
        //    }


        //    var Hols = "Holsters";

        //    if (!M.UseHolsters) Hols = "External";

        //    Editor_Tabs1.intValue = GUILayout.Toolbar(Editor_Tabs1.intValue, new string[] { "General", Hols, "AC", "Events" });

        //    if (Editor_Tabs1.intValue != 5) Editor_Tabs2.intValue = 5;

        //    Editor_Tabs2.intValue = GUILayout.Toolbar(Editor_Tabs2.intValue, new string[] { "Inputs", "Advanced", "Animator", "Debug" });

        //    if (Editor_Tabs2.intValue != 5) Editor_Tabs1.intValue = 5;

        //    //First Tabs
        //    int Selection = Editor_Tabs1.intValue;

        //    if (Selection == 0) DrawGeneral();
        //    else if (Selection == 1) ShowHolsters();
        //    else if (Selection == 2) DrawAC();
        //    else if (Selection == 3) DrawEvents();


        //    //2nd Tabs
        //    Selection = Editor_Tabs2.intValue;

        //    if (Selection == 0) DrawInputs();
        //    else if (Selection == 1) DrawAdvanced();
        //    else if (Selection == 2) DrawAnimator();
        //    else if (Selection == 3) DrawDebug();

        //    serializedObject.ApplyModifiedProperties();
        //}

    }
    #endif
    #endregion

}



//private void FindProperties()
//{
//    animal = serializedObject.FindProperty("animal");

//    m_AimInput = serializedObject.FindProperty("m_AimInput");
//    DropPoint = serializedObject.FindProperty("DropPoint");
//    m_ReloadInput = serializedObject.FindProperty("m_ReloadInput");
//    m_RightAttack = serializedObject.FindProperty("m_RightAttack");
//    m_LeftAttack = serializedObject.FindProperty("m_LeftAttack");
//    m_RightWindup = serializedObject.FindProperty("m_RightWindup");
//    m_LeftWindup = serializedObject.FindProperty("m_LeftWindup");
//    m_Guard = serializedObject.FindProperty("m_Guard");
//    //m_SpecialAttack = serializedObject.FindProperty("m_SpecialAttack");



//    DisableModes = serializedObject.FindProperty("DisableModes");
//    ExitOnState = serializedObject.FindProperty("ExitOnState");
//    ExitOnModes = serializedObject.FindProperty("ExitOnModes");
//    ExitFast = serializedObject.FindProperty("ExitFast");

//    Anim = serializedObject.FindProperty("anim");

//    start_weapon = serializedObject.FindProperty("startWeapon");
//    aim = serializedObject.FindProperty("aim");

//    IgnoreHandOffset = serializedObject.FindProperty("IgnoreHandOffset");
//    comboManager = serializedObject.FindProperty("comboManager");
//    DrawWeapon = serializedObject.FindProperty("DrawWeaponModeID");
//    StoreWeapon = serializedObject.FindProperty("StoreWeaponModeID");
//    UnarmedMode = serializedObject.FindProperty("UnarmedModeID");

//    // UseDefaultIK = serializedObject.FindProperty("UseDefaultIK");
//    StoreAfter = serializedObject.FindProperty("StoreAfter");
//    m_IgnoreDraw = serializedObject.FindProperty("m_IgnoreDraw");
//    m_IgnoreStore = serializedObject.FindProperty("m_IgnoreStore");
//    // DisableAim = serializedObject.FindProperty("DisableAim");


//    #region Animator Parameters
//    m_LeftWeaponType = serializedObject.FindProperty("m_LeftWeaponType");
//    m_RightWeaponType = serializedObject.FindProperty("m_RightWeaponType");
//    m_LeftHand = serializedObject.FindProperty("m_LeftHand");
//    m_IKFreeHand = serializedObject.FindProperty("m_IKFreeHand");
//    m_IKAim = serializedObject.FindProperty("m_IKAim");
//    m_ModeOn = serializedObject.FindProperty("m_ModeOn");
//    m_Mode = serializedObject.FindProperty("m_Mode");
//    m_WeaponPower = serializedObject.FindProperty("m_WeaponPower");
//    #endregion


//    holsters = serializedObject.FindProperty("holsters");
//    //  DefaultHolster = serializedObject.FindProperty("DefaultHolster");
//    HolsterTime = serializedObject.FindProperty("HolsterTime");



//    // ExitCombatOnDismount = serializedObject.FindProperty("ExitCombatOnDismount");

//    Editor_Tabs1 = serializedObject.FindProperty("Editor_Tabs1");
//    Editor_Tabs2 = serializedObject.FindProperty("Editor_Tabs2");
//    //HitMask = serializedObject.FindProperty("HitMask"); 


//    LeftHandEquipPoint = serializedObject.FindProperty("LeftHandEquipPoint");
//    RightHandEquipPoint = serializedObject.FindProperty("RightHandEquipPoint");

//    OnCombatMode = serializedObject.FindProperty("OnCombatMode");
//    OnCanAim = serializedObject.FindProperty("OnCanAim");
//    OnEquipWeapon = serializedObject.FindProperty("OnEquipWeapon");
//    OnUnequipWeapon = serializedObject.FindProperty("OnUnequipWeapon");
//    OnWeaponAction = serializedObject.FindProperty("OnWeaponAction");
//    //   OnMainAttackStart = serializedObject.FindProperty("OnMainAttackStart");


//    m_CombatLayerName = serializedObject.FindProperty("m_CombatLayerName");
//    m_CombatLayerPath = serializedObject.FindProperty("m_CombatLayerPath");




//    UseExternal = serializedObject.FindProperty("UseExternal");
//    UseHolsters = serializedObject.FindProperty("UseHolsters");


//    DestroyOnUnequip = serializedObject.FindProperty("DestroyOnUnequip");
//    InstantiateOnEquip = serializedObject.FindProperty("InstantiateOnEquip");


//    debug = serializedObject.FindProperty("debug");
//}


/// <summary> Draws all of the fields for the selected ability. </summary>

/*

  private void DrawInputs()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          EditorGUILayout.PropertyField(m_AimInput);
          EditorGUILayout.PropertyField(m_ReloadInput);
          EditorGUILayout.PropertyField(m_RightAttack);
          EditorGUILayout.PropertyField(m_LeftAttack);
          EditorGUILayout.PropertyField(m_RightWindup);
          EditorGUILayout.PropertyField(m_LeftWindup);
          EditorGUILayout.PropertyField(m_Guard);
      }

      if (M.UseHolsters)
      {
          using (new GUILayout.VerticalScope(EditorStyles.helpBox))
          {
              for (int i = 0; i < holsters.arraySize; i++)
              {
                  var element = holsters.GetArrayElementAtIndex(i);
                  var input = element.FindPropertyRelative("Input");
                  EditorGUILayout.PropertyField(input, new GUIContent($"Holster [{i}] Input"));
              }
          }
      }

  }

  private void DrawAC()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          EditorGUILayout.PropertyField(animal);

          if (animal.objectReferenceValue != null)
          {
              EditorGUILayout.PropertyField(DrawWeapon, new GUIContent("Mode [Draw Weapon]"));
              EditorGUILayout.PropertyField(StoreWeapon, new GUIContent("Mode [Store Weapon]"));
              EditorGUILayout.PropertyField(UnarmedMode, new GUIContent("Mode [Unarmed]"));

              EditorGUILayout.Space();

              EditorGUILayout.PropertyField(comboManager);
              EditorGUILayout.Space();

              EditorGUI.indentLevel++;
              EditorGUILayout.PropertyField(DisableModes);
              EditorGUI.indentLevel--;

              EditorGUI.indentLevel++;
              EditorGUILayout.PropertyField(ExitOnState);
              EditorGUI.indentLevel--;

              EditorGUI.indentLevel++;
              EditorGUILayout.PropertyField(ExitOnModes);
              EditorGUI.indentLevel--;

              EditorGUILayout.PropertyField(ExitFast);
          }
      }
  }

  private void DrawAnimator()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          EditorGUILayout.PropertyField(Anim);
          EditorGUILayout.LabelField("Animator Parameters", EditorStyles.boldLabel);
          //     DisplayParam(m_WeaponAction, UnityEngine.AnimatorControllerParameterType.Int);
          MalbersEditor.DisplayParam(M.Anim, m_LeftWeaponType, UnityEngine.AnimatorControllerParameterType.Int);
          MalbersEditor.DisplayParam(M.Anim, m_RightWeaponType, UnityEngine.AnimatorControllerParameterType.Int);
          MalbersEditor.DisplayParam(M.Anim, m_LeftHand, UnityEngine.AnimatorControllerParameterType.Bool);
          MalbersEditor.DisplayParam(M.Anim, m_IKFreeHand, UnityEngine.AnimatorControllerParameterType.Float);
          MalbersEditor.DisplayParam(M.Anim, m_IKAim, UnityEngine.AnimatorControllerParameterType.Float);
          MalbersEditor.DisplayParam(M.Anim, m_Guard, UnityEngine.AnimatorControllerParameterType.Bool);

          if (M.animal == null)
          {
              MalbersEditor.DisplayParam(M.Anim, m_ModeOn, UnityEngine.AnimatorControllerParameterType.Trigger);
              MalbersEditor.DisplayParam(M.Anim, m_Mode, UnityEngine.AnimatorControllerParameterType.Int);
              MalbersEditor.DisplayParam(M.Anim, m_WeaponPower, UnityEngine.AnimatorControllerParameterType.Float);
          }
      }


      EditorGUILayout.LabelField("Weapon Ability Values:" +
        "\nNone = 0" +
        "\nIdle = 100" +
        "\nAim = 97" +
        "\nReload = 96" +
        "\nDraw = 99" +
        "\nStore = 98", helpboxStyle);
  }
  private GUIStyle helpboxStyle;

  private void DrawDebug()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          EditorGUILayout.PropertyField(debug, new GUIContent("Debug", ""));

          if (Application.isPlaying)
          {
              using (new EditorGUI.DisabledGroupScope(true))
              {
                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                      if (M.HasAnimal) EditorGUILayout.Toggle("Preparing Mode", M.animal.IsPreparingMode);
                      EditorGUILayout.Toggle("Is In Combat mode", M.CombatMode);
                      EditorGUILayout.Toggle("Is Riding: ", M.IsRiding);
                  }

                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                      EditorGUILayout.Toggle("Is Aiming", M.Aim);
                      if (M.Aimer != null) EditorGUILayout.Toggle("Aiming Side", M.AimingSide);
                  }
                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                      EditorGUILayout.FloatField("IK Aim", M.IKAimWeight);
                      EditorGUILayout.FloatField("IK 2Hands", M.IK2HandsWeight);
                  }

                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                      EditorGUILayout.EnumPopup("Weapon Action: ", M.LeftWeaponAction);
                      EditorGUILayout.IntField("Anim Action: ", M.WeaponAnimAction);
                  }

                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                      EditorGUILayout.ObjectField("Active Right Weapon:  ", M.RightWeapon, typeof(MWeapon), false);

                      if (M.RightWeapon)
                      {
                          EditorGUILayout.ObjectField("Active Holster:  ", M.RightActiveHolster?.ID, typeof(HolsterID), false);
                          EditorGUILayout.ObjectField("Weapon.Type:  ", M.RightWeapon?.WeaponType, typeof(WeaponID), false);
                          EditorGUILayout.Toggle("Weapon.Active: ", M.RightWeapon.Enabled);
                          EditorGUILayout.Toggle("Weapon.Input: ", M.RightWeapon.Input);
                          EditorGUILayout.Toggle("Weapon.IsAiming: ", M.RightWeapon.IsAiming);
                          EditorGUILayout.Toggle("Weapon.RightHand: ", M.RightWeapon.IsRightHanded);
                          EditorGUILayout.Toggle("Weapon.Ready: ", M.RightWeapon.IsReady);
                          EditorGUILayout.Toggle("Weapon.CanAttack: ", M.RightWeapon.CanAttack);
                          EditorGUILayout.Toggle("Weapon.IsAttacking: ", M.RightWeapon.IsAttacking);
                          EditorGUILayout.Toggle("Weapon.IsReloading: ", M.RightWeapon.IsReloading);
                          EditorGUILayout.Toggle("Weapon.CanCharge: ", M.RightWeapon.CanCharge);
                          EditorGUILayout.Toggle("Weapon.HasAmmo: ", M.RightWeapon.HasAmmo);

                          if (M.RightWeapon.CanCharge)
                          {
                              EditorGUILayout.Toggle("Weapon.IsCharging: ", M.RightWeapon.IsCharging);
                              EditorGUILayout.FloatField("Weapon.Power: ", M.RightWeapon.Power);
                              EditorGUILayout.FloatField("Weapon.ChargeNorm: ", M.RightWeapon.ChargedNormalized);
                          }
                      }
                  }
                  using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                  {
                      EditorGUILayout.ObjectField("Active Left Weapon:  ", M.LeftWeapon, typeof(MWeapon), false);

                      if (M.LeftWeapon)
                      {
                          EditorGUILayout.ObjectField("Active Holster:  ", M.LeftActiveHolster?.ID, typeof(HolsterID), false);
                          EditorGUILayout.ObjectField("Weapon.Type:  ", M.LeftWeapon?.WeaponType, typeof(WeaponID), false);
                          EditorGUILayout.Toggle("Weapon.Active: ", M.LeftWeapon.Enabled);
                          EditorGUILayout.Toggle("Weapon.Input: ", M.LeftWeapon.Input);
                          EditorGUILayout.Toggle("Weapon.IsAiming: ", M.LeftWeapon.IsAiming);
                          EditorGUILayout.Toggle("Weapon.RightHand: ", M.RightWeapon.IsRightHanded);
                          EditorGUILayout.Toggle("Weapon.Ready: ", M.LeftWeapon.IsReady);
                          EditorGUILayout.Toggle("Weapon.CanAttack: ", M.LeftWeapon.CanAttack);
                          EditorGUILayout.Toggle("Weapon.IsAttacking: ", M.LeftWeapon.IsAttacking);
                          EditorGUILayout.Toggle("Weapon.IsReloading: ", M.LeftWeapon.IsReloading);
                          EditorGUILayout.Toggle("Weapon.CanCharge: ", M.LeftWeapon.CanCharge);
                          EditorGUILayout.Toggle("Weapon.HasAmmo: ", M.LeftWeapon.HasAmmo);

                          if (M.LeftWeapon.CanCharge)
                          {
                              EditorGUILayout.Toggle("Weapon.IsCharging: ", M.LeftWeapon.IsCharging);
                              EditorGUILayout.FloatField("Weapon.Power: ", M.LeftWeapon.Power);
                              EditorGUILayout.FloatField("Weapon.ChargeNorm: ", M.LeftWeapon.ChargedNormalized);
                          }
                      }
                  }
                  Repaint();
              }
          }
      }
  }

  private void ShowHolsters()
  {
      EditorGUILayout.Space();
      var selection = UseHolsters.boolValue ? 0 : 1;
      selection = GUILayout.Toolbar(selection, new string[] { "Use Holsters", "Use External" });
      UseExternal.boolValue = selection != 0;
      UseHolsters.boolValue = selection == 0;


      if (styleDesc == null)
      {
          styleDesc = new GUIStyle(StyleGreen)
          {
              fontSize = 12,
              fontStyle = FontStyle.Bold,
              alignment = TextAnchor.MiddleLeft,
              stretchWidth = true
          };

          styleDesc.normal.textColor = EditorStyles.label.normal.textColor;
      }


      if (UseExternal.boolValue)
      {
          EditorGUILayout.LabelField("Use the Method <Equip_External(GameObject)> to equip weapons", styleDesc);
          using (new GUILayout.VerticalScope(EditorStyles.helpBox))
          {
              EditorGUILayout.PropertyField(InstantiateOnEquip);
              EditorGUILayout.PropertyField(DestroyOnUnequip);
          }
      }

      //Holder Stufss
      if (M.UseHolsters)
      {
          EditorGUILayout.LabelField("The weapons are child of the Holsters", styleDesc);

          // EditorGUILayout.PropertyField(DefaultHolster, new GUIContent("Default Holster", "Default  Holster used when no Holster is selected"));
          EditorGUILayout.PropertyField(HolsterTime, new GUIContent("Holster Time", "Time to smooth parent the weapon to the Hand and Holster"));

          EditorGUILayout.PropertyField(DropPoint);

          holsterReordable.DoLayoutList();

          if (holsterReordable.index != -1)
          {
              using (new GUILayout.VerticalScope(EditorStyles.helpBox))
              {
                  var element = holsters.GetArrayElementAtIndex(holsterReordable.index);
                  var Input = element.FindPropertyRelative("Input");
                  var Slots = element.FindPropertyRelative("Slots");
                  EditorGUILayout.PropertyField(Input);
                  EditorGUI.indentLevel++;
                  EditorGUILayout.PropertyField(Slots, true);
                  EditorGUI.indentLevel--;
              }
          }
      }
  }

  private void DrawGeneral()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          var dC = start_weapon.displayName;
          if (M.StartWeapon && M.StartWeapon.IsPrefab())
          {
              dC += " [Prefab]";
          }

          using (new GUILayout.HorizontalScope())
          {
              EditorGUILayout.PropertyField(start_weapon, new GUIContent(dC, start_weapon.tooltip));
              MalbersEditor.DrawDebugIcon(debug);
          }

          //using (var cc = new EditorGUI.ChangeCheckScope())
          //{
          //    EditorGUILayout.PropertyField(aim);
          //    if (cc.changed && Application.isPlaying)
          //    {
          //        serializedObject.ApplyModifiedProperties();
          //        M.SetAimLogic(M.Aim);
          //        //Debug.Log("M.Aim = " + M.Aim);
          //    }
          //}


          EditorGUILayout.PropertyField(m_IgnoreDraw);
          EditorGUILayout.PropertyField(m_IgnoreStore);
          EditorGUILayout.PropertyField(StoreAfter);
      }

      EquipWeaponPoints();
  }

  private void EquipWeaponPoints()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          LeftHandEquipPoint.isExpanded = MalbersEditor.Foldout(LeftHandEquipPoint.isExpanded, "Weapon Equip Points");

          if (LeftHandEquipPoint.isExpanded)
          {
              EditorGUILayout.PropertyField(LeftHandEquipPoint, new GUIContent("Left Hand"));
              EditorGUILayout.PropertyField(RightHandEquipPoint, new GUIContent("Right Hand"));
              EditorGUILayout.PropertyField(IgnoreHandOffset);
          }


          //Animator Anim = M.GetComponent<Animator>();
          //if (Anim)
          //{
          //    if (LeftHandEquipPoint.objectReferenceValue == null)
          //    {
          //        M.LeftHandEquipPoint = Anim.GetBoneTransform(HumanBodyBones.LeftHand);
          //    }

          //    if (RightHandEquipPoint.objectReferenceValue == null)
          //    {
          //        M.RightHandEquipPoint = Anim.GetBoneTransform(HumanBodyBones.RightHand);
          //    }
          //}
      }

  }
  private void DrawAdvanced()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          if (M.animal == null)
              AddLayers();


          EditorGUILayout.LabelField(new GUIContent("Combat Animator", "Location and Name of the Combat while Riding Layer, on the Resource folder"), EditorStyles.boldLabel);
          EditorGUILayout.PropertyField(m_CombatLayerName, new GUIContent("Layer Name", "Name of the Riding Combat Layer"));
          EditorGUILayout.PropertyField(m_CombatLayerPath, new GUIContent("Animator Path", "Path of the Combat Layer on the Resource Folder"));

      }
  }


  private GUIStyle styleDesc;

  void DrawEvents()
  {
      using (new GUILayout.VerticalScope(EditorStyles.helpBox))
      {
          EditorGUILayout.PropertyField(OnCombatMode);
          EditorGUILayout.PropertyField(OnCanAim);
          EditorGUILayout.PropertyField(OnEquipWeapon);
          EditorGUILayout.PropertyField(OnUnequipWeapon);
          //  EditorGUILayout.PropertyField(OnMainAttackStart);
          EditorGUILayout.PropertyField(OnWeaponAction);
      }

      if (M.UseHolsters)
      {
          using (new GUILayout.VerticalScope(EditorStyles.helpBox))
          {
              for (int i = 0; i < holsters.arraySize; i++)
              {
                  var element = holsters.GetArrayElementAtIndex(i);
                  var input = element.FindPropertyRelative("OnWeaponInHolster");
                  EditorGUILayout.PropertyField(input, new GUIContent($" On Weapon In Holster [{i}]"));
              }
          }
      }
  }

  void AddLayerCombat(UnityEditor.Animations.AnimatorController CurrentAnimator)
  {
      var m_CombatLayerPath = serializedObject.FindProperty("m_CombatLayerPath");

      UnityEditor.Animations.AnimatorController MountAnimator =
          Resources.Load<UnityEditor.Animations.AnimatorController>(m_CombatLayerPath.stringValue);

      MTools.AddParametersOnAnimator(CurrentAnimator, MountAnimator);

      foreach (var item in MountAnimator.layers)
          CurrentAnimator.AddLayer(item);
  }

  private void AddLayers()
  {
      Animator anim = M.GetComponent<Animator>();

      if (anim)
      {
          var controller = (UnityEditor.Animations.AnimatorController)anim.runtimeAnimatorController;

          if (controller)
          {
              var layers = controller.layers.ToList();

              var defaultColor = GUI.color;
              GUI.color = Color.green;

              var ST = new GUIStyle(EditorStyles.miniButtonMid) { fontStyle = FontStyle.Bold };



              if (layers.Find(layer => layer.name == m_CombatLayerName.stringValue) == null)
              {
                  if (GUILayout.Button(new GUIContent("Add Combat Layers",
                      "There's no [Combat] layers on the current Animator. " +
                      "This will add all the Animator Parameters and States needed for the using weapons while Riding"), ST))
                  {
                      AddLayerCombat(controller);
                  }
              }
              GUI.color = defaultColor;

          }
      }
  }
}
#endif
#endregion
*/