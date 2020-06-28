using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DiscipleClan.Cards.CardEffects
{
    class CardEffectPyreAttack : CardEffectBase
    {
        public override bool TestEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            var pyreroom = GameObject.FindObjectOfType<PyreRoomState>() as PyreRoomState;
            int pyreAttack = 0;
            pyreroom.TryGetPyreAttack(out pyreAttack);

            int intInRange = pyreAttack;
            bool flag1 = pyreAttack > 0;
            bool flag2 = true;
            if (cardEffectState.GetTargetMode() == TargetMode.DropTargetCharacter)
                flag2 = cardEffectParams.targets.Count > 0;
            return intInRange >= 0 & flag2 & flag1;
        }

        private int GetDamageAmount(CardEffectState cardEffectState, CharacterState selfTarget)
        {
            var pyreroom = GameObject.FindObjectOfType<PyreRoomState>() as PyreRoomState;
            int pyreAttack = 0;
            pyreroom.TryGetPyreAttack(out pyreAttack);
            return pyreAttack;
        }

        public override IEnumerator ApplyEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            int damageAmount = this.GetDamageAmount(cardEffectState, cardEffectParams.selfTarget);
            if (cardEffectState.GetTargetMode() == TargetMode.Room)
                cardEffectParams.combatManager.IgnoreDuplicateSounds(true, true);
            for (int i = 0; i < cardEffectParams.targets.Count; ++i)
            {
                CharacterState target = cardEffectParams.targets[i];
                yield return (object)cardEffectParams.combatManager.ApplyDamageToTarget(damageAmount, target, new CombatManager.ApplyDamageToTargetParameters()
                {
                    playedCard = cardEffectParams.playedCard,
                    finalEffectInSequence = cardEffectParams.finalEffectInSequence,
                    relicState = cardEffectParams.sourceRelic,
                    selfTarget = cardEffectParams.selfTarget,
                    vfxAtLoc = cardEffectState.GetAppliedVFX(),
                    showDamageVfx = cardEffectParams.allowPlayingDamageVfx
                });
            }
            cardEffectParams.combatManager.IgnoreDuplicateSounds(false, false);
        }

        public bool WillEffectKillTarget(
          CharacterState target,
          CardState card,
          CardEffectState cardEffectState,
          out int resultantDamage)
        {
            int damageAmount = this.GetDamageAmount(cardEffectState, (CharacterState)null);
            resultantDamage = target.GetDamageToTarget(damageAmount, (CharacterState)null, card, out int _, Damage.Type.Default);
            return resultantDamage >= target.GetHP();
        }

        public override string GetHintText(CardEffectState cardEffectState, CharacterState selfTarget)
        {
            return "CardTraitScalingAddDamage_CurrentScaling_CardText".Localize((ILocalizationParameterContext)new LocalizedIntegers(new int[1]
            {
      this.GetDamageAmount(cardEffectState, selfTarget)
            }));
        }
    }
}
