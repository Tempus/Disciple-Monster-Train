using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class RoomStateModifierHoldover : RoomStateModifierBase, IRoomStateModifier, IProvider, IClient
    {
        public CardManager cardManager;
        public int numOfCards;

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            this.numOfCards = roomModifierData.GetParamInt();
        }

        public void NewProviderAvailable(IProvider provider)
        {
            if (DepInjector.MapProvider<CardManager>(provider, ref this.cardManager))
                this.cardManager.OnCardPlayedCallback += new CardManager.OnCardPlayedEvent(this.OnPlayedCard);
        }

        private void OnPlayedCard(
              CardState cardState,
              int roomIndex,
              SpawnPoint dropLocation,
              CombatManager.ApplyPreEffectsVfxAction onPreEffectsFiredVfx,
              CombatManager.ApplyEffectsAction onEffectsFired)
        {
            if (cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) +
                cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Monster) < numOfCards)
            {
                var trait = new CardTraitDataBuilder { TraitStateName = "CardTraitRetain" };
                cardState.AddTemporaryTrait(trait.Build(), cardManager);
            }
        }

        public void ProviderRemoved(IProvider removeProvider)
        {
            throw new NotImplementedException();
        }

        public void NewProviderFullyInstalled(IProvider newProvider)
        {
            throw new NotImplementedException();
        }
    }
}