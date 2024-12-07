using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class AnimalParts : CreatureParts
    {

        public AnimalParts(CreatureBody ib, CreatureSpecies species)
        {
            body = ib;
            essentialParts = new List<BodyPartId>();
            parts = new List<BodyPart>();
            dict = new Dictionary<BodyPartId, BodyPart>(species.bodyplan.nodes.Count);
            foreach (BodyPlanNode node in species.bodyplan.nodes)
            {
                BodyPart bp = new BodyPart(node);
                dict.Add(node.bodyPartId, bp);
                parts.Add(bp);
                if (bp.essential)
                {
                    essentialParts.Add(node.bodyPartId);
                }
            }
        }
        public AnimalParts(CreatureBody ib, int bpcount)
        {
            body = ib;
            essentialParts = new List<BodyPartId>();
            parts = new List<BodyPart>();
            dict = new Dictionary<BodyPartId, BodyPart>(bpcount);
        }
        public AnimalParts(CreatureBody ib)
        {
            new AnimalParts(ib, 0);
        }

        public new CreatureBody body;
        public BodyPartId breathingPart = new BodyPartId(BODY_PART.HEAD, SYMMETRY.CENTER);
        public List<BodyPartId> essentialParts;

    }
}