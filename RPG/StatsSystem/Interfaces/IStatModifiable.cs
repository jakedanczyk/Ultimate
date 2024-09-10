using UnityEngine;
using System.Collections;

namespace Urth
{

    /// <summary>
    /// Allows the stat to use modifiers
    /// </summary>
    public interface IStatModifiable
    {
        int StatModifierValue { get; }

        void AddModifier(StatModifier mod);
        void RemoveModifier(StatModifier mod);
        void ClearModifiers();
        void UpdateModifiers();
    }
}