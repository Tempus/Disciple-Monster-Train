using System.Collections;

namespace DiscipleClan.CardEffects
{
    class CardEffectAddWard : CardEffectBase
    {
        public override IEnumerator ApplyEffect(
            CardEffectState cardEffectState,
            CardEffectParams cardEffectParams)
        {
            if (cardEffectState.SaveManager.PreviewMode) { yield break; }

            int paramInt = cardEffectState.GetParamInt();
            string wardType = cardEffectState.GetParamStr();
            WardState wardState;
            switch (wardType)
            {
                case "WardStateShifter":
                    wardState = new WardStateShifter();
                    wardState.power = paramInt;
                    break;
                case "WardStatePower":
                    wardState = new WardStatePower();
                    wardState.power = paramInt;
                    break;
                case "WardStatePyrebound":
                    wardState = new WardStatePyrebound();
                    wardState.power = paramInt;
                    break;
                case "WardStatePyreHarvest":
                    wardState = new WardStatePyreHarvest();
                    wardState.power = paramInt;
                    break;
                case "WardStateRandomDamage":
                    wardState = new WardStateRandomDamage();
                    wardState.power = paramInt;
                    break;
                default:
                    yield break;
            }

            int roomIndex = cardEffectParams.selectedRoom;

            if (cardEffectState.GetParamBool())
            {
                WardManager.AddWardLater(wardState, roomIndex);
                yield break;
            }

            WardManager.AddWard(wardState, roomIndex);
            yield break;
        }
    }
}
