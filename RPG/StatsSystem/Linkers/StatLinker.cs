using UnityEngine;
using System;
using System.Collections;

namespace Urth
{

    /// <summary>
    /// The base class used by all stat linkers
    /// </summary>
    public abstract class StatLinker : IStatValueChange
    {
        /// <summary>
        /// Triggers when the Value of the linker changes
        /// </summary>
        public event EventHandler OnValueChange;

        /// <summary>
        /// The Stat linked to by the stat linker
        /// </summary>
        public Stat LinkedStat { get; private set; }

        /// <summary>
        /// Gets the value of the stat linker
        /// </summary>
        public abstract int Value { get; }

        /// <summary>
        /// Basic Constructor. Listens to the Stat's OnValueChange
        /// event if the stat implements IStatValueChange.
        /// </summary>
        public StatLinker(Stat stat)
        {
            LinkedStat = stat;

            IStatValueChange iValueChange = LinkedStat as IStatValueChange;
            if (iValueChange != null)
            {
                iValueChange.OnValueChange += OnLinkedStatValueChange;
            }
        }

        /// <summary>
        /// Listens to the LinkedStat's OnValueChange event if able to
        /// </summary>
        private void OnLinkedStatValueChange(object stat, EventArgs args)
        {
            if (OnValueChange != null)
            {
                OnValueChange(this, null);
            }
        }
    }
}