using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Items that can be picked up, inventoried, and equipped
 */
public interface IUrthItem
{
    float Weight { get; set; }
    float Size { get; set; }
    float LongestDimension { get; set; }
}

