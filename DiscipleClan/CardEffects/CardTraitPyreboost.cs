using DiscipleClan.Artifacts;
using HarmonyLib;
using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.CardEffects
{
    public class CardTraitPyreboost : CardTraitState
    {
        public CardUpgradeState cardUpgradeState;

        public void OnPyreAttackChange(int PyreAttack, int PyreNumAttacks)
        {
            if (PyreAttack == 0) { return; }
            if (!GetCard().GetTemporaryCardStateModifiers().HasUpgrade(cardUpgradeState))
                GetCard().GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState);

            int PyreboostMultiplier = GetPyreboostCount();

            cardUpgradeState.SetAttackDamage(PyreAttack * PyreNumAttacks * PyreboostMultiplier);
            cardUpgradeState.SetAdditionalHeal(PyreAttack * PyreNumAttacks * PyreboostMultiplier);

            GetCard().UpdateDamageText(null);
        }

        [HarmonyPatch(typeof(CardTraitState), "Setup")]
        class InitForCardTrait
        {
            static void Postfix(CardTraitState __instance)
            {
                CardTraitPyreboost trait;
                if ((trait = (__instance as CardTraitPyreboost)) != null)
                {
                    if (trait.cardUpgradeState == null)
                    {
                        trait.cardUpgradeState = new CardUpgradeState();
                        trait.cardUpgradeState.Setup(new CardUpgradeDataBuilder
                        {
                            UpgradeTitleKey = "StatusEffect_pyreboost_CardText",
                            UpgradeDescriptionKey = "StatusEffect_pyreboost_CardTooltipText"
                        }.Build());

                        trait.GetCard().GetTemporaryCardStateModifiers().AddUpgrade(trait.cardUpgradeState);

                        ProviderManager.SaveManager.pyreAttackChangedSignal.AddListener(trait.OnPyreAttackChange);
                        trait.OnPyreAttackChange(ProviderManager.SaveManager.GetDisplayedPyreAttack(), ProviderManager.SaveManager.GetDisplayedPyreNumAttacks());
                    }
                }
            }
        }

        public override int GetModifiedCost(int cost, CardState thisCard, CardStatistics cardStats, MonsterManager monsterManager)
        {
            OnPyreAttackChange(ProviderManager.SaveManager.GetDisplayedPyreAttack(), ProviderManager.SaveManager.GetDisplayedPyreNumAttacks());
            return cost;
        }

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

        //public override void CreateAdditionalTooltips(TooltipContainer tooltipContainer)
        //{
        //    TooltipUI tooltipUI = tooltipContainer.InstantiateTooltip("PyreboostForCards", TooltipDesigner.TooltipDesignType.Keyword);
        //    string title = LocalizeTraitKey("StatusEffect_pyreboost_CardText");
        //    string body = "StatusEffect_pyreboost_CardTooltipText".Localize();
        //    tooltipUI?.Set(title, body);
        //}
        public override string GetCardText()
        {
            //var basetext = LocalizeTraitKey("StatusEffect_pyreboost_Stack_CardText");
            //if (GetPyreboostCount() > 1)
            //    return string.Format(basetext, GetPyreboostCount());
            
            return LocalizeTraitKey("StatusEffect_pyreboost_CardText");
        }
    }
}
