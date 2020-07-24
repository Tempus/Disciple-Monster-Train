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

            cardUpgradeState.SetAttackDamage(PyreAttack * PyreNumAttacks);
            cardUpgradeState.SetAdditionalHeal(PyreAttack * PyreNumAttacks);

            GetCard().UpdateDamageText(null);
            GetCard().UpdateCardBodyText();
        }

        [HarmonyPatch(typeof(CardTraitState), "Setup")]
        class InitForCardTrait
        {
            static void Postfix(CardTraitState __instance)
            {
                CardTraitPyreboost trait;
                if ((trait = (__instance as CardTraitPyreboost)) != null)
                {
                    trait.cardUpgradeState = new CardUpgradeState();
                    trait.cardUpgradeState.Setup(new CardUpgradeDataBuilder
                    {
                        UpgradeTitleKey = "StatusEffect_pyreboost_CardText",
                        UpgradeDescriptionKey = "StatusEffect_pyreboost_CardTooltipText"
                    }.Build());

                    trait.GetCard().GetTemporaryCardStateModifiers().AddUpgrade(trait.cardUpgradeState);
                    trait.GetCard().UpdateCardBodyText();

                    ProviderManager.SaveManager.pyreAttackChangedSignal.AddListener(trait.OnPyreAttackChange);
                    trait.OnPyreAttackChange(ProviderManager.SaveManager.GetDisplayedPyreAttack(), ProviderManager.SaveManager.GetDisplayedPyreNumAttacks());
                }
            }
        }

        public override int GetModifiedCost(int cost, CardState thisCard, CardStatistics cardStats, MonsterManager monsterManager)
        {
            OnPyreAttackChange(ProviderManager.SaveManager.GetDisplayedPyreAttack(), ProviderManager.SaveManager.GetDisplayedPyreNumAttacks());
            return cost;
        }

        //public override int OnApplyingDamage(ApplyingDamageParameters damageParams)
        //{
        //    OnPyreAttackChange(0, 0);
        //    return damageParams.damage;
        //}

        //public override void CreateAdditionalTooltips(TooltipContainer tooltipContainer)
        //{
        //    TooltipUI tooltipUI = tooltipContainer.InstantiateTooltip("PyreboostForCards", TooltipDesigner.TooltipDesignType.Keyword);
        //    string title = LocalizeTraitKey("StatusEffect_pyreboost_CardText");
        //    string body = "StatusEffect_pyreboost_CardTooltipText".Localize();
        //    tooltipUI?.Set(title, body);
        //}
        public override string GetCardText()
        {
            return LocalizeTraitKey("StatusEffect_pyreboost_CardText");
        }
    }
}
