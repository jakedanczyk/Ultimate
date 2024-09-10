using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class StatInterface
    {
        public StatCollection statCollection;

        public StatInterface()
        {
            statCollection = new StatCollection();
        }
        public StatInterface(StatCollection istatCollection)
        {
            statCollection = istatCollection;
        }

    }
}
