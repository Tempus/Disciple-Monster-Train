using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.StatusEffects
{
    class StatusEffectSlow : StatusEffectState
    {
        public const string statusId = "slow";

        public override void OnStacksAdded(CharacterState character, int numStacksAdded)
        {
            if (character.HasStatusEffect("ambush"))
            {
                character.RemoveStatusEffect("ambush", false, 1, true);
                character.RemoveStatusEffect("slow", false, 1, false);
                character.UpdateCharacterStateUI();
            }
        }

        public override bool TestTrigger(InputTriggerParams inputTriggerParams, OutputTriggerParams outputTriggerParams)
        {
            if (inputTriggerParams.associatedCharacter == null) { return true; }
            if (inputTriggerParams.associatedCharacter.HasStatusEffect("ambush"))
            {
                inputTriggerParams.associatedCharacter.RemoveStatusEffect("ambush", false, 1, false);
                inputTriggerParams.associatedCharacter.RemoveStatusEffect("slow", false, 1, true);
                inputTriggerParams.associatedCharacter.UpdateCharacterStateUI();
            }
            return true;
        }

        public static void Make()
        {
            new StatusEffectDataBuilder
            {
                StatusEffectStateName = typeof(StatusEffectSlow).AssemblyQualifiedName,
                StatusId = "slow",
                TriggerStage = StatusEffectData.TriggerStage.OnPreAttacking,
                DisplayCategory = StatusEffectData.DisplayCategory.Persistent,
                RemoveStackAtEndOfTurn = false,
                IconPath = "chrono/Clan Assets/clan_32.png",
            }.Build();
        }
    }


    // Gotta implement Slow and Enemy Quick - it's real chunky

    //[HarmonyPatch(typeof(CombatManager), "DoUnitCombat")]
    //class RedoUnitCombat
    //{
    //    // Creates and registers card data for each card class
    //    // private IEnumerator DoUnitCombat(RoomState room, MonsterManager monsterManager, HeroManager heroManager)
    //    static bool Prefix(CombatManager __instance, RoomState room, MonsterManager monsterManager, HeroManager heroManager)
    //    {
    //        List<CharacterState> unitCombatHeroes = new List<CharacterState>();
    //        List<CharacterState> unitCombatMonsters = new List<CharacterState>();

    //        // Add all the characters in the room to the list
    //        room.AddCharactersToList(unitCombatMonsters, Team.Type.Monsters);
    //        room.AddCharactersToList(unitCombatHeroes, Team.Type.Heroes);

    //        // Set the room selection to the current room
    //        bool flag = heroManager.GetOuterTrainBossCharacter() != null && heroManager.GetOuterTrainBossCharacter().GetCurrentRoomIndex() == room.GetRoomIndex();
    //        if (unitCombatMonsters.Count > 0 && ((unitCombatHeroes.Count > 0) | flag))
    //        {
    //            yield return roomUI.SetSelectedRoom(room.GetRoomIndex());
    //        }

    //        // Time for Battle -> Quick, Regular, Slow

    //        // Ambush/Quick effects
    //        List<CharacterState> ambushUnits = new List<CharacterState>();

    //        // Enemy (Hero) quick turn
    //        room.GetCharactersWithStatus(Team.Type.Heroes, "ambush", out List<CharacterState> charactersWithStatus);
    //        if (charactersWithStatus.Count > 0)
    //        {
    //            foreach (CharacterState Heroes in charactersWithStatus)
    //            {
    //                StatusEffectState.InputTriggerParams inputTriggerParams = new StatusEffectState.InputTriggerParams(__instance);
    //                StatusEffectState.OutputTriggerParams outputParams = new StatusEffectState.OutputTriggerParams();
    //                yield return statusEffectManager.TriggerStatusEffectOnUnit(StatusEffectData.TriggerStage.OnAmbush, Heroes, inputTriggerParams, outputParams);
    //                if (outputParams.statusTriggered)
    //                {
    //                    ambushUnits.Add(Heroes);
    //                    yield return RunUnitTurn(Heroes, heroManager, monsterManager, room.GetRoomIndex(), allowTheft: true);
    //                }
    //            }
    //            // Reset the monsters incase any have died.
    //            unitCombatMonsters.Clear();
    //            room.AddCharactersToList(unitCombatMonsters, Team.Type.Monsters);
    //        }

    //        // Player (Monster) quick turn
    //        charactersWithStatus.Clear();
    //        room.GetCharactersWithStatus(Team.Type.Monsters, "ambush", out charactersWithStatus);
    //        if (charactersWithStatus.Count > 0)
    //        {
    //            foreach (CharacterState monster in charactersWithStatus)
    //            {
    //                StatusEffectState.InputTriggerParams inputTriggerParams = new StatusEffectState.InputTriggerParams(__instance);
    //                StatusEffectState.OutputTriggerParams outputParams = new StatusEffectState.OutputTriggerParams();
    //                yield return statusEffectManager.TriggerStatusEffectOnUnit(StatusEffectData.TriggerStage.OnAmbush, monster, inputTriggerParams, outputParams);
    //                if (outputParams.statusTriggered)
    //                {
    //                    ambushUnits.Add(monster);
    //                    yield return RunUnitTurn(monster, monsterManager, heroManager, room.GetRoomIndex(), allowTheft: false);
    //                }
    //            }
    //            unitCombatHeroes.Clear();
    //            room.AddCharactersToList(unitCombatHeroes, Team.Type.Heroes);
    //        }

    //        // Make sure we don't do Slowed characters now
    //        room.GetCharactersWithStatus(Team.Type.Monsters | Team.Type.Heroes, "slow", out ambushUnits);

    //        // Enemy (Hero) regular turn
    //        for (int h2 = 0; h2 < unitCombatHeroes.Count; h2++)
    //        {
    //            if (!ambushUnits.Contains(unitCombatMonsters[h2]))
    //            {
    //                yield return RunUnitTurn(unitCombatHeroes[h2], heroManager, monsterManager, room.GetRoomIndex(), allowTheft: true);
    //            }
    //        }
    //        unitCombatMonsters.Clear();
    //        room.AddCharactersToList(unitCombatMonsters, Team.Type.Monsters);

    //        // Player (Monster) regular turn
    //        for (int h2 = 0; h2 < unitCombatMonsters.Count; h2++)
    //        {
    //            if (!ambushUnits.Contains(unitCombatMonsters[h2]))
    //            {
    //                yield return RunUnitTurn(unitCombatMonsters[h2], monsterManager, heroManager, room.GetRoomIndex(), allowTheft: false);
    //            }
    //        }
    //        unitCombatMonsters.Clear();
    //        unitCombatHeroes.Clear();
    //        room.AddCharactersToList(unitCombatMonsters, Team.Type.Monsters);
    //        room.AddCharactersToList(unitCombatHeroes, Team.Type.Heroes);

    //        // Enemy (Hero) slow turn
    //        charactersWithStatus.Clear();
    //        room.GetCharactersWithStatus(Team.Type.Heroes, "slow", out charactersWithStatus);
    //        for (int h2 = 0; h2 < charactersWithStatus.Count; h2++)
    //        {
    //            yield return RunUnitTurn(charactersWithStatus[h2], heroManager, monsterManager, room.GetRoomIndex(), allowTheft: true);
    //        }

    //        // Player (Monster) slow turn
    //        charactersWithStatus.Clear();
    //        room.GetCharactersWithStatus(Team.Type.Monsters, "slow", out charactersWithStatus);
    //        for (int h2 = 0; h2 < charactersWithStatus.Count; h2++)
    //        {
    //            yield return RunUnitTurn(charactersWithStatus[h2], monsterManager, heroManager, room.GetRoomIndex(), allowTheft: false);
    //        }

    //        // Cleanup
    //        if (!saveManager.PreviewMode)
    //        {
    //            yield return DoForegroundMoveAnimations(TargetHelper.SetAttackInfo(attackInfo, heroManager, monsterManager, room.GetRoomIndex()));
    //        }
    //        unitCombatHeroes.Clear();
    //        unitCombatMonsters.Clear();
    //    }
    //}
}
