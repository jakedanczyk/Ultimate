using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public enum WORKTASK
    {
        MINE,//break up and pick up raw terrain that is too firm to be dug(TODO be able to drop what is dug on designated tile within reach, but also specify materials to keep)
        DIG,//for soft terrain. limit determined by tool and ability
        STONECUT,//cut stone into shapes (also gives mining xp if done on raw terrain)
        COLLECT_STONE,//collect stone pieces, specify material/shape/size in WorkUI
        SEARCH_STONE,//'look around' for stones of a particular type
        LEVEL,//flatten a chunk of terrain
        SCAN_TERRAIN,

        HARVEST_PLANT,//Gather fruit or other primary product of certain plants.
                      //May destroy plant entirely (i.e. potato) or partialy (lettuce) or not at all (berry bushes, fruit tree)

        FELL,//a tree or other similar object (saguaro, giant fungi)
        CHOP,
        PROCESS_TREE,//process a downed tree into logs, sticks, bark, etc.
                     //(use this instead of step-by-step process for now. need more art time/assets first).
        DELIMB,//downed trees //if tree is still standing, height limits % of limbs can be cut. no wait, requires modular tree models.)
        DEBARK,//remove bark from limbed tree trunk
        CUT_LOGS, //cutting trunk into shorter logs

        SPLIT,//split logs into quarter logs
        MAKE_KINDLING,//make quarters,branches,etc. into kindling
        WOODWORK,

        GATHER,
        CLEAR,
        UPROOT,

        CONSTRUCT,

        FISH

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
        public TerrainBlock miningOutput;
        public TerrainWorksiteData(int iid, bool isSurface, int3 ipos)
        {
            id = iid;
            surface = isSurface;
            pos = ipos;
        }
        public List<TerrainBlockFraction> GetFractions()
        {
            return terrainBlock.fractions;
        }
        public void SetTerrainBlock(TerrainBlock itb)
        {
            terrainBlock = itb;
            miningOutput = itb;
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