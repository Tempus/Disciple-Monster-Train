using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;

namespace DiscipleClan.Cards.Triggers
{
    class RelocateTrigger : MTCharacterTrigger
    {
		/*
		Your trigger now is ready to use. To call your custom trigger, go to CustomTriggerManager and Queue it up. 
		CustomTriggerManager also allows you to GetTrigger, just insert it onto your character
		Btw if your triggering doesn’t work make sure to check out your effect’s preview mode
		If it’s false it may not trigger when you want it to
		*/
    }

    // Gotta patch in places to call this trigger from... gonna be OnSpawnPointChange

    // [HarmonyPatch(typeof(CombatManager), "DoUnitCombat")]
    // class RedoUnitCombat
    // {
    //     // Creates and registers card data for each card class
    //     // private IEnumerator DoUnitCombat(RoomState room, MonsterManager monsterManager, HeroManager heroManager)
    //     static bool Prefix(CombatManager __instance, RoomState room, MonsterManager monsterManager, HeroManager heroManager)
    //     {
}