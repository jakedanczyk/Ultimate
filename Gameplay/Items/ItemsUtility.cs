using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    public static class ItemsUtility
    {
        public static float ItemQualityRatio(QUALITY q) 
        {
            switch (q)
            {
                case QUALITY.RUINED:
                    return 0f;
                case QUALITY.AWFUL:
                    return .33f;
                case QUALITY.SHODDY:
                    return 0.5f;
                case QUALITY.POOR:
                    return 0.8f;
                case QUALITY.PLAIN:
                    return 1f;
                case QUALITY.DECENT:
                    return 1.25f;
                case QUALITY.FINE:
                    return 1.5f;
                case QUALITY.SUPERIOR:
                    return 1.8f;
                case QUALITY.EXCELLENT:
                    return 2.0f;
                case QUALITY.MASTERFUL:
                    return 2.5f;
                case QUALITY.INCREDIBLE:
                    return 3.0f;
                case QUALITY.MYTHIC:
                    return 4.0f;
                default:
                    Debug.Log("missed switch entry");
                    return 0f;
            }
        }

        static string[] qualityStringArr = new string[]
        {
            "Ruined",
            "Awful",
            "Shoddy",
            "Poor",
            "Plain",
            "Decent",
            "Fine",
            "Superior",
            "Excellent",
            "Masterful",
            "Incredible",
            "Mythic"
        };

        public static string QualityString(QUALITY q)
        {
            return qualityStringArr[(int)q];
        }
    }

}