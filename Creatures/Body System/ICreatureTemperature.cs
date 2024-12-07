using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public interface ICreatureTemperature
    {
        void UpdateTemperature(float timestep);
    }
}