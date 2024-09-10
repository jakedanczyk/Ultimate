using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string SAFETY_0 = "In Urth, player-characters experience many dangerous situations." +
        "A major goal in developing Urth was to 'bring to life' the trials, labors, and dangers our ancestor faced" +
        "while they roamed the ancient Earth. However, Urth is far from a perfect emulation of reality, and despite our efforts, it fails" +
        "to model many dangers that exist in the real world. Never assume URTH is an accurate model of anything in reality. ";
    string SAFETY_1 = "The terrain of URTH is far simpler, and far more predictable than real EARTH terrain. " +
        "Real On Earth, cliffs are often covered in weak, crumbling stone. " +
        "Caves can be highly unstable, waiting only for one bump, or even a shout, to unleash tons of rock" +
        "Making fires in caves (or even underneath rock overhangs) can be deadly, toxic gases can accumulate, the heat can cause the rock to expand and crack" +
        "On frozen bodies of water, natural ice can have weakspots";
    string SAFETY_2 = "In particular, the danger of unstable ground is far greater in reality."+
        "Digging large holes and artificial caves is extremely dangerous." +
        "and should only be done by professionals using proper technique and safety equipment.";
    string SAFETY_3 = "Things you should never do in real life that you might do in URTH";
    List<string> SAFETY_LIST = new List<string>()
    {
        "Never start a fire in an enclosed space without ventilation. Oxygen will be consumed and toxic gases will accumulate. You could fall unconscious before you even realized you were in danger.",
        "Never build a fire close to a cave wall, cave ceiling, or a cliff wall. Heat could crack stone and cause a collapse",
        "Never approach the edge of a natural cliff unless you know the cliff is stable and sturdy.",
        "Avoid banks and steep slopes of sediments such as clay or gravel. These can collapse.",
    };
}

