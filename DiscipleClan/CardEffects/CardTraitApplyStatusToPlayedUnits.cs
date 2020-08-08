using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using System.Collections;
using System.Linq;

namespace DiscipleClan.CardEffects
{
    public class CardTraitApplyStatusToPlayedUnits : CardTraitState
    {
        static public CardUpgradeState upgrade;

        public override void OnCardDrawn(CardState thisCard, CardManager cardManager, PlayerManager playerManager, MonsterManager monsterManager)
        {
            if (upgrade == null)
            {
                API.Log(BepInEx.Logging.LogLevel.All, "Creating first time upgrade");
                var upgradeBuilder = new CardUpgradeDataBuilder
                {
                     HideUpgradeIconOnCard = true,
                };

                upgradeBuilder.StatusEffectUpgrades.AddRange(GetParamStatusEffects().ToList());

                upgrade = new CardUpgradeState();
                upgrade.Setup(upgradeBuilder.Build());
            }

            API.Log(BepInEx.Logging.LogLevel.All, "Activate Chronomancy");

            foreach (CardState item in cardManager.GetAllCards())
                if (item.IsMonsterCard() && !item.GetTemporaryCardStateModifiers().HasUpgrade(upgrade))
                    item.GetTemporaryCardStateModifiers().AddUpgrade(upgrade);
        }

        public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, CardManager cardManager, RelicManager relicManager, CombatManager combatManager)
        {
            foreach (CardState item in cardManager.GetAllCards())
                if (item.GetTemporaryCardStateModifiers().HasUpgrade(upgrade))
                    item.GetTemporaryCardStateModifiers().RemoveUpgrade(upgrade);

            yield break;
        }

        //public override string GetCardText()
        //{
        //    return LocalizeTraitKey("MustPlayOnPyre_Title");
        //}
    }
}
