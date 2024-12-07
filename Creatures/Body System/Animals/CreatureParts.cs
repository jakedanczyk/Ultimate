using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public abstract class CreatureParts : ICreatureParts
    {
        public CreatureBody body;
        public HashSet<BODY_PART> partTypes;
        public Dictionary<BodyPartId, BodyPart> dict;
        public List<BodyPart> parts;

        public List<BodyPart> GetParts()
        {
            return parts;
        }
    }
}
