using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class RoomStateModifierRewind : RoomStateModifierBase, IRoomStateModifier, IRoomStateCardManagerModifier, IProvider, IClient
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
            DepInjector.MapProvider<CardManager>(provider, ref this.cardManager);
        }

        public void CardDiscarded(CardState cardState)
        {
            if (cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Spell) +
                cardManager.GetCardStatistics().GetNumCardsPlayedThisTurnOfType(CardType.Monster) < numOfCards)
            {
                cardManager.DrawSpecificCard(cardState);
            }
        }

        public void CardDrawn(CardState cardState)
        {
            throw new NotImplementedException();
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