using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Urth
{

    public enum TERRAIN_MATS_1
    {
        ADAKITE,
        ALABASTER,
        AMPHIBOLITE,
        ANDESITE,
        ANTHRACITE,
        ARGILLITE,
        ARKOSE,
        BASALT,
        BASANITE,
        BAUXITE,
        BLUESCHIST,
        BRECCIA,
        BRUCITE,
        CARBONATITE,
        CATACLASITE,
        CHALK,
        CHARCOAL,
        CHARNOCKITE,
        CHERT,
        CHROMITE,
        CLAY_BROWN,
        CLAY_GRAY,
        CLAY_RED,
        CLAY_TAN,
        CLAY_WHITE,
        CLAYSTONE,
        COAL,
        COBBLES_BLACK,
        COBBLES_BROWN,
        COBBLES_COLOR,
        COBBLES_GRAY,
        COBBLES_WHITE,
        CONGLOMERATE_CLAYSTONE,
        CONGLOMERATE_LIMESTONE,
        CONGLOMERATE_QUARTZITE,
        COQUINA,
        DACITE,
        DIORITE,
        DIOTOMITE,
        DOLOMITE,
        DOLORITE,
        DUNITE,
        ECLOGITE,
        EOLIANITE,
        FAULT_BRECCIA,
        FOIDITE,
        FOIDOLITE,
        GABBRO,
        GNEISS,
        GRANITE,
        GRANODIORITE,
        GRAPHITE,
        GRAVEL_BLACK,
        GRAVEL_BROWN,
        GRAVEL_COLORFUL,
        GRAVEL_GRAY,
        GRAVEL_WHITE,
        GRAYWACKE,
        GREENSCHIST,
        GYPSUM,
        HARDPAN_BLACK,
        HARDPAN_BROWN,
        HARDPAN_GRAY,
        HARDPAN_RED,
        HARDPAN_TAN,
        HARDPAN_WHITE,
        HARDPAN_YELLOW,
        HORNBLENDE,
        HORNFELS,
        HUMUS_BLACK,
        HUMUS_BROWN,
        HUMUS_GRAY,
        HUMUS_RED,
        HUMUS_TAN,
        ICE_BLUE,
        ICE_CLEAR,
        ICE_DIRTY,
        ICE_ERODED,
        ICE_WHITE,
        IGNIMBRITE,
        KAOLIN,
        KIMBERLITE,
        KOMATITTE,
        LAMPROPHYRE,
        LATERITE,
        LATITE,
        LAVA_COOLED,
        LAVA_CRUSTED,
        LAVA_ORANGE,
        LAVA_RED,
        LEUCOGRANITE,
        LIGNITE,
        LIMESTONE,
        MARBLE,
        MARL,
        MICA,
        MICROCLINE,
        MONZONITE,
        MUD_ALGAE,
        MUD_BLACK,
        MUD_BROWN,
        MUD_GRAY,
        MUD_RED,
        MUD_TAN,
        MUDSTONE,
        NEPHELINITE,
        OBSIDIAN,
        OIL_SHALE,
        OOZE_BLACK,
        OOZE_BROWN,
        OOZE_GRAY,
        OOZE_RED,
        ORTHOCLASE,
        PEAT_DARK,
        PEAT_LIGHT,
        PEBBLES_BLACK,
        PEBBLES_BROWN,
        PEBBLES_COLORFUL,
        PEBBLES_GRAY,
        PEBBLES_WHITE,
        PEGMATITE,
        PERICLASE,
        PERIDOTITE,
        PERMAFROST_DIRTY,
        PERMAFROST_ICY,
        PERMAFROST_PEATY,
        PHONOLITE,
        PHOSPHORITE,
        PHYLLITE,
        PORPHYRY,
        PUMICE,
        PYROXENITE,
        QUARTZ_MONZONITE,
        QUARTZITE,
        RED_GRANITE,
        RHYOLITE,
        SAND_BLACK,
        SAND_GRAY,
        SAND_RED,
        SAND_TAN,
        SAND_WHITE,
        SAND_YELLOW,
        SANDSTONE,
        SCHIST,
        SERPENTINITE,
        SHALE,
        SILTSTONE,
        SKARN,
        SLATE,
        SNOW_CORNED,
        SNOW_DENSE,
        SNOW_DIRTY,
        SNOW_HARD,
        SNOW_ICY,
        SNOW_POWDER,
        SNOW_THIN_COBBLES,
        SNOW_THIN_GRAVEL,
        SNOW_THIN_LITTER,
        SNOW_THIN_PEBBLES,
        SOAPSTONE,
        STONE_DECOMPOSED_BLACK,
        STONE_DECOMPOSED_BROWN,
        STONE_DECOMPOSED_GRAY,
        STONE_DECOMPOSED_GREEN,
        STONE_DECOMPOSED_RED,
        STONE_DECOMPOSED_TAN,
        STONE_DECOMPOSED_WHITE,
        STONE_DECOMPOSED_YELLOW,
        STONE_FRACTURED_BLACK,
        STONE_FRACTURED_BROWN,
        STONE_FRACTURED_GRAY,
        STONE_FRACTURED_GREEN,
        STONE_FRACTURED_RED,
        STONE_FRACTURED_TAN,
        STONE_FRACTURED_WHITE,
        STONE_FRACTURED_YELLOW,
        STONE_LICHEN_BLACK,
        STONE_LICHEN_BROWN,
        STONE_LICHEN_GRAY,
        STONE_LICHEN_GREEN,
        STONE_LICHEN_RED,
        STONE_LICHEN_TAN,
        STONE_LICHEN_WHITE,
        STONE_LICHEN_YELLOW,
        STONE_SAPROLITE_BLACK,
        STONE_SAPROLITE_BROWN,
        STONE_SAPROLITE_GRAY,
        STONE_SAPROLITE_GREEN,
        STONE_SAPROLITE_RED,
        STONE_SAPROLITE_TAN,
        STONE_SAPROLITE_WHITE,
        STONE_SAPROLITE_YELLOW,
        STONE_WEATHERED_BLACK,
        STONE_WEATHERED_BROWN,
        STONE_WEATHERED_GRAY,
        STONE_WEATHERED_GREEN,
        STONE_WEATHERED_RED,
        STONE_WEATHERED_TAN,
        STONE_WEATHERED_WHITE,
        STONE_WEATHERED_YELLOW,
        SUBSOIL_BLACK,
        SUBSOIL_BROWN,
        SUBSOIL_GRAY,
        SUBSOIL_RED,
        SUBSOIL_TAN,
        SUBSOIL_WHITE,
        SUBSOIL_YELLOW,
        SUBSTRATE_BLACK,
        SUBSTRATE_BROWN,
        SUBSTRATE_GRAY,
        SUBSTRATE_RED,
        SUBSTRATE_ROCKY_BLACK,
        SUBSTRATE_ROCKY_BROWN,
        SUBSTRATE_ROCKY_GRAY,
        SUBSTRATE_ROCKY_RED,
        SUBSTRATE_ROCKY_TAN,
        SUBSTRATE_ROCKY_WHITE,
        SUBSTRATE_ROCKY_YELLOW,
        SUBSTRATE_TAN,
        SUBSTRATE_WHITE,
        SUBSTRATE_YELLOW,
        SYENITE,
        TALC,
        TALUS_BLACK,
        TALUS_BROWN,
        TALUS_GRAY,
        TALUS_GREEN,
        TALUS_RED,
        TALUS_TAN,
        TALUS_WHITE,
        TALUS_YELLOW,
        TEPHRITE,
        TILL_BLACK,
        TILL_BROWN,
        TILL_GRAY,
        TILL_RED,
        TILL_TAN,
        TILL_WHITE,
        TILL_YELLOW,
        TONALITE,
        TOPSOIL_BLACK,
        TOPSOIL_BROWN,
        TOPSOIL_GRAY,
        TOPSOIL_RED,
        TOPSOIL_TAN,
        TRACHYTE,
        TRAVERTINE,
        TUFA,
        TUFF,
        VOLCANIC_ASH_BLACK,
        VOLCANIC_ASH_GRAY,
        WACKESTONE,
        WHITESCHIST,
        WOOD_ASH,
    }



    [System.Serializable]
        public class MaterialsLibrary : MonoBehaviour
        {
            public static MaterialsLibrary Instance { get; private set; }
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
            }
            // Start is called before the first frame update
            void Start()
            {
                matsDict = new Dictionary<UMATERIAL, UMaterial>(matsList.Count);
                for(int i = 0; i < matsList.Count; i++) 
                {
                    Debug.Log(matsList[i].typeName);
                    UMaterial mat = matsList[i];
                    if (System.Enum.TryParse(mat.typeName, out UMATERIAL mtype))
                    {
                        Debug.Log(mtype);
                        mat.type = mtype;
                        matsList[i] = mat;
                        matsDict[mtype] = mat;
                    }
                }
            }

            // Update is called once per frame
            void Update()
            {

            }

            public List<UMaterial> matsList;
            public Dictionary<UMATERIAL, UMaterial> matsDict;

            public Dictionary<string, UMATERIAL> voxelMats = new Dictionary<string, UMATERIAL>
            {
                {"Soil", UMATERIAL.SUBSOIL },
                {"Bedrock", UMATERIAL.BASALT },
                {"Snow", UMATERIAL.SNOW },
                {"Granite", UMATERIAL.GRANITE},
                { "ice_blue", UMATERIAL.ICE },
                { "ice_clear", UMATERIAL.ICE },
                { "ice_eroded", UMATERIAL.ICE },
                { "ice_white", UMATERIAL.ICE },
                { "snow_fresh", UMATERIAL.SNOW },
                { "snow_ice", UMATERIAL.SNOW },
                { "snow_packed", UMATERIAL.SNOW },
                { "snow_smooth", UMATERIAL.SNOW },
                { "snow_windblown", UMATERIAL.SNOW },

                { "snow_cobbles", UMATERIAL.SNOW },
                { "snow_dirt", UMATERIAL.SNOW },
                { "snow_litter", UMATERIAL.SNOW },
                { "snow_grass", UMATERIAL.SNOW },
                { "snow_moss", UMATERIAL.SNOW },
                { "snow_gravel", UMATERIAL.SNOW },
                { "snow_rocks", UMATERIAL.SNOW },

                { "clay_brown", UMATERIAL.CLAY},
                { "clay_grey", UMATERIAL.CLAY},
                { "clay_ochre", UMATERIAL.CLAY},
                { "clay_red", UMATERIAL.CLAY},
                { "clay_white", UMATERIAL.CLAY},

                { "gravel_black", UMATERIAL.GRAVEL_BASALT },
                { "gravel_blue", UMATERIAL.GRAVEL_BASALT },
                { "gravel_brown", UMATERIAL.GRAVEL_BASALT },
                { "gravel_gray", UMATERIAL.GRAVEL_BASALT },
                { "gravel_green", UMATERIAL.GRAVEL_BASALT },
                { "gravel_mixed_1", UMATERIAL.GRAVEL_BASALT },
                { "gravel_mixed_2", UMATERIAL.GRAVEL_BASALT },
                { "gravel_mixed_3", UMATERIAL.GRAVEL_BASALT },
                { "gravel_red", UMATERIAL.GRAVEL_BASALT },
                { "gravel_white", UMATERIAL.GRAVEL_BASALT },
                { "gravel_yellow", UMATERIAL.GRAVEL_LIMESTONE },

                { "sand_black", UMATERIAL.GRAVEL_BASALT },
                { "sand_gray", UMATERIAL.GRAVEL_BASALT },
                { "sand_green", UMATERIAL.GRAVEL_BASALT },
                { "sand_red", UMATERIAL.GRAVEL_BASALT },
                { "sand_tan", UMATERIAL.GRAVEL_BASALT },
                { "sand_white", UMATERIAL.GRAVEL_BASALT },
                { "sand_yellow", UMATERIAL.GRAVEL_LIMESTONE },

                { "cobbles_black", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_gray", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_green", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_mixed_1", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_mixed_2", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_mixed_3", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_red", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_tan", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_white", UMATERIAL.GRAVEL_BASALT },
                { "cobbles_yellow", UMATERIAL.GRAVEL_LIMESTONE },


                { "topsoil_clay", UMATERIAL.TOPSOIL_CLAY },
                { "topsoil_clay_loam", UMATERIAL.TOPSOIL_CLAY_LOAM},
                { "topsoil_loamy_sand", UMATERIAL.TOPSOIL_LOAMY_SAND },
                { "topsoil_loam", UMATERIAL.TOPSOIL_LOAM },
                { "topsoil_sandy_loam", UMATERIAL.TOPSOIL_SANDY_LOAM},
                { "topsoil_sandy_clay", UMATERIAL.TOPSOIL_SANDY_CLAY},
                { "topsoil_sandy_clay_loam", UMATERIAL.TOPSOIL_SANDY_CLAY_LOAM},
                { "topsoil_silt", UMATERIAL.TOPSOIL_SILT},
                { "topsoil_silt_loam", UMATERIAL.TOPSOIL_SILT_LOAM},
                { "topsoil_silty_clay", UMATERIAL.TOPSOIL_SILTY_CLAY},
                { "topsoil_silty_clay_loam", UMATERIAL.TOPSOIL_SILTY_CLAY_LOAM},

                { "turf_clover", UMATERIAL.TURF_CLOVER },
                { "turf_clover_dead", UMATERIAL.TURF_CLOVER },
                { "turf_clover_dirt", UMATERIAL.TURF_CLOVER },
                { "turf_clover_litter", UMATERIAL.TURF_CLOVER },
                { "turf_clover_rocks", UMATERIAL.TURF_CLOVER },
                { "turf_clover_snow", UMATERIAL.TURF_CLOVER },
                { "turf_grass", UMATERIAL.TURF_GRASS },
                { "turf_grass_dead", UMATERIAL.TURF_GRASS },
                { "turf_grass_dirt", UMATERIAL.TURF_GRASS },
                { "turf_grass_large", UMATERIAL.TURF_GRASS },
                { "turf_grass_litter", UMATERIAL.TURF_GRASS },
                { "turf_grass_rocks", UMATERIAL.TURF_GRASS },
                { "turf_grass_snow", UMATERIAL.TURF_GRASS },
                { "turf_moss", UMATERIAL.TURF_MOSS },
                { "turf_moss_dirt", UMATERIAL.TURF_MOSS },
                { "turf_moss_litter", UMATERIAL.TURF_MOSS },
                { "turf_moss_rocks", UMATERIAL.TURF_MOSS },
                { "turf_moss_snow", UMATERIAL.TURF_MOSS },
                
                { "peat", UMATERIAL.TURF_MOSS },

                { "litter_dirt", UMATERIAL.TURF_MOSS },
                { "litter_lobed", UMATERIAL.TURF_MOSS },
                { "litter_oval", UMATERIAL.TURF_MOSS },
                { "litter_palmate", UMATERIAL.TURF_MOSS },
                { "litter_pine", UMATERIAL.TURF_MOSS },

                { "mud_black", UMATERIAL.TURF_MOSS },
                { "mud_brown", UMATERIAL.TURF_MOSS },
                { "mud_grass", UMATERIAL.TURF_MOSS },
                { "mud_grey", UMATERIAL.TURF_MOSS },
                { "mud_tan", UMATERIAL.TURF_MOSS },


                { "weathered_black", UMATERIAL.GRAVEL_BASALT },
                { "weathered_gray", UMATERIAL.GRAVEL_BASALT },
                { "weathered_green", UMATERIAL.GRAVEL_BASALT },
                { "weathered_red", UMATERIAL.GRAVEL_BASALT },
                { "weathered_tan", UMATERIAL.GRAVEL_BASALT },
                { "weathered_white", UMATERIAL.GRAVEL_BASALT },
                { "weathered_yellow", UMATERIAL.GRAVEL_LIMESTONE },

                { "cracked_black", UMATERIAL.GRAVEL_BASALT },
                { "cracked_gray", UMATERIAL.GRAVEL_BASALT },
                { "cracked_green", UMATERIAL.GRAVEL_BASALT },
                { "cracked_red", UMATERIAL.GRAVEL_BASALT },
                { "cracked_tan", UMATERIAL.GRAVEL_BASALT },
                { "cracked_white", UMATERIAL.GRAVEL_BASALT },
                { "cracked_yellow", UMATERIAL.GRAVEL_LIMESTONE },

                { "bricks_red", UMATERIAL.GRAVEL_LIMESTONE },
                { "bricks_pink", UMATERIAL.GRAVEL_LIMESTONE },
                { "bricks_yellow", UMATERIAL.GRAVEL_LIMESTONE },
                { "bricks_white", UMATERIAL.GRAVEL_LIMESTONE },
                { "bricks_ruins", UMATERIAL.GRAVEL_LIMESTONE },

                { "pavement_dry_black", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_dry_brown", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_dry_grey", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_dry_red", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_dry_white", UMATERIAL.GRAVEL_LIMESTONE },

                { "pavement_rough_black", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_rough_brown", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_rough_grey", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_rough_red", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_rough_white", UMATERIAL.GRAVEL_LIMESTONE },

                { "pavement_tight_black", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_tight_brown", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_tight_grey", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_tight_red", UMATERIAL.GRAVEL_LIMESTONE },
                { "pavement_tight_white", UMATERIAL.GRAVEL_LIMESTONE },

                { "adobe", UMATERIAL.GRAVEL_LIMESTONE },
                { "mudbrick", UMATERIAL.GRAVEL_LIMESTONE },

                { "native_adamant", UMATERIAL.GRAVEL_LIMESTONE },
                { "native_gold", UMATERIAL.GRAVEL_LIMESTONE },
                { "native_maar", UMATERIAL.GRAVEL_LIMESTONE },
                { "native_mithril", UMATERIAL.GRAVEL_LIMESTONE },
                { "native_silver", UMATERIAL.GRAVEL_LIMESTONE },
                { "native_platinum", UMATERIAL.GRAVEL_LIMESTONE },

                /*
                ACANTHITE,//silver sulfide
                AZURITE,//bright blue copper ore, copper carbonate
                BANDED_IRON,
                BISMUTHINITE,
                CASSITERITE,//tin
                CHALCOCITE,//copper sulfide
                CHALCHOPYRITE,//copper-iron-sulfide
                COBALTITE,//
                CUPRITE,//copper(i) oxide, dark red
                HEMATITE,//iron(III) oxide, feeble magnetism
                HORN_SILVER,//silver chloride
                GALENA, //lead sulfide
                GARNIERITE,//nickel ore
                GOETHITE,//iron(iii) oxide-hydroxide
                LIMONITE,//hydrated iron oxide-hydroxides
                MAGNETITE,//iron(II,III) oxide, magnetic
                MALACHITE,//copper carbonate hydroxide
                OLIVENITE,//copper-arsenic 
                PYRITE,//iron-sulfide. poor iron ore due to difficult chemistry, but ancient source of iron-sulfate, for iron-gall ink
                SPHALERITE,//zinc sulfide
                TENNANTITE, //copper-arsenic-sulfosalt, with fractions or iron, zinc, and silver
                TETRAHEDRITE, //copper-antimony-sulfosalt, with fractions or iron, zinc, and silver
                WOLFRAMITE,//tunsgen ore

                ARSENOPYRITE,//FeAsS. arsenic ore for arsenic-bronze. could be iron ore but toxic if arsenic is released
                CINNABAR,//mercury sulfide
                HALITE,//mineral sodium chloride
                NITER,//potassium nitrate
                REALGAR,//arsenic sulfide
                SULFUR,
                SYLVINITE,//mixed halite and sylvite
                SYLVITE,//mineral potassium chloride

                ANTHRACITE,
                BITUMINOUS_COAL,
                LIGNITE,

                ANDESITE,
                ARGILLITE,
                ARKOSE,
                BASALT,
                BAUXITE,
                BRECCIA,
                BRUCITE,
                CHALK,
                CHERT,
                CHROMITE,
                CONGLOMERATE_CALCITE,
                CONGLOMERATE_QUARTZ,
                CONGLOMERATE_HEMATITE,
                CLAYSTONE,
                DACITE,
                DIORITE,
                DOLOMITE,
                GABBRO,
                GNEISS,
                GRANITE,
                GRANODIORITE,
                GREYWACKE,
                GYPSUM,
                KAOLIN,
                KIMBERLITE,
                KOMATITTE,
                LIMESTONE,
                MARBLE,
                MICA,
                MICROCLINE,
                MUDSTONE,
                OBSIDIAN,
                ORTHOCLASE,
                PERICLASE,
                PERIDOTITE,
                PHYLLITE,
                PUMICE,
                QUARTZITE,
                RHYOLITE,
                SANDSTONE,
                SERPENTINITE,
                SCHIST,
                SHALE,
                SILTSTONE,
                SLATE,
                SOAPSTONE,
                TALC,
                TUFF,
                */


                //50 so far
                //another 10 or so mixed soil types. soil/gravel, soil/rock, gravel/snow, rock/snow,

                //20 or so metals
                //20 or so ores
                //10 chemical deposits
                //20 gemstones
                //50 stones
                //10 wood variants
                //190 to here

                //looks like we may have space for cracked and lined surfaces
                //cracked rock
                //masoned rock
                //bricks

                { "basalt", UMATERIAL.BASALT },
                { "chert", UMATERIAL.CHERT },
                { "claystone", UMATERIAL.CLAYSTONE },
                { "granite_blackwhite", UMATERIAL.GRANITE },
                { "limestone", UMATERIAL.LIMESTONE },
                { "sandstone", UMATERIAL.SANDSTONE },
                { "shale", UMATERIAL.SHALE},
                { "slate", UMATERIAL.SLATE},
            };

            public TERRAIN_MATS_1 RandomCommonSedimentaryStone()
            {
                return TERRAIN_MATS_1.SILTSTONE;
            }

        }

}