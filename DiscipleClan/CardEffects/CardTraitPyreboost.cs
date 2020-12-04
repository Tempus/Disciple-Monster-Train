using DiscipleClan.Artifacts;
using HarmonyLib;
using Trainworks;
using Trainworks.Builders;
using Trainworks.Managers;

namespace DiscipleClan.CardEffects
{
    public class CardTraitPyreboost : CardTraitState
    {
        public int GetPyreboostCount()
        {
            int count = 0;
            foreach (var trait in GetCard().GetTraitStates())
            {
                if (trait.GetTraitStateName().Contains("Pyreboost"))
                    count++;
            }

            foreach (var cardUpgrade in GetCard().GetCardStateModifiers().GetCardUpgrades())
            {
                foreach (var trait in cardUpgrade.GetTraitDataUpgrades())
                {
                    if (trait.GetTraitStateName().Contains("Pyreboost"))
                        count++;
                }
            }

            foreach (var trait in GetCard().GetCardStateModifiers().GetTemporaryTraits())
            {
                if (trait.GetTraitStateName().Contains("Pyreboost"))
                    count++;
            }

            foreach (var cardUpgrade in GetCard().GetTemporaryCardStateModifiers().GetCardUpgrades())
            {
                foreach (var trait in cardUpgrade.GetTraitDataUpgrades())
                {
                    if (trait.GetTraitStateName().Contains("Pyreboost"))
                        count++;
                }
            }

            foreach (var trait in GetCard().GetTemporaryCardStateModifiers().GetTemporaryTraits())
            {
                if (trait.GetTraitStateName().Contains("Pyreboost"))
                    count++;
            }

            return count;
        }

        public int GetTotalPyreDamage()
        {
            int PyreAttack          = ProviderManager.SaveManager.GetDisplayedPyreAttack();
            int PyreNumAttacks      = ProviderManager.SaveManager.GetDisplayedPyreNumAttacks();
            int PyreboostMultiplier = GetPyreboostCount();
            return PyreAttack * PyreNumAttacks * PyreboostMultiplier;
        }

        public override int OnApplyingDamage(ApplyingDamageParameters damageParams)
        {
            int pyreDamage = GetTotalPyreDamage();
            if (pyreDamage == 0)
                return damageParams.damage;

            int extraDamage = GetExtraDamage(damageParams.damageSourceCard);

            return pyreDamage + extraDamage;
        }

        private int GetExtraDamage(CardState thisCard)
        {
            CardStateModifiers cardStateModifiers = thisCard.GetCardStateModifiers();
            CardStateModifiers temporaryCardStateModifiers = thisCard.GetTemporaryCardStateModifiers();
            return CardStateModifiers.GetUpgradedStatValue(CardStateModifiers.GetUpgradedStatValue(0, CardStateModifiers.StatType.Damage, cardStateModifiers), CardStateModifiers.StatType.Damage, temporaryCardStateModifiers);
        }

        public override string GetCardText()
        {
            return LocalizeTraitKey("StatusEffect_pyreboost_CardText");
        }

        public override string GetCurrentEffectText(CardStatistics cardStatistics, SaveManager saveManager, RelicManager relicManager)
        {
            int baseDamage = GetTotalPyreDamage();
            int extraDamage = GetExtraDamage(GetCard());

            // The card text must be different if we're in combat or out of it
            if (cardStatistics != null && cardStatistics.GetIsInActiveBattle())
            {
                // In combat
                if (extraDamage > 0)
                    return string.Format("Pyreboost_CardText_InsideCombat_WithUpgrades".Localize(), extraDamage, "tempUpgradeHighlight", baseDamage);
                else
                    return string.Format("Pyreboost_CardText_InsideCombat_NoUpgrades".Localize(), baseDamage);
            }
            
            // Out of combat
            if (extraDamage > 0)
                return string.Format(("Pyreboost_CardText_OutsideCombat").Localize(), extraDamage, "tempUpgradeHighlight");
            
            return string.Empty;
        }
    }
}
