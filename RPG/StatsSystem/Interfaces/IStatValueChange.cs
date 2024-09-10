using UnityEngine;
using System.Collections;
using System;

namespace Urth
{

    /// <summary>
    /// Used to indicate when the stat's value changes
    /// </summary>
    public interface IStatValueChange
    {
        event EventHandler OnValueChange;
    }
}