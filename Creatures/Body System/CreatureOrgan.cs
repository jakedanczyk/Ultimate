using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ORGAN
{
    EYE,
    NOSE,
    EAR,
    BRAIN,
    HEART,
    LUNGS,
    STOMACH,
    GUT,
    LIVER,
    PANCREAS,
    KIDNEYS,
    GALLBLADDER,
    BLADDER
}

/*A BodyPart have one or more organs
 * Organs provide functionality to creatures bodies
 */
public class CreatureOrgan
{
    public ORGAN type;
}
