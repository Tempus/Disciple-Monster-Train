using DiscipleClan.CardEffects;
using HarmonyLib;
using Trainworks;
using Trainworks.Enums.MTTriggers;
using Trainworks.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Triggers
{
    public static class OnGainEmber
    {
        public static CharacterTrigger OnGainEmberCharTrigger = new CharacterTrigger("OnGainEmber");
        public static CardTrigger OnGainEmberCardTrigger = new CardTrigger("OnGainEmber");

        static OnGainEmber()
        {
            CustomTriggerManager.AssociateTriggers(OnGainEmberCardTrigger, OnGainEmberCharTrigger);
        }
    }

    [HarmonyPatch(typeof(PlayerManager), "AddEnergy")]
    class EnergyAddTrigger
    {
        static void Postfix(PlayerManager __instance, int addEnergy)
        {
            if (addEnergy == 0)
                return;

            Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Energy boost: " + addEnergy);

            MonsterManager monsterManager;
            ProviderManager.TryGetProvider<MonsterManager>(out monsterManager);

            List<CharacterState> units = new List<CharacterState>();
            monsterManager.AddCharactersInTowerToList(units);

            CustomTriggerManager.QueueAndRunTrigger(OnGainEmber.OnGainEmberCharTrigger, units.ToArray(), true, true, null, addEnergy);

            CardManager cardManager;
            ProviderManager.TryGetProvider<CardManager>(out cardManager);
            foreach (var card in cardManager.GetHand())
            {
                CustomTriggerManager.FireCardTriggers(OnGainEmber.OnGainEmberCardTrigger, card, -1, true, null, addEnergy, null);
            }

            foreach (var relic in ProviderManager.SaveManager.GetCollectedRelics())
            {
                foreach (var effect in relic.GetEffectsOfType<RelicEffectPyreDamageOnEmber>())
                {
                    if (effect == null)
                        continue;
                    effect.OnGainEmber(addEnergy);
                }
            }
        }
    }

    public interface IRelicEffectOnGainEmber
    {
        void OnGainEmber(int addAmount);
    }

}