using Trainworks.Builders;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class RoomStateModifierHoldover : RoomStateModifierBase, IRoomStateModifier
    {
        public CardManager cardManager;
        public int numOfCards;

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.numOfCards = roomModifierData.GetParamInt();
            this.cardManager = GameObject.FindObjectOfType<CardManager>().GetComponent<CardManager>() as CardManager;
            this.cardManager.OnCardPlayedCallback += new CardManager.OnCardPlayedEvent(this.OnPlayedCard);
        }

        private void OnPlayedCard(
              CardState cardState,
              int roomIndex,
              SpawnPoint dropLocation,
              bool fromPlayedCard,
              CombatManager.ApplyPreEffectsVfxAction onPreEffectsFiredVfx,
              CombatManager.ApplyEffectsAction onEffectsFired)
        {
            if (cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) +
                cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Monster) < numOfCards)
            {
                var trait = new CardTraitDataBuilder { TraitStateName = "CardTraitRetain" };
                if (!cardState.HasTemporaryTrait(typeof(CardTraitRetain)))
                {
                    cardState.AddTemporaryTrait(trait.Build(), cardManager);
                    cardState.UpdateCardBodyText();
                }
            }
        }
    }
}