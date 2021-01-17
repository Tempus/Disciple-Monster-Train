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

        public static Dictionary<CharacterState, int> energyData = new Dictionary<CharacterState, int>(40);

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

            ProviderManager.TryGetProvider(out CardManager cardManager);
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

            // Iterate over the playable rooms from top to bottom
            //int v = 0;
            for (int i = 2; i >= 0; i--)
            {
                List<CharacterState> charList = new List<CharacterState>();
                ProviderManager.CombatManager.GetMonsterManager().AddCharactersInRoomToList(charList, i);
                foreach (var unit in charList)
                {
                    // We have to communicate some data about this trigger firing to the effect
                    //     To do this, we attach it to the CharacterState via Dictionary and hope for the best
                    //     If this triggers for a single unit multiple times it overwrites, but that shouldn't happen before the effect
                    if (OnGainEmber.energyData.ContainsKey(unit))
                        OnGainEmber.energyData.Remove(unit);

                    OnGainEmber.energyData.Add(unit, addEnergy);
                    CustomTriggerManager.QueueTrigger(OnGainEmber.OnGainEmberCharTrigger, unit);

                    //Trainworks.Trainworks.Log(BepInEx.Logging.LogLevel.All, "Trigger Queued for " + unit.name + "(Room " + i.ToString() + ", #" + v.ToString() + ")");
                    //v++;
                }
            }
        }
    }

    public interface IRelicEffectOnGainEmber
    {
        void OnGainEmber(int addAmount);
    }

}