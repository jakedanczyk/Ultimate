using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public static partial class UrthUtility
    {
        public static List<int2> GetAdjTiles(int steps = 1, int origX = 0, int origY = 0)
        {
            List<int2> adjacents = new List<int2> { };
            for (int x = -steps; x <= steps; x++)
            {
                for (int y = -steps; y <= steps; y++)
                {
                    if (x == 0 && y == 0) { continue; }
                    adjacents.Add(new int2(origX + x, origY + y));
                }
            }
            return adjacents;
        }
        public static List<int2> GetAdjTiles(int steps, int2 orig)
        {
            return GetAdjTiles(steps, orig.x, orig.y);
        }

        static int TILE_LENGTH_PX = 1000;
        public static int2 TileIndexToPxCoord(int i)
        {
            int xPx = 1 + (i % TILE_LENGTH_PX);
            int yPx = 1 + (i / TILE_LENGTH_PX);
            return new int2(xPx, yPx);
        }

        public static List<int2> GetAreaTiles(int steps, int2 orig)
        {
            List<int2> tiles = new List<int2>() { orig };
            List<int2> adjacents = GetAdjTiles(steps, orig);
            tiles.AddRange(adjacents);
            return tiles;
        }

        public static double NormalRandom(double mean, double sdev)
        {
            System.Random r = new System.Random();
            double u1 = r.NextDouble();
            double u2 = r.NextDouble();
            double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);
            double randNormal = mean + sdev * randStdNormal;
            return randNormal;
        }
        public static float NormalRandom(float mean, float sdev)
        {
            return (float)NormalRandom((double)mean, (double)sdev);
        }

        public static Vector3 Float3ToVector3(float3 f)
        {
            return new Vector3(f.x, f.y, f.z);
        }

        public static List<(int, UItemContainer)> SortContainersBySize(List<(int id, UItemContainer container)> list, bool ascending = true)
        {
            List<(int, UItemContainer)> sorteds = new List<(int, UItemContainer)>(list.Count);
            List<(int, float, UItemContainer)> unsorteds = new List<(int, float, UItemContainer)>(list.Count);
            foreach ((int id, UItemContainer container) in list)
            {
                unsorteds.Add((id, container.totalSpace, container));
            }
            List<(int, float, UItemContainer)> sortedVals = ascending ? unsorteds.OrderBy(s => s.Item2).ToList() : unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((int id, float val, UItemContainer item) in sortedVals)
            {
                sorteds.Add((id, item));
            }
            return sorteds;
        }
        public static List<(int, UItemData)> SortItemsByFloat(ITEM_PROPERTY sortProp, List<(int id, UItemData item)> list, bool ascending = true)
        {
            List<(int, UItemData)> sorteds = new List<(int, UItemData)>(list.Count);
            List<(int, float, UItemData)> unsorteds = new List<(int, float, UItemData)>(list.Count);
            foreach ((int id, UItemData item) in list)
            {
                switch (sortProp)
                {
                    case ITEM_PROPERTY.WEIGHT:
                        unsorteds.Add((id, item.Weight, item));
                        break;
                    case ITEM_PROPERTY.LENGTH:
                        unsorteds.Add((id, item.Length, item));
                        break;
                    case ITEM_PROPERTY.VOLUME:
                        unsorteds.Add((id, item.volumeComponent, item));
                        break;
                }
            }
            List<(int, float, UItemData)> sortedVals = ascending ? unsorteds.OrderBy(s => s.Item2).ToList() : unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((int id, float val, UItemData item) in sortedVals)
            {
                sorteds.Add((id, item));
            }
            return sorteds;
        }
        public static List<(int, UItemData)> SortItemsByString(ITEM_PROPERTY sortProp, List<(int id, UItemData item)> list, bool ascending = true)
        {
            List<(int, UItemData)> sorteds = new List<(int, UItemData)>(list.Count);
            List<(int, string, UItemData)> unsorteds = new List<(int, string, UItemData)>(list.Count);
            foreach ((int id, UItemData item) in list)
            {
                switch (sortProp)
                {
                    case ITEM_PROPERTY.NAME:
                        unsorteds.Add((id, item.GetName(), item));
                        break;
                }
            }
            List<(int, string, UItemData)> sortedVals = ascending ? unsorteds.OrderBy(s => s.Item2).ToList() : unsorteds.OrderByDescending(s => s.Item2).ToList();
            foreach ((int id, string val, UItemData item) in sortedVals)
            {
                sorteds.Add((id, item));
            }
            return sorteds;
        }
    }

}