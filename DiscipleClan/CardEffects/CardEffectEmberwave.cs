using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class CardEffectEmberwave : CardEffectBase
    {

        public override bool TestEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            int intInRange = GetDamageAmount(cardEffectState);
            bool flag1 = GetDamageAmount(cardEffectState) > 0;
            bool flag2 = true;
            if (cardEffectState.GetTargetMode() == TargetMode.DropTargetCharacter)
                flag2 = cardEffectParams.targets.Count > 0;
            return intInRange >= 0 & flag2 & flag1;
        }

        private int GetDamageAmount(CardEffectState cardEffectState)
        {
            PlayerManager playerManager;
            ProviderManager.TryGetProvider<PlayerManager>(out playerManager);

            int num = cardEffectState.GetIntInRange() * playerManager.GetEnergy();

            return num;
        }

        public override IEnumerator ApplyEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            int damageAmount = this.GetDamageAmount(cardEffectState);
            if (cardEffectState.GetTargetMode() == TargetMode.Room)
                cardEffectParams.combatManager.IgnoreDuplicateSounds(true, true);
            for (int i = 0; i < cardEffectParams.targets.Count; ++i)
            {
                CharacterState target = cardEffectParams.targets[i];

                for (int j = 0; j < cardEffectParams.playerManager.GetEnergy(); j++)
                {
                    yield return (object)cardEffectParams.combatManager.ApplyDamageToTarget(cardEffectState.GetIntInRange(), target, new CombatManager.ApplyDamageToTargetParameters()
                    {
                        playedCard = cardEffectParams.playedCard,
                        finalEffectInSequence = cardEffectParams.finalEffectInSequence,
                        relicState = cardEffectParams.sourceRelic,
                        selfTarget = cardEffectParams.selfTarget,
                        vfxAtLoc = cardEffectState.GetAppliedVFX(),
                        showDamageVfx = cardEffectParams.allowPlayingDamageVfx
                    });
                }

            }
            cardEffectParams.combatManager.IgnoreDuplicateSounds(false, false);
        }

        public bool WillEffectKillTarget(
          CharacterState target,
          CardState card,
          CardEffectState cardEffectState,
          out int resultantDamage)
        {
            int damageAmount = GetDamageAmount(cardEffectState);
            resultantDamage = target.GetDamageToTarget(damageAmount, (CharacterState)null, card, out int _, Damage.Type.Default);
            return resultantDamage >= target.GetHP();
        }

        public override string GetHintText(CardEffectState cardEffectState, CharacterState selfTarget)
        {
            return "CardTraitScalingAddDamage_CurrentScaling_CardText".Localize(new LocalizedIntegers(GetDamageAmount(cardEffectState)));
        }
    }
}
