using MonsterTrainModdingAPI;
using System.Collections.Generic;

namespace DiscipleClan.CardEffects
{
    class CardEffectScryConsumePyrehit :CardEffectScry
    {
        public override void AddDelegate(CardEffectState cardEffectState,
          CardEffectParams cardEffectParams,
          DeckScreen deckScreen)
        {
            deckScreen.AddDeckScreenCardStateChosenDelegate((DeckScreen.CardStateChosenDelegate)(chosenCardState =>
            {
				cardEffectParams.cardManager.MoveToStandByPile(chosenCardState, wasPlayed: false, wasExhausted: true, new RemoveFromStandByCondition(() => CardPile.ExhaustedPile), new CardManager.DiscardCardParams(), HandUI.DiscardEffect.Exhausted);

                var allTargets = new List<CharacterState>();
                cardEffectParams.heroManager.AddCharactersInRoomToList(allTargets, cardEffectParams.GetSelectedRoom().GetRoomIndex());

                API.Log(BepInEx.Logging.LogLevel.All, "Belomancy target count: " + allTargets.Count);

                if (allTargets.Count > 0)
                {
                    for (int i = 0; i < chosenCardState.GetCostWithoutAnyModifications(); i++)
                    {
                        cardEffectParams.combatManager.ApplyDamageToTarget(5, allTargets[0], new CombatManager.ApplyDamageToTargetParameters
                        {
                            damageType = Damage.Type.TowerHeart,
                            playedCard = cardEffectParams.playedCard,
                        });
                    }
                }
                cardEffectParams.screenManager.SetScreenActive(ScreenName.Deck, false, (ScreenManager.ScreenActiveCallback)null);
            }));
        }

        public override string GetTooltipBaseKey()
        {
            return "ScreenDeck_Select_CardEffectRecursion";
        }

    }
}
