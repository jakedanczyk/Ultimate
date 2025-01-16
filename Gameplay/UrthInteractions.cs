using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public static class UrthInteractions
    {

        /*Work performance depends on...
         * quality of the tool
         * characters skills and attributes for the task
         * characters skill with the tool
         * difficulty of the task
         */
        public static float GetItemScoreMine(UItemData itemData)
        {
            float itemTemplateScore = ItemsLibrary.Instance.templatesDict[itemData.type].worktaskScores[WORKTASK.MINE];
            float itemQualityScore = ItemsUtility.ItemQualityRatio(itemData.quality);
            float itemMaterialScore = 1f;//TODO
            return itemQualityScore * itemMaterialScore;
        }
        public static void SimpleMineTerrainBlock(CreatureManager creature, WIELD_SLOT slot, TerrainWorksite terrainWorksite)
        {
            TerrainBuilder.Instance.Dig(terrainWorksite.data.pos);
        }

        public static void MineTerrainBlock(CreatureManager creature, WIELD_SLOT slot, TerrainWorksite terrainWorksite)
        {
            float itemScore = 0;
            float toolSkillScore = 0;
            UItemData itemData = null;
            switch (slot)
            {
                case WIELD_SLOT.LEFT:
                    itemData = creature.GetLeftItem();
                    break;
                case WIELD_SLOT.RIGHT:
                    itemData = creature.GetRightItem();
                    break;
            }
            if (itemData != null)
            {
                itemScore = GetItemScoreMine(itemData);
                toolSkillScore = creature.body.stats.GetStatByName(itemData.type.ToString());
            }
            else
            {
                itemScore = 1f;
                toolSkillScore = 1f;
            }

            float taskSkillScore = creature.body.stats.Mining();
            float characterScore = taskSkillScore * toolSkillScore;

            float mineScoreTotal = itemScore * characterScore;
            float remaining = mineScoreTotal;
            while (remaining > 0)
            {
                List<TerrainBlockFraction> unfracturedFractions = terrainWorksite.GetUnfracturedFractions();
                if (unfracturedFractions.Count == 0)
                {
                    break;
                }
                float damageShare = remaining / unfracturedFractions.Count;
                remaining = 0f;
                foreach (TerrainBlockFraction fraction in unfracturedFractions)
                {
                    float fractionDamageFactor = 1f / fraction.volumeFraction / fraction.Resistance();
                    float damage = damageShare * fractionDamageFactor;
                    fraction.fracture = damage + fraction.fracture;
                    if (fraction.fracture > 1f)
                    {//fully fractured, could be remainder damage to carry over
                        float remainder = (fraction.fracture - 1f) / fractionDamageFactor;
                        remaining += remainder;
                        fraction.fracture = 1f;
                    }
                }
            }
        }
        public static void BreakTerrainBlock(CreatureManager creature, UItem tool, TerrainWorksite terrainWorksite)
        {
            float itemTemplateScore = ItemsLibrary.Instance.templatesDict[tool.data.type].worktaskScores[WORKTASK.MINE];
            float itemQualityScore = ItemsUtility.ItemQualityRatio(tool.data.quality);
            float itemMaterialScore = 1f;//TODO
            float itemScore = itemQualityScore * itemMaterialScore;

            float taskSkillScore = creature.body.stats.Mining();
            float toolSkillScore = creature.body.stats.GetStatByName(tool.data.type.ToString());
            float characterScore = taskSkillScore * toolSkillScore;

            float mineScoreTotal = itemScore * characterScore;
            float remaining = mineScoreTotal;
            while(remaining > 0)
            {
                List<TerrainBlockFraction> unfracturedFractions = terrainWorksite.GetUnfracturedFractions();
                if(unfracturedFractions.Count == 0)
                {
                    break;
                }
                float damageShare = remaining / unfracturedFractions.Count;
                remaining = 0f;
                foreach(TerrainBlockFraction fraction in unfracturedFractions)
                {
                    float fractionDamageFactor = 1f / fraction.volumeFraction / fraction.Resistance();
                    float damage = damageShare * fractionDamageFactor;
                    fraction.fracture = damage + fraction.fracture;
                    if(fraction.fracture > 1f)
                    {//fully fractured, could be remainder damage to carry over
                        float remainder = (fraction.fracture - 1f) / fractionDamageFactor;
                        remaining += remainder;
                        fraction.fracture = 1f;
                    }
                }
            }
        }
        //Digging only affects terrain layers soft enough to be dug.
        //The limit of terrain firmness is affected by stats of creature and tool
        public static void DigTerrainBlock(CreatureManager creature, WIELD_SLOT slot, TerrainWorksite terrainWorksite)
        {
            float itemScore = 0;
            float toolSkillScore = 0;
            UItemData itemData = null;
            switch (slot)
            {
                case WIELD_SLOT.LEFT:
                    itemData = creature.GetLeftItem();
                    break;
                case WIELD_SLOT.RIGHT:
                    itemData = creature.GetRightItem();
                    break;
            }
            if (itemData != null)
            {
                float itemTemplateScore = ItemsLibrary.Instance.templatesDict[itemData.type].worktaskScores[WORKTASK.DIG];
                float itemQualityScore = ItemsUtility.ItemQualityRatio(itemData.quality);
                float itemMaterialScore = 1f;//TODO
                itemScore = itemQualityScore * itemMaterialScore;
                toolSkillScore = creature.body.stats.GetStatByName(itemData.type.ToString());
            }
            else
            {
                itemScore = 1f;
                toolSkillScore = 1f;
            }

            float taskSkillScore = creature.body.stats.Digging();
            float characterScore = taskSkillScore * toolSkillScore;

            float digScoreTotal = itemScore * characterScore;
            float energyRemaining = digScoreTotal;
            float volumeRemaining = itemScore;
            if (terrainWorksite.data.terrainBlock.surfaceHorizons)
            {//layered. Dig highest layers first
                int idx = 0;
                while(volumeRemaining > 0)
                {
                    TerrainBlockFraction fraction = terrainWorksite.data.terrainBlock.fractions[idx];
                    if(fraction.Resistance() > digScoreTotal)
                    {
                        MessageLogControl.Instance.NewMessage("Terrain to tough to dig");
                        break;
                    }
                    float volumeDug = Mathf.Min(fraction.volumeFraction, itemScore);
                    volumeRemaining -= volumeDug;
                }
            }
            else
            {//non-layered, same logic as mining (proportional)
                while (volumeRemaining > 0)
                {
                    List<TerrainBlockFraction> fractions = terrainWorksite.GetExistentFractions();
                    if(fractions.Count == 0)
                    {
                        break;
                    }
                    float digShare = volumeRemaining / fractions.Count;
                    volumeRemaining = 0f;
                    foreach (TerrainBlockFraction fraction in fractions)
                    {
                        float fractionDigFactor = 1f / fraction.volumeFraction;
                        float dig = digShare * fractionDigFactor;
                        fraction.volumeFraction -= dig;
                        if (fraction.volumeFraction < 0f)
                        {//fully dug, could be remaining digging to apply to other fractions
                            float remainder = -(fraction.volumeFraction);
                            volumeRemaining += remainder;
                            fraction.volumeFraction = 0f;
                        }
                    }
                }
            }
            if(terrainWorksite.GetExistentFractions().Count == 0)
            {//fully dug, destroy
                TerrainBuilder.Instance.Dig(terrainWorksite.data.pos);
            }
        }
    }
}