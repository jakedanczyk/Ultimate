﻿using UnityEngine;
using System.Collections;

namespace Urth
{
    /// <summary>
    /// Modifier that takes a percentage of the stat's value
    /// </summary>
    [System.Serializable]
    public class StatModBasePercent : StatModifier
    {
        /// <summary>
        /// The order in which the modifier is applied to the stat
        /// </summary>
        public override int Order
        {
            get { return 1; }
        }

        /// <summary>
        /// Calculates the amount to apply to the stat based off the 
        /// sum of all the stat modifier's value and the current value of
        /// the stat.
        /// </summary>
        public override int ApplyModifier(int statValue, float modValue)
        {
            return (int)(statValue * modValue);
        }

        /// <summary>
        /// Constructor that sets the Value and sets Stacks to true
        /// </summary>
        public StatModBasePercent(float value) : base(value) { }

        /// <summary>
        /// Constructor that sets the Value and Stacks
        /// </summary>
        public StatModBasePercent(float value, bool stacks) : base(value, stacks) { }
    }
}
