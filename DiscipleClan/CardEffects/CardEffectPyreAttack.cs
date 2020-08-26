using System.Collections;
using UnityEngine;

namespace DiscipleClan.CardEffects
{
    class CardEffectPyreAttack : CardEffectBase
    {
        PyreRoomState pyreroom;

        public override bool TestEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            int intInRange = GetDamageAmount();
            bool flag1 = GetDamageAmount() > 0;
            bool flag2 = true;
            if (cardEffectState.GetTargetMode() == TargetMode.DropTargetCharacter)
                flag2 = cardEffectParams.targets.Count > 0;

            return intInRange >= 0 & flag2 & flag1;
        }

        private int GetDamageAmount()
        {
            if (pyreroom == null)
                pyreroom = GameObject.FindObjectOfType<PyreRoomState>() as PyreRoomState;
            int pyreAttack = 0;
            pyreroom.TryGetPyreAttack(out pyreAttack);


            return pyreAttack;
        }

        public override IEnumerator ApplyEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            int damageAmount = this.GetDamageAmount();
            if (cardEffectState.GetTargetMode() == TargetMode.Room)
                cardEffectParams.combatManager.IgnoreDuplicateSounds(true);
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
            cardEffectParams.combatManager.IgnoreDuplicateSounds(false);
        }

        public bool WillEffectKillTarget(
          CharacterState target,
          CardState card,
          CardEffectState cardEffectState,
          out int resultantDamage)
        {
            int damageAmount = GetDamageAmount();
            resultantDamage = target.GetDamageToTarget(damageAmount, (CharacterState)null, card, out int _, Damage.Type.Default);
            return resultantDamage >= target.GetHP();
        }

        public override string GetCardText(CardEffectState cardEffectState, RelicManager relicManager = null)
        {
            int damage = 0;
            try
            {
                damage = GetDamageAmount();
            }
            catch (System.Exception)
            {

            }
            return "Flashfire_EffectDesc".Localize(new LocalizedIntegers(damage));
        }

        public override string GetHintText(CardEffectState cardEffectState, CharacterState selfTarget)
        {
            return "CardTraitScalingAddDamage_CurrentScaling_CardText".Localize(new LocalizedIntegers(GetDamageAmount()));
        }
    }
}
