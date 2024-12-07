
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{

    public enum SYMMETRY 
    { 
        CENTER,
        LEFT,
        RIGHT,
    }

    public enum BODY_PART
    {
        HEAD,
        NECK,
        SHOULDER,
        UPPER_ARM,
        FOREARM,
        HAND,
        CHEST,
        THORAX,
        ABDOMEN,
        PELVIS,
        THIGH,
        KNEE,
        CALF,
        ANKLE,
        FOOT,
        TAIL

    }

    public enum BODY_PART_HUMANOID
    {
        HEAD,
        NECK,
        SHOULDER,
        UPPER_ARM,
        FOREARM,
        HAND,
        CHEST,
        ABDOMEN,
        PELVIS,
        THIGH,
        KNEE,
        CALF,
        ANKLE,
        FOOT,
    }


    [System.Serializable]
    public class BodyPartId : IEquatable<BodyPartId>
    {
        public BODY_PART type;
        public SYMMETRY sym;

        public BodyPartId(BODY_PART it, SYMMETRY isym)
        {
            type = it; sym = isym;
        }

        public BodyPartId(BODY_PART it)
        {
            type = it; sym = SYMMETRY.CENTER;
        }

        public bool Equals(BodyPartId other)
        {
            if (other == null)
                return false;

            if (this.type == other.type && this.sym == other.sym)
                return true;
            else
                return false;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;

            BodyPartId bpidObj = obj as BodyPartId;
            if (bpidObj == null)
                return false;
            else
                return Equals(bpidObj);
        }

        public override int GetHashCode()
        {
            return this.type.GetHashCode() * sizeof(SYMMETRY) + this.sym.GetHashCode();
        }

        public static bool operator ==(BodyPartId bp1, BodyPartId bp2)
        {
            if (((object)bp1) == null || ((object)bp2) == null)
                return System.Object.Equals(bp1, bp2);

            return bp1.Equals(bp2);
        }

        public static bool operator !=(BodyPartId bp1, BodyPartId bp2)
        {
            if (((object)bp1) == null || ((object)bp2) == null)
                return !System.Object.Equals(bp1, bp2);

            return !(bp1.Equals(bp2));
        }

    }

    [System.Serializable]
    public class BodyPart
    {
        public int id;
        public int lr; //-1, 1, or 1 for left, right, or center
        public SYMMETRY symmetry;
        public BODY_PART type;
        public List<IntegralBodyPartLayer> bodyLayers;
        public List<EquippedBodyPartLayer> equipLayers;

        public List<IntegralBodyPartLayer> bleedingLayers;
        public bool isInjured = false;

        public float integrity;


        public int[] protection = new int[6]; // blunt cut pierce insulation water wind
        public virtual int[] ProtValue
        {
            get { return protection; }
        }

        public bool isSubmerged = false;
        public float wetness = 0;
        public float damageModifer = 1;
        public float size;



        public bool essential;
        public bool thermalCore;
        public float skinTemp;
        public float innerTemp;
        public float thermoRadius; //all bodyparts modeled as spheres for heat exchange? pipes are about equal complexity, but doing everything as same shape would be nice

        public float waterCover = 0f;

        public float Functionality()
        {
            return isInjured ? 0f : 1f;
        }

        public BodyPart(BODY_PART itype, SYMMETRY isym)
        {
            type = itype;
            symmetry = isym;
        }
        public BodyPart(BodyPlanNode bpNode)
        {
            type = bpNode.bodyPartId.type;
            symmetry = bpNode.bodyPartId.sym;
        }
    }
}
