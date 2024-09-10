using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public static class UrthConstants
    {
        //public static int HUD_RAYCAST_LENGTH = 
        public static int IDLES_IDX = 0;
        public static int GUARDS_IDX = 1;
        public static int WORK_IDX = 2;
        public static int ACTIONS_IDX = 3;
        public static int CHARGE_SWING_IDX = 4;
        public static int CHARGE_CUT_IDX = 5;
        public static int CHARGE_STAB_IDX = 6;
        public static int SWING_IDX = 7;
        public static int CUT_IDX = 8;
        public static int STAB_IDX = 9;
        public static int WEAPON_ACTIONS_BASE = 1000;
        public static int TOOL_ACTIONS_BASE = 2000;
        public static int GUARD_ACTIONS_BASE = 3000;
        public const string ITEM_TAG = "Item";
        public const string WORKSITE_TAG = "Worksite";
        public const string CONSTRUCTION_TAG = "Construction";

        public static string CHARACTER_INTERFACE = "CharacterInterface";
        public static string CONSTRUCTION_INTERFACE = "ConstructionInterface";
        public static string CONSTRUCTION_PANEL = "ConstructionPanel";
        public static string CONSTRUCTION_PLANNING_INTERFACE = "ConstructionPlanningInterface";
        public static string CONSTRUCTION_ITEMS_PANEL = "ConstructionItemsPanel";
        public static string CONSTRUCTION_SETTINGS_PANEL = "ConstructionSettingsPanel";
        public static string INVENTORY_PANEL = "InventoryPanel";
        public static string ITEM_DISPLAY_PANEL = "ItemDisplayPanel";
        
        public static string UI_ACTION_MAP = "UI";
        public static string CHARACTER_ACTION_MAP = "CharacterUI";
        public static string CONSTRUCTION_ACTION_MAP = "ConstructionUI";
        public static string GAMEPLAY_ACTION_MAP = "Gameplay";
        public static string ITEM_TEMPLATES_XML = "Gameplay/Items/ItemTemplates.xml";
        public static string STATIC_TEMPLATES_XML = "Gameplay/Statics/StaticTemplates.xml";

        public static float LOG_VOLUME = 0.0245436926f;//0.25m dia by 0.5m length


        public static StatType[] BODY_ATTRIBUTES = new StatType[] {
            StatType.Agility, StatType.Balance, StatType.Coordination, StatType.Dexterity, StatType.Endurance,
            StatType.Flexibility, StatType.Reflex , StatType.Speed, StatType.Strength, StatType.Toughness
        };
        public static StatType[] MIND_ATTRIBUTES = new StatType[] {
            StatType.Charisma, StatType.Creativity, StatType.Empathy, StatType.Focus, StatType.Intellect,
            StatType.Memory, StatType.Patience , StatType.Perception, StatType.Willpower, StatType.Wisdom
        };
        public const int ATTRIB_LVL_DEFAULT = 20;

        public static List<(BodyPartId, float, CONTAINER)> standardBodyContainers = new List<(BodyPartId, float, CONTAINER)>
        {
            (new BodyPartId(BODY_PART.HAND,SYMMETRY.LEFT), 0.1f, CONTAINER.HAND),
            (new BodyPartId(BODY_PART.HAND,SYMMETRY.RIGHT), 0.1f, CONTAINER.HAND),
            (new BodyPartId(BODY_PART.HEAD,SYMMETRY.CENTER), 0.05f, CONTAINER.MOUTH),
        };


        #region Biomes data
        public static readonly float BIOME_TEMP_ICE = -10f;
        public static readonly float BIOME_TEMP_TUNDRA = 0f;
        public static readonly float BIOME_TEMP_BOREAL = 3f;
        public static readonly float BIOME_TEMP_COOL = 6f;
        public static readonly float BIOME_TEMP_WARM = 12f;
        public static readonly float BIOME_TEMP_SUBTROPIC = 18f;
        public static readonly float BIOME_TEMP_TROPIC = 24f;
        public static readonly float ARIDITY_RAIN = 0.25f;
        public static readonly float ARIDITY_WET = 0.5f;
        public static readonly float ARIDITY_MOIST = 1f;
        public static readonly float ARIDITY_MILD = 2f;
        public static readonly float ARIDITY_DRY = 4f;
        public static readonly float ARIDITY_DESERT = 8f;
        public static readonly float ARIDITY_EXTREME = 16f;

        static readonly float LOG_MIN_ARIDITY = Mathf.Log(ARIDITY_RAIN, 2f);
        static readonly float LOG_MAX_ARIDITY = Mathf.Log(ARIDITY_EXTREME, 2f);


        //static readonly NativeArray<float> BIOME_TEMPS = new NativeArray<float>(BIOME_TEMPS_ARR.Length, Allocator.Temp);// { BIOME_TEMP_ICE, BIOME_TEMP_TUNDRA, BIOME_TEMP_BOREAL, BIOME_TEMP_COOL, BIOME_TEMP_WARM, BIOME_TEMP_SUBTROPIC, BIOME_TEMP_TROPIC };

        //public void PopulateBiomes()
        //{
        //    BIOME_TEMPS = new NativeArray<float>(BIOME_TEMPS_ARR.Length, Allocator.Temp){ BIOME_TEMP_ICE, BIOME_TEMP_TUNDRA, BIOME_TEMP_BOREAL, BIOME_TEMP_COOL, BIOME_TEMP_WARM, BIOME_TEMP_SUBTROPIC, BIOME_TEMP_TROPIC };
        //}
        public static readonly float[][] BIOME_ARIDS = new float[][]
        {
        new float[] { ARIDITY_MOIST, ARIDITY_DRY},
        new float[] { ARIDITY_RAIN, ARIDITY_WET, ARIDITY_MOIST, ARIDITY_MILD, ARIDITY_DRY},
        new float[] { ARIDITY_RAIN, ARIDITY_WET, ARIDITY_MOIST, ARIDITY_MILD, ARIDITY_DRY},
        new float[] { ARIDITY_RAIN, ARIDITY_WET, ARIDITY_MOIST, ARIDITY_MILD, ARIDITY_DRY, ARIDITY_DESERT},
        new float[] { ARIDITY_RAIN, ARIDITY_WET, ARIDITY_MOIST, ARIDITY_MILD, ARIDITY_DRY, ARIDITY_DESERT, ARIDITY_EXTREME},
        new float[] { ARIDITY_RAIN, ARIDITY_WET, ARIDITY_MOIST, ARIDITY_MILD, ARIDITY_DRY, ARIDITY_DESERT, ARIDITY_EXTREME},
        };
        public static readonly BIOME[][] BIOME_ARR = new BIOME[][]
        {
        new BIOME[] { BIOME.ICE_SHEET, BIOME.FROZEN_DESERT},
        new BIOME[] { BIOME.ARTIC_RAIN, BIOME.ARTIC_WET, BIOME.ARTIC_MOIST, BIOME.ARTIC_DRY, BIOME.ARTIC_DESERT},
        new BIOME[] { BIOME.ARTIC_RAIN, BIOME.ARTIC_WET, BIOME.ARTIC_MOIST, BIOME.ARTIC_DRY, BIOME.ARTIC_DESERT},
        new BIOME[] { BIOME.ARTIC_RAIN, BIOME.ARTIC_WET, BIOME.ARTIC_MOIST, BIOME.ARTIC_DRY, BIOME.ARTIC_DESERT},
        new BIOME[] { BIOME.ARTIC_RAIN, BIOME.ARTIC_WET, BIOME.ARTIC_MOIST, BIOME.ARTIC_DRY, BIOME.ARTIC_DESERT},
        };

        static Dictionary<BIOME, float2> BIOMES_DICT = new Dictionary<BIOME, float2>()
        {
            [BIOME.ICE_SHEET] = new float2(BIOME_TEMP_ICE, ARIDITY_MOIST),
            [BIOME.FROZEN_DESERT] = new float2(BIOME_TEMP_ICE, ARIDITY_DRY),

            [BIOME.ARTIC_RAIN] = new float2(BIOME_TEMP_TUNDRA, ARIDITY_RAIN),
            [BIOME.ARTIC_WET] = new float2(BIOME_TEMP_TUNDRA, ARIDITY_WET),
            [BIOME.ARTIC_MOIST] = new float2(BIOME_TEMP_TUNDRA, ARIDITY_MOIST),
            [BIOME.ARTIC_DRY] = new float2(BIOME_TEMP_TUNDRA, ARIDITY_MILD),
            [BIOME.ARTIC_DESERT] = new float2(BIOME_TEMP_TUNDRA, ARIDITY_DRY),

            [BIOME.SUBARTIC_RAIN] = new float2(BIOME_TEMP_BOREAL, ARIDITY_RAIN),
            [BIOME.SUBARTIC_WET] = new float2(BIOME_TEMP_BOREAL, ARIDITY_WET),
            [BIOME.SUBARTIC_MOIST] = new float2(BIOME_TEMP_BOREAL, ARIDITY_MOIST),
            [BIOME.SUBARTIC_DRY] = new float2(BIOME_TEMP_BOREAL, ARIDITY_MILD),
            [BIOME.SUBARTIC_DESERT] = new float2(BIOME_TEMP_BOREAL, ARIDITY_DRY),

            [BIOME.COOL_RAIN] = new float2(BIOME_TEMP_COOL, ARIDITY_RAIN),
            [BIOME.COOL_WET] = new float2(BIOME_TEMP_COOL, ARIDITY_WET),
            [BIOME.COOL_MOIST] = new float2(BIOME_TEMP_COOL, ARIDITY_MOIST),
            [BIOME.COOL_MILD] = new float2(BIOME_TEMP_COOL, ARIDITY_MILD),
            [BIOME.COOL_DRY] = new float2(BIOME_TEMP_COOL, ARIDITY_DRY),
            [BIOME.COOL_DESERT] = new float2(BIOME_TEMP_COOL, ARIDITY_DESERT),

            [BIOME.WARM_RAIN] = new float2(BIOME_TEMP_WARM, ARIDITY_RAIN),
            [BIOME.WARM_WET] = new float2(BIOME_TEMP_WARM, ARIDITY_WET),
            [BIOME.WARM_MOIST] = new float2(BIOME_TEMP_WARM, ARIDITY_MOIST),
            [BIOME.WARM_MILD] = new float2(BIOME_TEMP_WARM, ARIDITY_MILD),
            [BIOME.WARM_DRY] = new float2(BIOME_TEMP_WARM, ARIDITY_DRY),
            [BIOME.WARM_DESERT] = new float2(BIOME_TEMP_WARM, ARIDITY_DESERT),
            [BIOME.WARM_EXTREME] = new float2(BIOME_TEMP_WARM, ARIDITY_EXTREME),

            [BIOME.SUBTROPIC_RAIN] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_RAIN),
            [BIOME.SUBTROPIC_WET] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_WET),
            [BIOME.SUBTROPIC_MOIST] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_MOIST),
            [BIOME.SUBTROPIC_MILD] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_MILD),
            [BIOME.SUBTROPIC_DRY] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_DRY),
            [BIOME.SUBTROPIC_DESERT] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_DESERT),
            [BIOME.SUBTROPIC_EXTREME] = new float2(BIOME_TEMP_SUBTROPIC, ARIDITY_EXTREME),

            [BIOME.TROPIC_RAIN] = new float2(BIOME_TEMP_TROPIC, ARIDITY_RAIN),
            [BIOME.TROPIC_WET] = new float2(BIOME_TEMP_TROPIC, ARIDITY_WET),
            [BIOME.TROPIC_MOIST] = new float2(BIOME_TEMP_TROPIC, ARIDITY_MOIST),
            [BIOME.TROPIC_MILD] = new float2(BIOME_TEMP_TROPIC, ARIDITY_MILD),
            [BIOME.TROPIC_DRY] = new float2(BIOME_TEMP_TROPIC, ARIDITY_DRY),
            [BIOME.TROPIC_DESERT] = new float2(BIOME_TEMP_TROPIC, ARIDITY_DESERT),
            [BIOME.TROPIC_EXTREME] = new float2(BIOME_TEMP_TROPIC, ARIDITY_EXTREME),
        };

        public static Dictionary<int2, BIOME> ReverseBiomes()
        {
            Dictionary<int2, BIOME> reversed = new Dictionary<int2, BIOME>() { };
            foreach (var item in BIOMES_DICT)
            {
                reversed[(int2)item.Value] = item.Key;
            }
            return reversed;
        }
        static Dictionary<int2, BIOME> CLIMATE_TO_BIOMES_DICT = ReverseBiomes();


        //public static readonly float[] BIOME_TEMPS = new float[] { BIOME_TEMP_ICE, BIOME_TEMP_TUNDRA, BIOME_TEMP_BOREAL, BIOME_TEMP_COOL, BIOME_TEMP_WARM, BIOME_TEMP_SUBTROPIC, BIOME_TEMP_TROPIC };
        static NativeArray<float> BIOME_TEMPS = new NativeArray<float>(
            new float[] { BIOME_TEMP_ICE, BIOME_TEMP_TUNDRA, BIOME_TEMP_BOREAL, BIOME_TEMP_COOL, BIOME_TEMP_WARM, BIOME_TEMP_SUBTROPIC, BIOME_TEMP_TROPIC },
            Allocator.Temp);

        static public NativeArray<float2> GetRegionalBiomeScores(float h, float t, float a)
        {
            NativeArray<float2> biomes = new NativeArray<float2>(4, Allocator.Temp);
            float tIdx = 0f, hotFrac = 0f, coldFrac = 0f;
            int tTierCold = 0;
            //find temperature tier
            if (t >= BIOME_TEMPS[BIOME_TEMPS.Length - 1])
            {
                tIdx = BIOME_TEMPS[BIOME_TEMPS.Length - 1];
                tTierCold = (int)tIdx;
                hotFrac = 0f;
                coldFrac = 1f;
            }
            else if (t > BIOME_TEMPS[0])
            {
                for (int i = 1; i < BIOME_TEMPS.Length; i++)
                {
                    if (t < BIOME_TEMPS[i])
                    {
                        float frac = (t - BIOME_TEMPS[i - 1]) / (BIOME_TEMPS[i] - BIOME_TEMPS[i - 1]);
                        tIdx = (float)i + frac - 1f;
                        break;
                    }
                }
                tTierCold = (int)tIdx;
                hotFrac = (tIdx - tTierCold);
                coldFrac = 1f - hotFrac;
            }

            float[] tempFracs = new float[2] { coldFrac, hotFrac };

            for (int tTierHotColdIdx = 0; tTierHotColdIdx < 2; tTierHotColdIdx++)
            {
                float tempFrac = tempFracs[tTierHotColdIdx];
                if (tempFrac > 0f)
                {
                    int tTierIdx = tTierCold + tTierHotColdIdx;
                    float tTierMinArid = BIOME_ARIDS[tTierIdx][0];
                    float tTierMaxArid = BIOME_ARIDS[tTierIdx][BIOME_ARIDS[tTierIdx].Length - 1];
                    if (a <= tTierMinArid)
                    { //in minimum aridity tier for this temperature tier 
                        float weight = tempFrac;
                        //float biomeEnumValue = (float)CLIMATE_TO_BIOMES_DICT[new int2(tTierIdx, 0)];
                        float biomeEnumValue = (float)BIOME_ARR[tTierIdx][0];
                        biomes[tTierHotColdIdx * 2] = new float2(weight, biomeEnumValue);
                    }
                    else if (a >= tTierMaxArid)
                    { //in maximum aridity tier for this temperature tier 
                        float weight = tempFrac;
                        //float biomeEnumValue = (float)CLIMATE_TO_BIOMES_DICT[new int2(tTierIdx, BIOME_ARIDS[tTierIdx].Length - 1)];
                        float biomeEnumValue = (float)BIOME_ARR[tTierIdx][BIOME_ARIDS[tTierIdx].Length - 1];
                        biomes[tTierHotColdIdx * 2] = new float2(weight, biomeEnumValue);
                    }
                    else
                    {
                        for (int aTier = 1; aTier < BIOME_ARIDS[tTierIdx].Length; aTier++)
                        {  //find aridity position in this temperature tier
                            if (a < BIOME_ARIDS[tTierCold][aTier])
                            {
                                float aridFrac = (a - BIOME_ARIDS[tTierIdx][aTier - 1]) / (BIOME_ARIDS[tTierCold][aTier] - BIOME_ARIDS[tTierCold][aTier - 1]);
                                float weightAridBiome = tempFrac * aridFrac;
                                float aridBiomeEnumValue = (float)BIOME_ARR[tTierIdx][aTier];
                                biomes[tTierHotColdIdx * 2] = new float2(weightAridBiome, aridBiomeEnumValue);

                                float moistFrac = 1 - aridFrac;
                                float weightMoistBiome = tempFrac * moistFrac;
                                float moistBiomeEnumValue = (float)BIOME_ARR[tTierIdx][aTier - 1];
                                biomes[tTierHotColdIdx * 2 + 1] = new float2(weightMoistBiome, moistBiomeEnumValue);
                                break;
                            }
                        }
                    }
                }
            }

            return biomes;
        }
        #endregion

    }
}