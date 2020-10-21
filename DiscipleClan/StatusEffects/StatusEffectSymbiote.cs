using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectSymbiote : StatusEffectState
    {
        public const string statusId = "symbiote";
        public int lastBuff = 0;
        public int lastCharNum = 0;

        protected override IEnumerator OnTriggered(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            // Damage
            GetAssociatedCharacter().DebuffDamage(lastBuff);
            lastBuff = GetEffectMagnitude(0);
            GetAssociatedCharacter().BuffDamage(GetEffectMagnitude(0));

            // HP
            ProviderManager.TryGetProvider<RoomManager>(out RoomManager roomManager);
            int numUnits = roomManager.GetRoom(GetAssociatedCharacter().GetCurrentRoomIndex()).GetNumCharacters(Team.Type.Monsters, false)-1;
            if (numUnits != lastCharNum)
            {
                if (numUnits > lastCharNum)
                {
                    if (GetAssociatedCharacter().GetMaxHP() > 10)
                        yield return GetAssociatedCharacter().DebuffMaxHP((numUnits - lastCharNum) * 10);
                    else
                        GetAssociatedCharacter().Sacrifice(new CardState());
                }
            }
            else if (numUnits < lastCharNum)
                yield return GetAssociatedCharacter().BuffMaxHP((lastCharNum - numUnits) * 10);

            lastCharNum = numUnits;
            yield break;
        }

        public override int GetEffectMagnitude(int stacks)
        {
            RoomManager roomManager;
            ProviderManager.TryGetProvider<RoomManager>(out roomManager);

            var capacityInfo = roomManager.GetRoom(GetAssociatedCharacter().GetCurrentRoomIndex()).GetCapacityInfo(Team.Type.Monsters);
            int difference = capacityInfo.count;

            // There are empty spaces on the floor
            if (difference > 0)
            {

            }
            // We've taken up extra spaces on the floor
            else
            {

            }
            
            return capacityInfo.count * 5;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectSymbiote).AssemblyQualifiedName,
                TriggerStage = StatusEffectData.TriggerStage.OnPreCharacterTrigger,
                StatusId = statusId,
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                IconPath = "chrono/Status/card-discard.png",
                IsStackable = false,
                ShowStackCount = false,
            }.Build();
        }
    }
}
