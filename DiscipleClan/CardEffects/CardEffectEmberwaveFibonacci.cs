using MonsterTrainModdingAPI.Managers;
using System.Collections;

namespace DiscipleClan.CardEffects
{
    class CardEffectEmberwaveFibonacci : CardEffectBase
    {

        public override bool TestEffect(
          CardEffectState cardEffectState,
          CardEffectParams cardEffectParams)
        {
            int intInRange = GetDamageAmount(cardEffectState);
            bool flag2 = true;
            if (cardEffectState.GetTargetMode() == TargetMode.DropTargetCharacter)
                flag2 = cardEffectParams.targets.Count > 0;
            return intInRange >= 0 && flag2;
        }

        private int GetDamageAmount(CardEffectState cardEffectState)
        {
            PlayerManager playerManager;
            ProviderManager.TryGetProvider<PlayerManager>(out playerManager);

            int fibonacci = Fibonacci(playerManager.GetEnergy()+1);

            return cardEffectState.GetParamInt() + fibonacci;
        }
        
        public static int Fibonacci(int n)
        {
            if (n == 0) return 0;
            else if (n == 1) return 1;
            else
            {
                return Fibonacci(n - 1) + Fibonacci(n - 2);
            }
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
            int damageAmount = GetDamageAmount(cardEffectState);
            resultantDamage = target.GetDamageToTarget(damageAmount, (CharacterState)null, card, out int _, Damage.Type.Default);
            return resultantDamage >= target.GetHP();
        }

        public override string GetCardText(CardEffectState cardEffectState, RelicManager relicManager = null)
        {
            LocalizationUtil.GeneratedTextDisplay = LocalizationUtil.GeneratedTextDisplayType.Show;
            return "Emberwave_Fib".Localize(new LocalizedIntegers(GetDamageAmount(cardEffectState)));
        }

        public override string GetHintText(CardEffectState cardEffectState, CharacterState selfTarget)
        {
            return "CardTraitScalingAddDamage_CurrentScaling_CardText".Localize(new LocalizedIntegers(GetDamageAmount(cardEffectState)));
        }
    }
}
