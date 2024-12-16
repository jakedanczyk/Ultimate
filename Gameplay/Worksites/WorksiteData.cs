using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public enum WORKTASK
    {
        MINE,
        DIG,//gathering or moving loosened terrain
        STONECUTTING,
        LEVEL,

        FELL,
        DELIMB,
        DEBARK,
        CHOP, //cutting trunk into shorter logs
        SPLIT,//splitting logs lengthwise
        WOODWORK,

        GATHER,
        CLEAR,
        UPROOT,

        CONSTRUCT,

    }
    public enum WORKSITE_TYPE
    {
        TERRAIN,
        STUMP,
        TREE,
        BUSH,
        CONSTRUCTION,
        CRAFT,
        FARM,
    }

    /*Represents a voxel of terrain
     * Has 1 to 8 material types
     * Each material can be present in several forms:
     *  solid, stone, cobble, pebble, gravel, sand, dust
     * There is a 'loosened' fraction. It tracks how much is available for gathering
     * This can be increased by working tools
     * A shovel is able to "loosen" and "gather" from firm soils (but not solid stone) in a single action
     * 
     * skills: mining, digging, masonry
     */
    public class TerrainWorksiteData
    {
        public int id;
        public bool prospected = false;
        public bool surface;
        public int3 pos;
        public TerrainBlock terrainBlock;
        public TerrainWorksiteData(int iid, bool isSurface, int3 ipos)
        {
            id = iid;
            surface = isSurface;
            pos = ipos;
        }
    }
    public class PlantWorksiteData
    {
        public int id;
        public PlantWorksiteData(int iid)
        {
            id = iid;
        }
    }

    //public class WorksiteData
    //{
    //    public int id;
    //    public WORKSITE type;
    //    public TerrainWorksiteData terrainWorksiteData;
    //    public PlantWorksiteData plantWorksiteData;
    //    public WorksiteData(int iid, WORKSITE itype)
    //    {
    //        id = iid;
    //        type = itype;
    //        switch (itype)
    //        {
    //            case WORKSITE.TERRAIN:
    //                terrainWorksiteData = new TerrainWorksiteData();
    //                break;
    //            case WORKSITE.TREE:
    //                plantWorksiteData = new PlantWorksiteData();
    //                break;
    //        }
    //    }
    //}

}