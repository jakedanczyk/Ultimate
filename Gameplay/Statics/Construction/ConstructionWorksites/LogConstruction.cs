using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class LogConstruction : ConstructionWorksite
    {
        public List<(float, float, float)> logs;//(diameter, length, quantity)
        public List<(float, float, float)> modelScales;//(x,y,z) transform scales

        public new float PrimaryVolume()
        {
            float vol = 0f;
            foreach((float,float,float) logSet in logs)
            {
                float rad = logSet.Item1 * 0.5f;
                float csa = rad * rad * Mathf.PI;
                vol += (csa * logSet.Item2 * logSet.Item3);
            }
            primaryVolume = vol;
            return vol;
        }

        public override void UpdateSuppliesNeeded()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateComponents()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateComponentsFinal()
        {
            throw new System.NotImplementedException();
        }

        public int supplyCount;

        public override USTATIC type => throw new System.NotImplementedException();
    }
}