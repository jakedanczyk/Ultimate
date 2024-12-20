using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

namespace Urth
{
    [Serializable]
    public struct StatPair
    {
        public StatType type;
        public int val;
        public StatPair(StatType it, int iv)
        {
            type = it; val = iv;
        }
    }
    public class CharacterCreationMenuControl : MonoBehaviour
    {
        public MalbersAnimations.Controller.MAnimal playerAnimal;
        public List<MonoBehaviour> otherPlayerMonos;
        public PlayerCreatureManager playerCreatureManager;
        public CREATURE species;
        public StatPair[] stats;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetPlayerCharacter()
        {
            CreatureStatCollection newStats = new CreatureStatCollection();
            newStats.ConfigureStats();
            foreach (StatPair statPair in stats)
            {
                newStats.GetStat(statPair.type).StatBaseValue = statPair.val;
            }

            CreatureData data = new CreatureData(0, CREATURE.HUMAN, GENDER.MALE, -20, 80, .3f, 1.91f, .6f, Unity.Mathematics.float3.zero);
            CreatureBody playerBody = new CreatureBody(data);
            playerCreatureManager.body = playerBody;

            playerCreatureManager.body.stats.statInterface.statCollection = newStats;
            playerCreatureManager.body.type = species;
            playerCreatureManager.body.data.type = species;
            GameManager.Instance.playerCharacterBody = playerCreatureManager.body;
        }

        public void Initialize()
        {
            int idx = 0;
            foreach (StatType statType in UrthConstants.BODY_ATTRIBUTES)
            {
                stats[idx] = new StatPair(statType, UrthConstants.ATTRIB_LVL_DEFAULT);
                idx++;
            }
            foreach (StatType statType in UrthConstants.MIND_ATTRIBUTES)
            {
                stats[idx] = new StatPair(statType, UrthConstants.ATTRIB_LVL_DEFAULT);
                idx++;
            }
        }

        public void Activate()
        {
            foreach (MonoBehaviour mono in otherPlayerMonos)
            {
                mono.enabled = true;
            }
        }
    }

    [CustomEditor(typeof(CharacterCreationMenuControl))]
    public class CharacterCreationMenuControlEditor : Editor
    {
        CharacterCreationMenuControl menuControl;

        public override void OnInspectorGUI()
        {
            GUILayout.Label("For now: 1 fill character sheet \n 2 set player stats\n" +
                "3 activate animale \n 4 activate monos");
            if (GUILayout.Button("Fill Character Creation Sheet Stats"))
            {
                menuControl.Initialize();
            }
            if (GUILayout.Button("Set Player Stats"))
            {
                menuControl.SetPlayerCharacter();
                //menuControl.playerCreature.body
            }
            if (GUILayout.Button("Activate Animal"))
            {
                menuControl.playerAnimal.enabled = true;
            }
            if (GUILayout.Button("Activate OtherMonos"))
            {
                menuControl.Activate();
            }

            DrawDefaultInspector();
        }



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
            menuControl = (CharacterCreationMenuControl)target;

            //FindProperties();
            //if (item.data == null)
            //{
            //    item.data = new UItemData();
            //}
        }

        private void FindProperties()
        {
            //data = serializedObject.FindProperty("data");
            //if (data != null)
            //{
            //    type = data.FindPropertyRelative("type");
            //}
        }


    }
}

