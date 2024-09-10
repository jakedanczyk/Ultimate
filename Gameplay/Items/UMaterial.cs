using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UMATERIAL
{
    WATER,
    ICE,
    SNOW,

    SILVER,
    GOLD,
    COPPER,
    TIN,
    BRONZE,
    SPONGE_IRON,
    CAST_IRON,
    IRON,
    STEEL,
    PEWTER,

    ADAMANT,//hardest metal, 2x stronger than steel
    MARAGE,//almost as hard as adamant, with greater toughness but also greater density
    MITHRIL,//1.5x stronger than steel, specific density only 1.5, 

    NATIVE_ADAMANT,
    NATIVE_COPPER,
    NATIVE_GOLD,
    NATIVE_MARAGE,
    NATIVE_MITHRIL,
    NATIVE_SILVER,
    NATIVE_PLATINUM,


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

    AGATE,
    AMETHYST,
    BERYL,
    CARNELIAN,
    CHALCEDONY,
    DIAMOND,
    LAPIS_LAZULI,
    ONYX,//black and white banded chalcedony
    OPAL,
    PERIDOT,//olivine
    PRASE,//dark green chalcedony
    RUBY,
    SAPPHIRE,
    SARDONYX,//red banded chaldecony
    TOPAZ,//yellow
    TOPAZ_BLUE,
    TOPAZ_RED,
    ZIRCON,//clear
    ZIRCON_BLACK,
    ZIRCON_BLUE,
    ZIRCON_BROWN,
    ZIRCON_GREEN,
    ZIRCON_RED,
    ZIRCON_YELLOW,

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


    CLAY,
    GRAVEL_BASALT,
    GRAVEL_GRANITE,
    GRAVEL_GYPSUM,
    GRAVEL_LIMESTONE,
    GRAVEL_MAGNETITE,
    GRAVEL_QUARTZ,
    GRAVEL_RIVER,
    MUD,
    PEAT,
    SAND_BASALT,//black
    SAND_GRANITE,//grey
    SAND_GYPSUM,//white
    SAND_HEMATITE,//red
    SAND_LIMESTONE,//white
    SAND_MAGNETITE,//black
    SAND_QUARTZ,
    SUBSOIL,
    SUBSTRATE,
    TOPSOIL,
    TOPSOIL_CLAY,
    TOPSOIL_CLAY_LOAM,
    TOPSOIL_LOAMY_SAND,
    TOPSOIL_LOAM,
    TOPSOIL_SANDY_LOAM,
    TOPSOIL_SANDY_CLAY,
    TOPSOIL_SANDY_CLAY_LOAM,
    TOPSOIL_SILT,
    TOPSOIL_SILT_LOAM,
    TOPSOIL_SILTY_CLAY,
    TOPSOIL_SILTY_CLAY_LOAM,
    SUBSOIL_LOAMY_SAND,
    SUBSOIL_LOAM,
    SUBSOIL_CLAY,
    SUBSOIL_CLAY_LOAM,
    SUBSOIL_SANDY_LOAM,
    SUBSOIL_SANDY_CLAY,
    SUBSOIL_SANDY_CLAY_LOAM,
    SUBSOIL_SILT,
    SUBSOIL_SILT_LOAM,
    SUBSOIL_SILTY_CLAY,
    SUBSOIL_SILTY_CLAY_LOAM,
    TURF_CLOVER,
    TURF_GRASS,
    TURF_MOSS,

    LINEN,
    HEMP,
    COTTON,
    JUTE,
    WOOL,
    SILK,

    //woods
    MOUNTAIN_JUNIPER,
    COMMON_JUNIPER,
    CREEPING_JUNIPER,

    RED_CEDAR,

    LARCH,
    ALPINE_LARCH,
    TAMARACK,

    FIR,
    GRAND_FIR,
    DOUGLAS_FIR,

    MOUNTAIN_SPRUCE,
    WHITE_SPRUCE,

    BRISTLECONE_PINE,
    LIMBER_PINE,
    LODGEPOLE_PINE,
    PONDEROSA_PINE,
    WHITE_PINE, //western white pine
    WHITEBARK_PINE,

    HEMLOCK,
    MOUNTAIN_HEMLOCK,

    YEW,
    MOUNTAIN_YEW,

    PAPER_BIRCH,

    ASPEN,
    COTTONWOOD,
    POPLAR,
    BALSAM_POPLAR,

    ELM,
    BEECH,
    OAK,

    MAHOGANY,

    SAGE,
    SAGEBRUSH,

    SERVICEBERRY,
    HUCKLEBERRY,
    ELDERBERRY,
    THORN,

    PALM,

}
public enum MATERIAL_CLASS
{
    SKIN,
    FAT,
    MUSCLE,
    LIGAMENT,
    BONE,
    ENAMEL,

    FOLIAGE,
    FIBER,
    WOOD,

    LOOSE_SEDIMENT, //sand, topsoil
    COMPACT_SEDIMENT, //dried mud, hard clay soil
    STONE,
    METAL,

}

[System.Serializable]
public struct UMaterial
{//this struct is setup for display in Inspector
    public string typeName;
    public UMATERIAL type;
    public float density;
    public float toughness;
    public float hardness;
    public float yieldStrength;
    public float ultimateStrength;

    public Texture texture;
    public Texture weatheredTexture;
    public Texture rottedTexture;
}

//public class UMaterial
//{
//    public UMATERIAL type;
//    public float density;
//    public float toughness;
//    public float hardness;
//    public float yieldStrength;
//    public float ultimateStrength;

//    public Texture texture;
//    public Texture weatheredTexture;
//    public Texture rottedTexture;
//}
