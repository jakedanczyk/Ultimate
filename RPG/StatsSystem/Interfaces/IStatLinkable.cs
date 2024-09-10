using UnityEngine;
using System.Collections;

namespace Urth
{
    /// <summary>
    /// Allows the stat to use stat linkers
    /// </summary>
    public interface IStatLinkable
    {
        int StatLinkerValue { get; }

        void AddLinker(StatLinker linker);
        void RemoveLinker(StatLinker linker);
        void ClearLinkers();
        void UpdateLinkers();
    }
}
