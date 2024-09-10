using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public class GameBalance : MonoBehaviour
    {
        public static GameBalance Instance { get; private set; }
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

        public Dictionary<WORKTASK, List<(SKILL, float)>> worktaskSkillWeights = new Dictionary<WORKTASK, List<(SKILL, float)>>()
        {
            //{ WORKTASK.STONEBREAKING, new List<(SKILL,float)>(){(SKILL.MINING,0.8f),(SKILL.MASONRY,0.2f)} },
            //(WORKTASK.STONEBREAKING, new List<(SKILL,float)>(){(SKILL.MINING,0.8f)},
        };

        //public float 
    }

}