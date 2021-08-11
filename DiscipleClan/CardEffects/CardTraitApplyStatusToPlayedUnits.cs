using Trainworks;
using Trainworks.Builders;
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
                var upgradeBuilder = new CardUpgradeDataBuilder
                {
                     HideUpgradeIconOnCard = true,
                };

                upgradeBuilder.StatusEffectUpgrades.AddRange(GetParamStatusEffects().ToList());

                upgrade = new CardUpgradeState();
                upgrade.Setup(upgradeBuilder.Build());
            }

            foreach (CardState item in cardManager.GetAllCards())
                if (item.IsMonsterCard() && !item.GetTemporaryCardStateModifiers().HasUpgrade(upgrade))
                    item.GetTemporaryCardStateModifiers().AddUpgrade(upgrade);
        }

        public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, CardManager cardManager, RelicManager relicManager, CombatManager combatManager, RoomManager roomManager, SaveManager saveManager)
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
